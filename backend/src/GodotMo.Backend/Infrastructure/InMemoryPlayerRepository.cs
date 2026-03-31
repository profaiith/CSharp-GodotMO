using System.Collections.Concurrent;

namespace GodotMo.Backend.Infrastructure;

/// <summary>
/// In-memory implementation for local development.
/// Replace with a distributed persistence layer for real MMO deployment.
/// </summary>
public sealed class InMemoryPlayerRepository : IPlayerRepository
{
    private readonly ConcurrentDictionary<Guid, PlayerState> _players = new();
    private readonly ConcurrentDictionary<string, Guid> _tokenIndex = new();

    public PlayerState CreateGuest(string displayName)
    {
        var normalizedName = string.IsNullOrWhiteSpace(displayName) ? $"Guest-{Random.Shared.Next(1000, 9999)}" : displayName.Trim();
        var state = new PlayerState
        {
            PlayerId = Guid.NewGuid(),
            DisplayName = normalizedName,
            AccessToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
        };

        _players[state.PlayerId] = state;
        _tokenIndex[state.AccessToken] = state.PlayerId;

        return state;
    }

    public PlayerState? GetByPlayerId(Guid playerId) => _players.TryGetValue(playerId, out var state) ? state : null;

    public PlayerState? GetByAccessToken(string accessToken)
    {
        if (!_tokenIndex.TryGetValue(accessToken, out var playerId))
        {
            return null;
        }

        return GetByPlayerId(playerId);
    }

    public void Upsert(PlayerState state)
    {
        _players[state.PlayerId] = state;
        _tokenIndex[state.AccessToken] = state.PlayerId;
    }
}
