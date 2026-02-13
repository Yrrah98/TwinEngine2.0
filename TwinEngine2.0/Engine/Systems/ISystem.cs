using Microsoft.Xna.Framework;
using TwinEngine2._0.Engine.Core;

namespace TwinEngine2._0.Engine.Systems
{
    /// <summary>
    /// Interface for all systems in the ECS.
    /// Systems contain logic and operate on entities with specific components.
    /// </summary>
    public interface ISystem
    {
        /// <summary>
        /// Initializes the system with a reference to the world.
        /// </summary>
        void Initialize(World world);

        /// <summary>
        /// Updates the system each frame.
        /// </summary>
        void Update(GameTime gameTime);

        /// <summary>
        /// Execution order for this system.
        /// Lower numbers execute first.
        /// </summary>
        int ExecutionOrder { get; }
    }
}
