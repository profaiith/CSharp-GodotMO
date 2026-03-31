namespace GodotMo.Shared.Contracts;

/// <summary>
/// Request for creating a temporary guest session.
/// In production you can add account auth fields without breaking world APIs.
/// </summary>
public sealed record CreateGuestSessionRequest(string DisplayName);

/// <summary>
/// Session info returned to the Godot client.
/// Token is intentionally opaque to the client.
/// </summary>
public sealed record SessionResponse(Guid PlayerId, string DisplayName, string AccessToken);
