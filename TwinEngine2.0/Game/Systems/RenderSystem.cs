using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwinEngine2._0.Engine.Core;
using TwinEngine2._0.Engine.Systems;
using TwinEngine2._0.Game.Components;

namespace TwinEngine2._0.Game.Systems
{
    /// <summary>
    /// System responsible for rendering sprites and debug visualizations.
    /// Queries for entities with Position, Size, and Sprite components.
    /// </summary>
    public class RenderSystem : ISystem
    {
        private World _world;
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _whiteTexture;
        private CollisionSystem _collisionSystem;

        public int ExecutionOrder => 1000; // Render last

        public RenderSystem(SpriteBatch spriteBatch, Texture2D whiteTexture)
        {
            _spriteBatch = spriteBatch;
            _whiteTexture = whiteTexture;
        }

        /// <summary>
        /// Sets the collision system for QuadTree visualization.
        /// </summary>
        public void SetCollisionSystem(CollisionSystem collisionSystem)
        {
            _collisionSystem = collisionSystem;
        }

        public void Initialize(World world)
        {
            _world = world;
        }

        public void Update(GameTime gameTime)
        {
            // Rendering happens in Draw(), not Update()
        }

        /// <summary>
        /// Draws all entities with Position, Size, and Sprite components.
        /// Also draws debug visualizations for QuadTree and collisions.
        /// This is called separately from Update() by the game loop.
        /// </summary>
        public void Draw()
        {
            // Get all entities with PositionComponent
            var positions = _world.GetComponents<PositionComponent>();
            var entities = _world.GetEntitiesWith<PositionComponent>();

            _spriteBatch.Begin();

            // Iterate through all positioned entities
            for (int i = 0; i < positions.Length; i++)
            {
                var entity = new Entity(entities[i]);

                // Check if entity has all required components for rendering
                if (!_world.HasComponent<SizeComponent>(entity) ||
                    !_world.HasComponent<SpriteComponent>(entity))
                {
                    continue;
                }

                // Get component references (zero-copy)
                var position = positions[i];
                ref var size = ref _world.GetComponent<SizeComponent>(entity);
                ref var sprite = ref _world.GetComponent<SpriteComponent>(entity);

                // Determine color: Red if colliding, otherwise use sprite color
                Color renderColor = sprite.Color;
                if (_world.HasComponent<ColliderComponent>(entity))
                {
                    ref var collider = ref _world.GetComponent<ColliderComponent>(entity);
                    if (collider.IsColliding)
                    {
                        renderColor = Color.Red;
                    }
                }

                // Draw the sprite
                _spriteBatch.Draw(
                    sprite.Texture,
                    new Rectangle(
                        (int)position.Position.X,
                        (int)position.Position.Y,
                        (int)size.Size.X,
                        (int)size.Size.Y
                    ),
                    renderColor
                );
            }

            // Draw QuadTree visualization
            if (_collisionSystem != null)
            {
                DrawQuadTreeDebug();
            }

            _spriteBatch.End();
        }

        /// <summary>
        /// Draws the QuadTree structure with black lines for debugging.
        /// </summary>
        private void DrawQuadTreeDebug()
        {
            if (_collisionSystem?.QuadTree == null)
                return;

            var nodeBounds = _collisionSystem.QuadTree.GetAllNodeBounds();

            foreach (var bounds in nodeBounds)
            {
                DrawRectangleOutline(bounds, Color.Black, 2);
            }
        }

        /// <summary>
        /// Draws a rectangle outline using a texture.
        /// </summary>
        private void DrawRectangleOutline(Rectangle rect, Color color, int thickness)
        {
            // Top
            _spriteBatch.Draw(_whiteTexture, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
            // Bottom
            _spriteBatch.Draw(_whiteTexture, new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color);
            // Left
            _spriteBatch.Draw(_whiteTexture, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
            // Right
            _spriteBatch.Draw(_whiteTexture, new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color);
        }
    }
}
