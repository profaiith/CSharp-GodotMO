using GodotMo.Backend.Abstractions;
using GodotMo.Backend.Infrastructure;
using GodotMo.Shared.Contracts;

namespace GodotMo.Backend.Features.Sessions;

/// <summary>
/// Handles account/session entry points.
/// Keep auth/session concerns separated from world simulation.
/// </summary>
public sealed class SessionFeature : IMmoFeature
{
    public void AddServices(IServiceCollection services)
    {
        // No feature-specific services yet. Method exists for future growth.
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/v1/sessions").WithTags("Sessions");

        group.MapPost("/guest", (CreateGuestSessionRequest request, IPlayerRepository players) =>
        {
            var state = players.CreateGuest(request.DisplayName);
            var response = new SessionResponse(state.PlayerId, state.DisplayName, state.AccessToken);
            return Results.Ok(response);
        });
    }
}
