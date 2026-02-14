using Microsoft.Xna.Framework;
using TwinEngine2._0.Engine.Core;
using TwinEngine2._0.Engine.Spatial;
using TwinEngine2._0.Engine.Systems;
using TwinEngine2._0.Game.Components;

namespace TwinEngine2._0.Game.Systems
{
    /// <summary>
    /// System that updates entity positions based on input and velocity.
    /// Runs after InputSystem (ExecutionOrder: 100) but before rendering.
    /// Uses QuadTree spatial partitioning for efficient collision detection.
    /// </summary>
    public class MovementSystem : ISystem
    {
        private World _world;
        private readonly int _screenWidth;
        private readonly int _screenHeight;

        public int ExecutionOrder => 100; // After input (0), before rendering (1000)

        public MovementSystem(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
        }

        public void Initialize(World world)
        {
            _world = world;
        }

        /// <summary>
        /// Structure to hold collision detection results.
        /// </summary>
        private struct CollisionResult
        {
            public bool HasCollision;
            public Rectangle CollidedBounds;
        }

        /// <summary>
        /// Checks for collision and returns collision info for proper resolution.
        /// Uses QuadTree spatial partitioning to reduce collision checks.
        /// </summary>
        private CollisionResult GetCollision(Entity entity, float x, float y, Vector2 size, QuadTree quadTree)
        {
            Rectangle proposedBounds = new Rectangle((int)x, (int)y, (int)size.X, (int)size.Y);

            // Query QuadTree for nearby entities (broad phase)
            var candidates = quadTree.Query(proposedBounds);

            // Check collision with nearby candidates only (narrow phase)
            foreach (var candidate in candidates)
            {
                // Skip self
                if (candidate.Entity == entity)
                    continue;

                // Check AABB intersection
                if (proposedBounds.Intersects(candidate.Bounds))
                {
                    return new CollisionResult
                    {
                        HasCollision = true,
                        CollidedBounds = candidate.Bounds
                    };
                }
            }

            return new CollisionResult { HasCollision = false };
        }

        public void Update(GameTime gameTime)
        {
            // Get delta time for frame-rate independent movement
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Build QuadTree for spatial collision queries
            var worldBounds = new Rectangle(0, 0, _screenWidth, _screenHeight);
            var quadTree = new QuadTree(worldBounds, maxEntities: 4, maxDepth: 5);

            // Populate QuadTree with all colliders
            var colliderEntities = _world.GetEntitiesWith<ColliderComponent>();
            for (int i = 0; i < colliderEntities.Length; i++)
            {
                var colliderEntity = new Entity(colliderEntities[i]);
                if (_world.HasComponent<PositionComponent>(colliderEntity) &&
                    _world.HasComponent<SizeComponent>(colliderEntity))
                {
                    ref var pos = ref _world.GetComponent<PositionComponent>(colliderEntity);
                    ref var size = ref _world.GetComponent<SizeComponent>(colliderEntity);

                    Rectangle bounds = new Rectangle(
                        (int)pos.Position.X,
                        (int)pos.Position.Y,
                        (int)size.Size.X,
                        (int)size.Size.Y
                    );
                    quadTree.Insert(colliderEntity, bounds);
                }
            }

            // Get all entities with PositionComponent
            var positions = _world.GetComponents<PositionComponent>();
            var entities = _world.GetEntitiesWith<PositionComponent>();

            // Update position for entities that have input and velocity
            for (int i = 0; i < positions.Length; i++)
            {
                var entity = new Entity(entities[i]);

                // Only move entities that have both input and velocity
                if (!_world.HasComponent<InputComponent>(entity) ||
                    !_world.HasComponent<VelocityComponent>(entity))
                {
                    continue;
                }

                // Get component references (all mutable)
                ref var position = ref _world.GetComponent<PositionComponent>(entity);
                ref var input = ref _world.GetComponent<InputComponent>(entity);
                ref var velocity = ref _world.GetComponent<VelocityComponent>(entity);

                // Calculate movement: direction * speed * deltaTime
                Vector2 movement = input.MovementDirection * velocity.Speed * deltaTime;

                // Check for collisions if entity has a collider and size
                if (_world.HasComponent<ColliderComponent>(entity) &&
                    _world.HasComponent<SizeComponent>(entity))
                {
                    ref var size = ref _world.GetComponent<SizeComponent>(entity);

                    // Check horizontal collision
                    if (movement.X != 0)
                    {
                        float newX = position.Position.X + movement.X;
                        var collisionInfo = GetCollision(entity, newX, position.Position.Y, size.Size, quadTree);

                        if (collisionInfo.HasCollision)
                        {
                            // Resolve collision by positioning at contact point
                            if (movement.X > 0) // Moving right
                            {
                                position.Position.X = collisionInfo.CollidedBounds.Left - size.Size.X;
                            }
                            else // Moving left
                            {
                                position.Position.X = collisionInfo.CollidedBounds.Right;
                            }
                        }
                        else
                        {
                            position.Position.X = newX;
                        }
                    }

                    // Check vertical collision (for flying entities or manual movement)
                    if (movement.Y != 0)
                    {
                        float newY = position.Position.Y + movement.Y;
                        var collisionInfo = GetCollision(entity, position.Position.X, newY, size.Size, quadTree);

                        if (collisionInfo.HasCollision)
                        {
                            // Resolve collision by positioning at contact point
                            if (movement.Y > 0) // Moving down
                            {
                                position.Position.Y = collisionInfo.CollidedBounds.Top - size.Size.Y;
                            }
                            else // Moving up
                            {
                                position.Position.Y = collisionInfo.CollidedBounds.Bottom;
                            }
                        }
                        else
                        {
                            position.Position.Y = newY;
                        }
                    }
                }
                else
                {
                    // No collision checking - apply movement directly
                    position.Position += movement;
                }
            }
        }
    }
}
