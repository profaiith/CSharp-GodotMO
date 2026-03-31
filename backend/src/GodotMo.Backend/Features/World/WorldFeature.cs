using GodotMo.Backend.Abstractions;
using GodotMo.Backend.Infrastructure;
using GodotMo.Shared.Contracts;

namespace GodotMo.Backend.Features.World;

/// <summary>
/// World simulation endpoints.
/// For scale-out, these endpoints should publish commands to a world shard service.
/// </summary>
public sealed class WorldFeature : IMmoFeature
{
    public void AddServices(IServiceCollection services)
    {
        // Feature-specific world services would be registered here (combat, npc ai, etc).
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/v1/world").WithTags("World");

        group.MapGet("/{playerId:guid}", (Guid playerId, IPlayerRepository players) =>
        {
            var state = players.GetByPlayerId(playerId);
            return state is null ? Results.NotFound() : Results.Ok(ToSnapshot(state));
        });

        group.MapPost("/{playerId:guid}/input", (Guid playerId, MovementInputCommand input, IPlayerRepository players) =>
        {
            var state = players.GetByPlayerId(playerId);
            if (state is null)
            {
                return Results.NotFound();
            }

            // Authoritative simulation: apply intent on server, never trust raw client position.
            var direction = input.MoveDirection;
            state.Velocity = new Vector3Dto(
                direction.X * input.MoveSpeed,
                direction.Y * input.MoveSpeed,
                direction.Z * input.MoveSpeed);

            state.Position = new Vector3Dto(
                state.Position.X + state.Velocity.X * input.DeltaTimeSeconds,
                state.Position.Y + state.Velocity.Y * input.DeltaTimeSeconds,
                state.Position.Z + state.Velocity.Z * input.DeltaTimeSeconds);

            state.ServerTick++;
            players.Upsert(state);

            return Results.Ok(ToSnapshot(state));
        });
    }

    private static PlayerSnapshot ToSnapshot(PlayerState state) =>
        new(state.PlayerId, state.DisplayName, state.Position, state.Velocity, state.ServerTick);
}
