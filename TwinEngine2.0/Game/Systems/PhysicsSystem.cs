using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TwinEngine2._0.Engine.Core;
using TwinEngine2._0.Engine.Systems;
using TwinEngine2._0.Game.Components;

namespace TwinEngine2._0.Game.Systems
{
    /// <summary>
    /// System that handles physics simulation including gravity, jumping, and screen boundaries.
    /// Runs after MovementSystem (ExecutionOrder: 150) but before rendering.
    /// </summary>
    public class PhysicsSystem : ISystem
    {
        private World _world;
        private readonly int _screenWidth;
        private readonly int _screenHeight;

        // Global gravity toggle
        private static bool _gravityEnabled = true;
        private KeyboardState _previousKeyboardState;

        public int ExecutionOrder => 150; // After movement (100), before rendering (1000)

        public PhysicsSystem(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
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
                    position.Position.Y += gravity.VerticalVelocity * deltaTime;
                }

                // Enforce screen boundaries
                EnforceBoundaries(ref position, ref size, ref gravity);
            }
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
            else
            {
                gravity.IsGrounded = false;
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
