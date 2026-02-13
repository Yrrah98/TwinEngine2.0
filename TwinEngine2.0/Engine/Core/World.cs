using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TwinEngine2._0.Engine.Components;
using TwinEngine2._0.Engine.Storage;
using TwinEngine2._0.Engine.Systems;

namespace TwinEngine2._0.Engine.Core
{
    /// <summary>
    /// Central orchestrator for the ECS.
    /// Manages entities, components, and systems.
    /// </summary>
    public class World
    {
        private readonly EntityManager _entityManager;
        private readonly ComponentStorage _componentStorage;
        private readonly List<ISystem> _systems;

        public World()
        {
            _entityManager = new EntityManager();
            _componentStorage = new ComponentStorage();
            _systems = new List<ISystem>();
        }

        #region Entity Operations

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        public Entity CreateEntity()
        {
            return _entityManager.CreateEntity();
        }

        /// <summary>
        /// Destroys an entity and removes all its components.
        /// </summary>
        public void DestroyEntity(Entity entity)
        {
            _componentStorage.RemoveAllComponents(entity);
            _entityManager.DestroyEntity(entity);
        }

        #endregion

        #region Component Operations

        /// <summary>
        /// Adds a component to an entity.
        /// </summary>
        public void AddComponent<T>(Entity entity, T component) where T : IComponent
        {
            _componentStorage.AddComponent(entity, component);
        }

        /// <summary>
        /// Removes a component from an entity.
        /// </summary>
        public void RemoveComponent<T>(Entity entity) where T : IComponent
        {
            _componentStorage.RemoveComponent<T>(entity);
        }

        /// <summary>
        /// Gets a reference to a component for an entity.
        /// </summary>
        public ref T GetComponent<T>(Entity entity) where T : IComponent
        {
            return ref _componentStorage.GetComponent<T>(entity);
        }

        /// <summary>
        /// Checks if an entity has a specific component.
        /// </summary>
        public bool HasComponent<T>(Entity entity) where T : IComponent
        {
            return _componentStorage.HasComponent<T>(entity);
        }

        #endregion

        #region System Operations

        /// <summary>
        /// Adds a system to the world.
        /// Systems are automatically sorted by ExecutionOrder.
        /// </summary>
        public void AddSystem(ISystem system)
        {
            system.Initialize(this);
            _systems.Add(system);
            _systems.Sort((a, b) => a.ExecutionOrder.CompareTo(b.ExecutionOrder));
        }

        /// <summary>
        /// Updates all systems in execution order.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            foreach (var system in _systems)
            {
                system.Update(gameTime);
            }
        }

        #endregion

        #region Queries

        /// <summary>
        /// Gets all components of a specific type for iteration.
        /// Returns a cache-friendly dense array.
        /// </summary>
        public ReadOnlySpan<T> GetComponents<T>() where T : IComponent
        {
            var pool = _componentStorage.GetPool<T>();
            return pool.GetDense();
        }

        /// <summary>
        /// Gets all entity IDs that have a specific component.
        /// Indices match the component array from GetComponents.
        /// </summary>
        public ReadOnlySpan<uint> GetEntitiesWith<T>() where T : IComponent
        {
            var pool = _componentStorage.GetPool<T>();
            return pool.GetEntities();
        }

        #endregion
    }
}
