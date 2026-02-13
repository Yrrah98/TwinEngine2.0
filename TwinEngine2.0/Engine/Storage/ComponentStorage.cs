using System;
using System.Collections.Generic;
using TwinEngine2._0.Engine.Components;
using TwinEngine2._0.Engine.Core;

namespace TwinEngine2._0.Engine.Storage
{
    /// <summary>
    /// Type-erased storage for all component types.
    /// Manages a SparseSet for each component type.
    /// </summary>
    public class ComponentStorage
    {
        private readonly Dictionary<Type, object> _componentPools;

        public ComponentStorage()
        {
            _componentPools = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Gets or creates a SparseSet for a specific component type.
        /// </summary>
        public SparseSet<T> GetPool<T>() where T : IComponent
        {
            var type = typeof(T);

            if (!_componentPools.TryGetValue(type, out var pool))
            {
                pool = new SparseSet<T>();
                _componentPools[type] = pool;
            }

            return (SparseSet<T>)pool;
        }

        /// <summary>
        /// Adds a component to an entity.
        /// </summary>
        public void AddComponent<T>(Entity entity, T component) where T : IComponent
        {
            var pool = GetPool<T>();
            pool.Add(entity, component);
        }

        /// <summary>
        /// Removes a component from an entity.
        /// </summary>
        public void RemoveComponent<T>(Entity entity) where T : IComponent
        {
            var pool = GetPool<T>();
            pool.Remove(entity);
        }

        /// <summary>
        /// Gets a reference to a component for an entity.
        /// </summary>
        public ref T GetComponent<T>(Entity entity) where T : IComponent
        {
            var pool = GetPool<T>();
            return ref pool.Get(entity);
        }

        /// <summary>
        /// Checks if an entity has a specific component.
        /// </summary>
        public bool HasComponent<T>(Entity entity) where T : IComponent
        {
            var pool = GetPool<T>();
            return pool.Has(entity);
        }

        /// <summary>
        /// Removes all components from an entity.
        /// </summary>
        public void RemoveAllComponents(Entity entity)
        {
            foreach (var pool in _componentPools.Values)
            {
                // Use reflection to call Remove on each pool
                var removeMethod = pool.GetType().GetMethod("Remove");
                removeMethod?.Invoke(pool, new object[] { entity });
            }
        }
    }
}
