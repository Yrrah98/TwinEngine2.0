using TwinEngine2._0.Engine.Components;

namespace TwinEngine2._0.Game.Components
{
    /// <summary>
    /// Component that stores physics properties for gravity and jumping.
    /// Used by PhysicsSystem to simulate vertical movement.
    /// </summary>
    public struct GravityComponent : IComponent
    {
        /// <summary>
        /// Current vertical velocity in pixels per second.
        /// Positive values = falling down, negative = moving up (jumping).
        /// </summary>
        public float VerticalVelocity;

        /// <summary>
        /// Gravity acceleration in pixels per second squared.
        /// Typically a positive value (e.g., 980 for Earth-like gravity).
        /// </summary>
        public float Acceleration;

        /// <summary>
        /// Initial upward velocity when jumping (negative value).
        /// Example: -400 means jump with 400 pixels/sec upward velocity.
        /// </summary>
        public float JumpStrength;

        /// <summary>
        /// Whether the entity is currently touching the ground.
        /// Used to determine if jumping is allowed.
        /// </summary>
        public bool IsGrounded;

        public GravityComponent(float acceleration, float jumpStrength)
        {
            VerticalVelocity = 0f;
            Acceleration = acceleration;
            JumpStrength = jumpStrength;
            IsGrounded = false;
        }
    }
}
