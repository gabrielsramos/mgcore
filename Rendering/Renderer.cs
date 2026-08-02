using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mgcore.Rendering;

/// <summary>
/// Draws entity trees with a SpriteBatch. Supports world-space rendering under
/// a camera transform and screen-space UI rendering without one.
/// </summary>
public class Renderer
{
    private readonly SpriteBatch _spriteBatch;

    public Renderer(SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
    }

    public void DrawWorld(IEnumerable<RenderEntity> entities, Matrix? transformMatrix)
    {
        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            null,
            null,
            null,
            transformMatrix);
        DrawTree(entities);
        _spriteBatch.End();
    }

    public void DrawUI(IEnumerable<RenderEntity> entities)
    {
        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            null,
            null,
            null,
            null);
        DrawTree(entities);
        _spriteBatch.End();
    }

    private void DrawTree(IEnumerable<RenderEntity> entities)
    {
        foreach (var entity in entities)
        {
            DrawEntity(entity);
            DrawTree(entity.Children);
        }
    }

    private void DrawEntity(RenderEntity entity)
    {
        if (entity.Texture is null)
            return;

        var source = entity.SourceRectangle
            ?? new Rectangle(0, 0, entity.Texture.Width, entity.Texture.Height);
        var origin = entity.Origin
            ?? new Vector2(source.Width / 2f, source.Height / 2f);

        _spriteBatch.Draw(
            entity.Texture,
            entity.WorldPosition,
            source,
            entity.Color,
            0f,
            origin,
            Vector2.One,
            entity.Effects,
            entity.LayerDepth);
    }
}
