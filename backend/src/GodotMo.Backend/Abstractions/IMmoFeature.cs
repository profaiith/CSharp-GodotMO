namespace GodotMo.Backend.Abstractions;

/// <summary>
/// Feature module abstraction.
/// Every gameplay feature (inventory, guilds, matchmaking...) plugs into the host
/// by implementing this interface.
/// </summary>
public interface IMmoFeature
{
    /// <summary>
    /// Register feature-specific dependencies.
    /// Keep constructor injection narrow to prevent feature coupling.
    /// </summary>
    void AddServices(IServiceCollection services);

    /// <summary>
    /// Register HTTP endpoints.
    /// Route groups let each feature evolve independently and stay maintainable.
    /// </summary>
    void MapEndpoints(IEndpointRouteBuilder endpoints);
}
