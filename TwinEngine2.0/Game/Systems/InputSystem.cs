using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TwinEngine2._0.Engine.Core;
using TwinEngine2._0.Engine.Systems;
using TwinEngine2._0.Game.Components;

namespace TwinEngine2._0.Game.Systems
{
    /// <summary>
    /// System that reads keyboard and gamepad input and updates InputComponents.
    /// Runs first (ExecutionOrder: 0) before other systems process the input.
    /// </summary>
    public class InputSystem : ISystem
    {
        private World _world;

        public int ExecutionOrder => 0; // Run first, before movement

        public void Initialize(World world)
        {
            _world = world;
        }

        public void Update(GameTime gameTime)
        {
            // Get keyboard state
            var keyboardState = Keyboard.GetState();

            // Get all entities with InputComponent
            var inputs = _world.GetComponents<InputComponent>();
            var entities = _world.GetEntitiesWith<InputComponent>();

            // Update input for each entity
            for (int i = 0; i < inputs.Length; i++)
            {
                var entity = new Entity(entities[i]);
                ref var input = ref _world.GetComponent<InputComponent>(entity);

                // Calculate movement direction from WASD or Arrow keys
                Vector2 direction = Vector2.Zero;

                // Horizontal input
                if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
                    direction.X -= 1;
                if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
                    direction.X += 1;

                // Vertical input
                if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
                    direction.Y -= 1;
                if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
                    direction.Y += 1;

                // Normalize diagonal movement so it's not faster
                if (direction.LengthSquared() > 0)
                    direction.Normalize();

                input.MovementDirection = direction;

                // Jump input (spacebar)
                input.Jump = keyboardState.IsKeyDown(Keys.Space);
            }
        }
    }
}
