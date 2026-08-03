using Godot;

namespace LootGoblin;

public class TreeNode(Vector2I position, Vector2I dimensions, int depth)
{
    public Vector2I Position = position;
    public Vector2I Dimensions = dimensions;
    public int Depth = depth;
    public TreeNode Left = null;
    public TreeNode Right = null;
    public bool IsSplitVertical;
    public Room Room;
}