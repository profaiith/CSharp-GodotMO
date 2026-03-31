namespace GodotMo.Backend.Infrastructure;

/// <summary>
/// Repository boundary for player/session state.
/// Swap this out with Redis, SQL, or distributed actor storage when scaling.
/// </summary>
public interface IPlayerRepository
{
    PlayerState CreateGuest(string displayName);
    PlayerState? GetByPlayerId(Guid playerId);
    PlayerState? GetByAccessToken(string accessToken);
    void Upsert(PlayerState state);
}
