using Godot;
using GodotMo.Client.Features;

namespace GodotMo.Client.Bootstrap;

/// <summary>
/// Entry point node for dependency wiring inside Godot.
/// Keep scene setup in one place so onboarding is easier for new contributors.
/// </summary>
public partial class GameBootstrap : Node
{
    [Export] public FeatureRegistry FeatureRegistry = default!;

    public override void _Ready()
    {
        if (FeatureRegistry is null)
        {
            GD.PushError("FeatureRegistry is not assigned in GameBootstrap.");
            return;
        }

        // Register game features here as the project grows.
        GD.Print("GameBootstrap initialized.");
    }
}
