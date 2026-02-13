using Microsoft.Xna.Framework;
using TwinEngine2._0.Engine.Components;

namespace TwinEngine2._0.Game.Components
{
    /// <summary>
    /// Component that defines collision bounds for an entity.
    /// Used by CollisionSystem for spatial partitioning and collision detection.
    /// </summary>
    public struct ColliderComponent : IComponent
    {
        /// <summary>
        /// Collision bounds in world space (AABB - Axis-Aligned Bounding Box).
        /// </summary>
        public Rectangle Bounds;

        /// <summary>
        /// Flag indicating if this entity is currently colliding with another entity.
        /// Used for debug visualization (turns red when true).
        /// </summary>
        public bool IsColliding;

        public ColliderComponent(Rectangle bounds)
        {
            Bounds = bounds;
            IsColliding = false;
        }
    }
}
