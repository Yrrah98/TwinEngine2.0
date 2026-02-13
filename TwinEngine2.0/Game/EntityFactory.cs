using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwinEngine2._0.Engine.Core;
using TwinEngine2._0.Game.Components;

namespace TwinEngine2._0.Game
{
    /// <summary>
    /// Factory for creating game entities with predefined component configurations.
    /// Centralizes entity creation logic for reusability and maintainability.
    /// </summary>
    public static class EntityFactory
    {
        /// <summary>
        /// Creates a rectangular sprite entity (e.g., for simple shapes, debug visuals, or placeholder sprites).
        /// </summary>
        /// <param name="world">The world to create the entity in</param>
        /// <param name="position">Position in world space</param>
        /// <param name="size">Size (width, height)</param>
        /// <param name="texture">Texture to render (typically a 1x1 white texture for solid colors)</param>
        /// <param name="color">Tint color for the sprite</param>
        /// <returns>The created entity</returns>
        public static Entity CreateRectangleEntity(
            World world,
            Vector2 position,
            Vector2 size,
            Texture2D texture,
            Color color)
        {
            var entity = world.CreateEntity();

            world.AddComponent(entity, new PositionComponent(position));
            world.AddComponent(entity, new SizeComponent(size));
            world.AddComponent(entity, new SpriteComponent(texture, color));

            return entity;
        }

        /// <summary>
        /// Creates a player entity at the specified position.
        /// Includes input, movement, and physics components for full control.
        /// </summary>
        /// <param name="world">The world to create the entity in</param>
        /// <param name="position">Starting position</param>
        /// <param name="whiteTexture">1x1 white texture for rendering</param>
        /// <returns>The created player entity</returns>
        public static Entity CreatePlayer(World world, Vector2 position, Texture2D whiteTexture)
        {
            var entity = world.CreateEntity();

            // Rendering components
            world.AddComponent(entity, new PositionComponent(position));
            world.AddComponent(entity, new SizeComponent(32, 32));
            world.AddComponent(entity, new SpriteComponent(whiteTexture, Color.White));

            // Input and movement components
            world.AddComponent(entity, new InputComponent());
            world.AddComponent(entity, new VelocityComponent(200f)); // 200 pixels per second

            // Physics components
            world.AddComponent(entity, new GravityComponent(
                acceleration: 980f,    // Earth-like gravity (pixels/s^2)
                jumpStrength: -400f    // Jump velocity (negative = upward)
            ));

            // Collision component
            world.AddComponent(entity, new ColliderComponent(
                new Rectangle((int)position.X, (int)position.Y, 32, 32)
            ));

            return entity;
        }

        /// <summary>
        /// Creates an NPC entity at the specified position.
        /// Renders as a purple rectangle with collision detection.
        /// </summary>
        /// <param name="world">The world to create the entity in</param>
        /// <param name="position">Starting position</param>
        /// <param name="whiteTexture">1x1 white texture for rendering</param>
        /// <returns>The created NPC entity</returns>
        public static Entity CreateNPC(World world, Vector2 position, Texture2D whiteTexture)
        {
            var entity = world.CreateEntity();

            // Rendering components
            world.AddComponent(entity, new PositionComponent(position));
            world.AddComponent(entity, new SizeComponent(16, 16));
            world.AddComponent(entity, new SpriteComponent(whiteTexture, Color.Purple));

            // Collision component
            world.AddComponent(entity, new ColliderComponent(
                new Rectangle((int)position.X, (int)position.Y, 16, 16)
            ));

            return entity;
        }
    }
}
