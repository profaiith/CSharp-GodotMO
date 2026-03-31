using Godot;
using GodotMo.Client.Networking;

namespace GodotMo.Client.Player;

/// <summary>
/// 3D FPS controller with backend-authoritative sync hooks.
/// Keeps movement responsive locally while still letting server validate outcomes.
/// </summary>
public partial class FpsCharacterController3D : CharacterBody3D
{
    [Export] public Node3D CameraPivot = default!;
    [Export] public Camera3D PlayerCamera = default!;
    [Export] public float MoveSpeed = 7.5f;
    [Export] public float MouseSensitivity = 0.003f;
    [Export] public float Gravity = 18.0f;
    [Export] public string BackendBaseUrl = "http://localhost:5020";

    private BackendApiClient? _apiClient;
    private Guid _playerId;

    public override async void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;

        _apiClient = new BackendApiClient(BackendBaseUrl);

        // Bootstrap with guest session for prototyping. Swap with real auth later.
        var session = await _apiClient.CreateGuestSessionAsync("Player", CancellationToken.None);
        _playerId = session.PlayerId;

        GD.Print($"Connected as {session.DisplayName} ({session.PlayerId})");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion motion)
        {
            RotateY(-motion.Relative.X * MouseSensitivity);
            CameraPivot.RotateX(-motion.Relative.Y * MouseSensitivity);

            // Clamp vertical look to avoid unnatural camera flips.
            var pivotRotation = CameraPivot.Rotation;
            pivotRotation.X = Mathf.Clamp(pivotRotation.X, Mathf.DegToRad(-80), Mathf.DegToRad(80));
            CameraPivot.Rotation = pivotRotation;
        }
    }

    public override async void _PhysicsProcess(double delta)
    {
        var inputVector = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        var localDirection = new Vector3(inputVector.X, 0, inputVector.Y);
        var worldDirection = (Transform.Basis * localDirection).Normalized();

        Velocity = new Vector3(worldDirection.X * MoveSpeed, Velocity.Y, worldDirection.Z * MoveSpeed);

        if (!IsOnFloor())
        {
            Velocity += Vector3.Down * Gravity * (float)delta;
        }

        MoveAndSlide();

        if (_apiClient is null || _playerId == Guid.Empty)
        {
            return;
        }

        // In production, send at a fixed network tick (e.g., 20hz) with client prediction/reconciliation.
        var snapshot = await _apiClient.SendMovementAsync(_playerId, worldDirection, (float)delta, MoveSpeed, CancellationToken.None);

        // Optional reconciliation hook: lightly correct drift toward server-authoritative position.
        var serverPos = new Vector3(snapshot.Position.X, snapshot.Position.Y, snapshot.Position.Z);
        GlobalPosition = GlobalPosition.Lerp(serverPos, 0.15f);
    }
}
