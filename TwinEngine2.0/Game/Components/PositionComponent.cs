using Microsoft.Xna.Framework;
using TwinEngine2._0.Engine.Components;

namespace TwinEngine2._0.Game.Components
{
    /// <summary>
    /// Component that stores an entity's position in 2D space.
    /// </summary>
    public struct PositionComponent : IComponent
    {
        public Vector2 Position;

        public PositionComponent(float x, float y)
        {
            Position = new Vector2(x, y);
        }

        public PositionComponent(Vector2 position)
        {
            Position = position;
        }
    }
}
