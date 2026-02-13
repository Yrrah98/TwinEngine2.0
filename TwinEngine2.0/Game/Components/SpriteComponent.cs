using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwinEngine2._0.Engine.Components;

namespace TwinEngine2._0.Game.Components
{
    /// <summary>
    /// Component that stores sprite rendering data.
    /// </summary>
    public struct SpriteComponent : IComponent
    {
        public Texture2D Texture;
        public Color Color;

        public SpriteComponent(Texture2D texture, Color color)
        {
            Texture = texture;
            Color = color;
        }
    }
}
