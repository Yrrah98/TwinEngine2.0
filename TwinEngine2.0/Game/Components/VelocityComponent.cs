using TwinEngine2._0.Engine.Components;

namespace TwinEngine2._0.Game.Components
{
    /// <summary>
    /// Component that stores movement speed for an entity.
    /// Used by MovementSystem to calculate position changes.
    /// </summary>
    public struct VelocityComponent : IComponent
    {
        /// <summary>
        /// Movement speed in pixels per second.
        /// </summary>
        public float Speed;

        public VelocityComponent(float speed)
        {
            Speed = speed;
        }
    }
}
