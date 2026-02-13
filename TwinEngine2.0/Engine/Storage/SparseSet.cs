using System;
using TwinEngine2._0.Engine.Components;
using TwinEngine2._0.Engine.Core;

namespace TwinEngine2._0.Engine.Storage
{
    /// <summary>
    /// Sparse set data structure for efficient component storage.
    /// Provides O(1) add/remove/lookup and cache-friendly iteration.
    /// </summary>
    /// <typeparam name="T">Component type</typeparam>
    public class SparseSet<T> where T : IComponent
    {
        private int[] _sparse;      // Maps entity ID → dense array index
        private T[] _dense;         // Tightly packed component data (cache-friendly)
        private uint[] _entities;   // Maps dense index → entity ID
        private int _count;         // Current number of components

        private const int InitialCapacity = 16;

        public SparseSet()
        {
            _sparse = new int[InitialCapacity];
            _dense = new T[InitialCapacity];
            _entities = new uint[InitialCapacity];
            _count = 0;

            // Initialize sparse array with -1 (invalid index)
            Array.Fill(_sparse, -1);
        }

        /// <summary>
        /// Adds a component for an entity. O(1) operation.
        /// </summary>
        public void Add(Entity entity, T component)
        {
            // Ensure sparse array can accommodate this entity ID
            if (entity.Id >= _sparse.Length)
            {
                GrowSparse((int)entity.Id + 1);
            }

            // Ensure dense arrays have capacity
            if (_count >= _dense.Length)
            {
                GrowDense();
            }

            // Add to end of dense arrays
            _sparse[entity.Id] = _count;
            _dense[_count] = component;
            _entities[_count] = entity.Id;
            _count++;
        }

        /// <summary>
        /// Removes a component from an entity using swap-and-pop. O(1) operation.
        /// </summary>
        public void Remove(Entity entity)
        {
            if (!Has(entity))
            {
                return;
            }

            int denseIndex = _sparse[entity.Id];
            int lastIndex = _count - 1;

            // Swap with last element
            if (denseIndex != lastIndex)
            {
                _dense[denseIndex] = _dense[lastIndex];
                _entities[denseIndex] = _entities[lastIndex];
                _sparse[_entities[lastIndex]] = denseIndex;
            }

            // Mark as removed
            _sparse[entity.Id] = -1;
            _count--;
        }

        /// <summary>
        /// Checks if an entity has this component. O(1) operation.
        /// </summary>
        public bool Has(Entity entity)
        {
            if (entity.Id >= _sparse.Length)
            {
                return false;
            }

            int denseIndex = _sparse[entity.Id];
            return denseIndex >= 0 && denseIndex < _count;
        }

        /// <summary>
        /// Gets a reference to the component for an entity. O(1) operation.
        /// Returns a reference for zero-copy access.
        /// </summary>
        public ref T Get(Entity entity)
        {
            if (!Has(entity))
            {
                throw new InvalidOperationException($"Entity {entity.Id} does not have component {typeof(T).Name}");
            }

            return ref _dense[_sparse[entity.Id]];
        }

        /// <summary>
        /// Gets the tightly packed component array for cache-friendly iteration.
        /// </summary>
        public ReadOnlySpan<T> GetDense()
        {
            return new ReadOnlySpan<T>(_dense, 0, _count);
        }

        /// <summary>
        /// Gets the entity IDs corresponding to the dense component array.
        /// </summary>
        public ReadOnlySpan<uint> GetEntities()
        {
            return new ReadOnlySpan<uint>(_entities, 0, _count);
        }

        /// <summary>
        /// Gets the number of components in this set.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Grows the sparse array to accommodate more entity IDs.
        /// </summary>
        private void GrowSparse(int minCapacity)
        {
            int newCapacity = Math.Max(_sparse.Length * 2, minCapacity);
            var newSparse = new int[newCapacity];
            Array.Fill(newSparse, -1);
            Array.Copy(_sparse, newSparse, _sparse.Length);
            _sparse = newSparse;
        }

        /// <summary>
        /// Grows the dense arrays to accommodate more components.
        /// </summary>
        private void GrowDense()
        {
            int newCapacity = _dense.Length * 2;
            Array.Resize(ref _dense, newCapacity);
            Array.Resize(ref _entities, newCapacity);
        }
    }
}
