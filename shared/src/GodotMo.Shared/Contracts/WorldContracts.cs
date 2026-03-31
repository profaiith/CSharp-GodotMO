namespace GodotMo.Shared.Contracts;

/// <summary>
/// Snapshot returned by the backend-authoritative simulation.
/// </summary>
public sealed record PlayerSnapshot(Guid PlayerId, string DisplayName, Vector3Dto Position, Vector3Dto Velocity, long ServerTick);

/// <summary>
/// Input command from the client for one simulation frame.
/// Keep this as input intent (movement, look) instead of raw transforms.
/// </summary>
public sealed record MovementInputCommand(Vector3Dto MoveDirection, float DeltaTimeSeconds, float MoveSpeed);
