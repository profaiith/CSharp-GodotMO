using GodotMo.Shared.Contracts;

namespace GodotMo.Backend.Infrastructure;

/// <summary>
/// Backend-authoritative player state.
/// In a real MMO this model would live in a domain assembly and persist via snapshots/events.
/// </summary>
public sealed class PlayerState
{
    public required Guid PlayerId { get; init; }
    public required string DisplayName { get; init; }
    public string AccessToken { get; init; } = string.Empty;

    public Vector3Dto Position { get; set; } = new(0, 1, 0);
    public Vector3Dto Velocity { get; set; } = new(0, 0, 0);
    public long ServerTick { get; set; }
}
