using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mgcore.Rendering;

/// <summary>
/// A node in an entity tree. Holds a local position, optional sprite data,
/// arbitrary string data, and children positioned relative to it.
/// A null <see cref="Texture"/> makes this a container/pivot node — it is
/// skipped visually but its children are still drawn.
/// </summary>
public class RenderEntity
{
    private readonly List<RenderEntity> _children = new();

    public RenderEntity? Parent { get; private set; }
    public IReadOnlyList<RenderEntity> Children => _children;

    public Vector2 Position { get; set; }

    /// <summary>Absolute position, composed by summing positions up the tree.</summary>
    public Vector2 WorldPosition =>
        Parent is null ? Position : Parent.WorldPosition + Position;

    public Texture2D? Texture { get; set; }
    public Rectangle? SourceRectangle { get; set; }
    public Color Color { get; set; } = Color.White;

    /// <summary>Pivot point within the sprite in its own pixels. Null means the center.</summary>
    public Vector2? Origin { get; set; }

    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    public float LayerDepth { get; set; }

    public Dictionary<string, string> Tags { get; } = new();

    public void AddChild(RenderEntity child)
    {
        if (child == this)
            throw new ArgumentException("Cannot add an entity as its own child.", nameof(child));
        if (child.Parent == this)
            return;
        child.Parent?.RemoveChild(child);
        child.Parent = this;
        _children.Add(child);
    }

    public void RemoveChild(RenderEntity child)
    {
        if (_children.Remove(child))
            child.Parent = null;
    }

    public void ClearChildren()
    {
        foreach (var child in _children)
            child.Parent = null;
        _children.Clear();
    }
}
