namespace GodotMo.Client.Features;

/// <summary>
/// Modular feature contract for the Godot client.
/// Mirrors the backend feature pattern to keep both sides consistent.
/// </summary>
public interface IClientFeature
{
    string Name { get; }
    void Initialize();
    void Tick(double delta);
}
