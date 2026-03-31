using Godot;

namespace GodotMo.Client.Features;

/// <summary>
/// Lightweight orchestrator for feature modules.
/// Keeps main scene small and lets teams work on isolated systems.
/// </summary>
public partial class FeatureRegistry : Node
{
    private readonly List<IClientFeature> _features = new();

    public override void _Ready()
    {
        // Add features here as the project grows (inventory UI, chat, quest tracker, etc).
        GD.Print("FeatureRegistry ready. Register client features in code for compile-time safety.");

        foreach (var feature in _features)
        {
            feature.Initialize();
        }
    }

    public override void _Process(double delta)
    {
        foreach (var feature in _features)
        {
            feature.Tick(delta);
        }
    }

    public void Register(IClientFeature feature)
    {
        _features.Add(feature);
    }
}
