using Godot;

namespace LootGoblin;

public partial class DebugDetails : Node
{
    private bool _debugIsVisible = false;

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("toggle_debug"))
        {
            if (_debugIsVisible)
            {
                HideDetails();
            }
            else
            {
                ShowDetails();
            }
        }
    }

    private void HideDetails()
    {
        GetTree().CallGroup("debug", CanvasItem.MethodName.Hide);
        _debugIsVisible = false;
    }

    private void ShowDetails()
    {
        GetTree().CallGroup("debug", CanvasItem.MethodName.Show);
        _debugIsVisible = true;
    }
}