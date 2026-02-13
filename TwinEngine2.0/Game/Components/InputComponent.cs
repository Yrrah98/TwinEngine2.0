using Microsoft.Xna.Framework;
using TwinEngine2._0.Engine.Components;

namespace TwinEngine2._0.Game.Components
{
    /// <summary>
    /// Component that stores input state for an entity.
    /// Updated by InputSystem each frame based on keyboard/gamepad input.
    /// </summary>
    public struct InputComponent : IComponent
    {
        /// <summary>
        /// Normalized movement direction from input.
        /// Zero vector means no movement input.
        /// </summary>
        public Vector2 MovementDirection;

        /// <summary>
        /// Jump input state. True when jump button (spacebar) is pressed.
        /// </summary>
        public bool Jump;

        public InputComponent()
        {
            MovementDirection = Vector2.Zero;
            Jump = false;
        }
    }
}
