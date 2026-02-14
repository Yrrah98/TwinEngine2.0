using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TwinEngine2._0.Engine.Core;
using TwinEngine2._0.Engine.Spatial;
using TwinEngine2._0.Engine.Systems;
using TwinEngine2._0.Game.Components;

namespace TwinEngine2._0.Game.Systems
{
    /// <summary>
    /// System that handles physics simulation including gravity, jumping, and screen boundaries.
    /// Runs after MovementSystem (ExecutionOrder: 150) but before rendering.
    /// Uses QuadTree spatial partitioning for efficient collision detection.
    /// </summary>
    public class PhysicsSystem : ISystem
    {
        private World _world;
        private readonly int _screenWidth;
        private readonly int _screenHeight;
        private readonly Rectangle _worldBounds;

        // Global gravity toggle
        private static bool _gravityEnabled = true;
        private KeyboardState _previousKeyboardState;

        public int ExecutionOrder => 150; // After movement (100), before rendering (1000)

        public PhysicsSystem(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _worldBounds = new Rectangle(0, 0, screenWidth, screenHeight);
            _previousKeyboardState = Keyboard.GetState();
        }

        public void Initialize(World world)
        {
            _world = world;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Check for gravity toggle (P key - press and release)
            var currentKeyboardState = Keyboard.GetState();
            if (currentKeyboardState.IsKeyDown(Keys.P) && _previousKeyboardState.IsKeyUp(Keys.P))
            {
                _gravityEnabled = !_gravityEnabled;
            }
            _previousKeyboardState = currentKeyboardState;

            // Build QuadTree for spatial collision queries
            var quadTree = new QuadTree(_worldBounds, maxEntities: 4, maxDepth: 5);

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

            // Process all entities with GravityComponent
            var gravityComponents = _world.GetComponents<GravityComponent>();
            var entities = _world.GetEntitiesWith<GravityComponent>();

            for (int i = 0; i < gravityComponents.Length; i++)
            {
                var entity = new Entity(entities[i]);

                // Must have PositionComponent and SizeComponent for physics
                if (!_world.HasComponent<PositionComponent>(entity) ||
                    !_world.HasComponent<SizeComponent>(entity))
                {
                    continue;
                }

                ref var gravity = ref _world.GetComponent<GravityComponent>(entity);
                ref var position = ref _world.GetComponent<PositionComponent>(entity);
                ref var size = ref _world.GetComponent<SizeComponent>(entity);

                // Apply gravity if enabled
                if (_gravityEnabled)
                {
                    // Apply gravity acceleration
                    gravity.VerticalVelocity += gravity.Acceleration * deltaTime;

                    // Check for jump input (only if grounded and has InputComponent)
                    if (gravity.IsGrounded && _world.HasComponent<InputComponent>(entity))
                    {
                        ref var input = ref _world.GetComponent<InputComponent>(entity);
                        if (input.Jump)
                        {
                            gravity.VerticalVelocity = gravity.JumpStrength;
                            gravity.IsGrounded = false;
                        }
                    }

                    // Apply vertical velocity to position
                    float newY = position.Position.Y + gravity.VerticalVelocity * deltaTime;

                    // Check for collisions with other entities using QuadTree
                    var collisionInfo = GetCollision(entity, position.Position.X, newY, size.Size, quadTree);

                    if (collisionInfo.HasCollision)
                    {
                        // Resolve collision by positioning entity at contact point
                        if (gravity.VerticalVelocity > 0) // Moving down
                        {
                            // Position entity so its bottom touches the top of the obstacle
                            position.Position.Y = collisionInfo.CollidedBounds.Top - size.Size.Y;
                            gravity.IsGrounded = true;
                        }
                        else if (gravity.VerticalVelocity < 0) // Moving up
                        {
                            // Position entity so its top touches the bottom of the obstacle
                            position.Position.Y = collisionInfo.CollidedBounds.Bottom;
                        }

                        // Stop vertical movement
                        gravity.VerticalVelocity = 0;
                    }
                    else
                    {
                        // No collision, apply the movement
                        position.Position.Y = newY;
                        gravity.IsGrounded = false;
                    }
                }

                // Enforce screen boundaries
                EnforceBoundaries(ref position, ref size, ref gravity);
            }
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

        /// <summary>
        /// Keeps entities within screen bounds and updates grounded state.
        /// </summary>
        private void EnforceBoundaries(ref PositionComponent position, ref SizeComponent size, ref GravityComponent gravity)
        {
            // Bottom boundary (ground)
            if (position.Position.Y + size.Size.Y >= _screenHeight)
            {
                position.Position.Y = _screenHeight - size.Size.Y;
                gravity.VerticalVelocity = 0;
                gravity.IsGrounded = true;
            }

            // Top boundary
            if (position.Position.Y < 0)
            {
                position.Position.Y = 0;
                gravity.VerticalVelocity = 0;
            }

            // Left boundary
            if (position.Position.X < 0)
            {
                position.Position.X = 0;
            }

            // Right boundary
            if (position.Position.X + size.Size.X > _screenWidth)
            {
                position.Position.X = _screenWidth - size.Size.X;
            }
        }
    }
}
