using Godot;
using GodotMo.Shared.Contracts;
using System.Net.Http.Json;

namespace GodotMo.Client.Networking;

/// <summary>
/// Thin HTTP client adapter.
/// Replace with WebSocket/ENet transport once moving to realtime state streaming.
/// </summary>
public sealed class BackendApiClient
{
    private readonly HttpClient _httpClient;

    public BackendApiClient(string baseUrl)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    public async Task<SessionResponse> CreateGuestSessionAsync(string displayName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/v1/sessions/guest", new CreateGuestSessionRequest(displayName), cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<SessionResponse>(cancellationToken: cancellationToken))!;
    }

    public async Task<PlayerSnapshot> SendMovementAsync(Guid playerId, Vector3 inputDirection, float delta, float speed, CancellationToken cancellationToken)
    {
        var command = new MovementInputCommand(new Vector3Dto(inputDirection.X, inputDirection.Y, inputDirection.Z), delta, speed);
        var response = await _httpClient.PostAsJsonAsync($"/api/v1/world/{playerId}/input", command, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<PlayerSnapshot>(cancellationToken: cancellationToken))!;
    }
}
