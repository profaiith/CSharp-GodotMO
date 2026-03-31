namespace GodotMo.Shared.Contracts;

/// <summary>
/// Engine-agnostic vector contract used between backend and client.
/// Keep DTOs simple so they serialize fast and stay easy to version.
/// </summary>
public sealed record Vector3Dto(float X, float Y, float Z);
