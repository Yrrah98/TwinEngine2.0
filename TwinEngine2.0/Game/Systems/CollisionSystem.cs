using Microsoft.Xna.Framework;
using TwinEngine2._0.Engine.Core;
using TwinEngine2._0.Engine.Spatial;
using TwinEngine2._0.Engine.Systems;
using TwinEngine2._0.Game.Components;

namespace TwinEngine2._0.Game.Systems
{
    /// <summary>
    /// System that detects collisions using QuadTree spatial partitioning.
    /// Runs after physics (ExecutionOrder: 200) but before rendering.
    /// </summary>
    public class CollisionSystem : ISystem
    {
        private World _world;
        private QuadTree _quadTree;
        private readonly Rectangle _worldBounds;

        public int ExecutionOrder => 200; // After physics (150), before rendering (1000)

        public QuadTree QuadTree => _quadTree; // For visualization

        public CollisionSystem(int worldWidth, int worldHeight)
        {
            _worldBounds = new Rectangle(0, 0, worldWidth, worldHeight);
            _quadTree = new QuadTree(_worldBounds, maxEntities: 4, maxDepth: 5);
        }

        public void Initialize(World world)
        {
            _world = world;
        }

        public void Update(GameTime gameTime)
        {
            // Clear and rebuild QuadTree each frame
            _quadTree.Clear();

            // Get all entities with colliders
            var colliders = _world.GetComponents<ColliderComponent>();
            var entities = _world.GetEntitiesWith<ColliderComponent>();

            // Reset collision flags and update bounds based on position/size
            for (int i = 0; i < colliders.Length; i++)
            {
                var entity = new Entity(entities[i]);
                ref var collider = ref _world.GetComponent<ColliderComponent>(entity);

                // Reset collision flag
                collider.IsColliding = false;

                // Update bounds from position and size components
                if (_world.HasComponent<PositionComponent>(entity) &&
                    _world.HasComponent<SizeComponent>(entity))
                {
                    ref var position = ref _world.GetComponent<PositionComponent>(entity);
                    ref var size = ref _world.GetComponent<SizeComponent>(entity);

                    collider.Bounds = new Rectangle(
                        (int)position.Position.X,
                        (int)position.Position.Y,
                        (int)size.Size.X,
                        (int)size.Size.Y
                    );
                }

                // Insert into QuadTree
                _quadTree.Insert(entity, collider.Bounds);
            }

            // Check collisions using QuadTree
            for (int i = 0; i < colliders.Length; i++)
            {
                var entity = new Entity(entities[i]);
                ref var collider = ref _world.GetComponent<ColliderComponent>(entity);

                // Query QuadTree for potential collision candidates
                var candidates = _quadTree.Query(collider.Bounds);

                // Check AABB collision with each candidate
                foreach (var candidate in candidates)
                {
                    // Don't check collision with self
                    if (candidate.Entity == entity)
                        continue;

                    // AABB intersection test
                    if (collider.Bounds.Intersects(candidate.Bounds))
                    {
                        collider.IsColliding = true;

                        // Mark the other entity as colliding too
                        if (_world.HasComponent<ColliderComponent>(candidate.Entity))
                        {
                            ref var otherCollider = ref _world.GetComponent<ColliderComponent>(candidate.Entity);
                            otherCollider.IsColliding = true;
                        }
                    }
                }
            }
        }
    }
}
