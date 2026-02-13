namespace TwinEngine2._0.Engine.Core
{
    /// <summary>
    /// Manages entity creation and destruction.
    /// Uses simple incrementing IDs (KISS principle).
    /// </summary>
    public class EntityManager
    {
        private uint _nextId;

        public EntityManager()
        {
            _nextId = 0;
        }

        /// <summary>
        /// Creates a new entity with a unique ID.
        /// </summary>
        public Entity CreateEntity()
        {
            return new Entity(_nextId++);
        }

        /// <summary>
        /// Marks an entity as destroyed.
        /// Note: Component cleanup is handled by the World.
        /// </summary>
        public void DestroyEntity(Entity entity)
        {
            // In a more advanced implementation, we could recycle IDs here
            // For now, KISS - just let the World handle component cleanup
        }
    }
}
