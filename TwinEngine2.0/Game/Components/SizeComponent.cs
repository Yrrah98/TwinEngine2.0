using Microsoft.Xna.Framework;
using TwinEngine2._0.Engine.Components;

namespace TwinEngine2._0.Game.Components
{
    /// <summary>
    /// Component that stores an entity's size (width and height).
    /// </summary>
    public struct SizeComponent : IComponent
    {
        public Vector2 Size;

        public SizeComponent(float width, float height)
        {
            Size = new Vector2(width, height);
        }

        public SizeComponent(Vector2 size)
        {
            Size = size;
        }
    }
}
