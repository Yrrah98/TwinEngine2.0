using Microsoft.Xna.Framework;
using TwinEngine2._0.Engine.Core;
using TwinEngine2._0.Engine.Systems;
using TwinEngine2._0.Game.Components;

namespace TwinEngine2._0.Game.Systems
{
    /// <summary>
    /// System that updates entity positions based on input and velocity.
    /// Runs after InputSystem (ExecutionOrder: 100) but before rendering.
    /// </summary>
    public class MovementSystem : ISystem
    {
        private World _world;

        public int ExecutionOrder => 100; // After input (0), before rendering (1000)

        public void Initialize(World world)
        {
            _world = world;
        }

        public void Update(GameTime gameTime)
        {
            // Get delta time for frame-rate independent movement
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

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

                // Update position
                position.Position += movement;
            }
        }
    }
}
