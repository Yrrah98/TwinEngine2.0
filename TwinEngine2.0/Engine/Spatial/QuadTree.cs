using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TwinEngine2._0.Engine.Core;

namespace TwinEngine2._0.Engine.Spatial
{
    /// <summary>
    /// QuadTree spatial partitioning data structure for efficient collision detection.
    /// Divides 2D space into quadrants to minimize collision checks.
    /// </summary>
    public class QuadTree
    {
        private readonly int _maxEntities;
        private readonly int _maxDepth;
        private readonly int _depth;
        private readonly Rectangle _bounds;

        private List<QuadTreeEntry> _entities;
        private QuadTree[] _children;
        private bool _isDivided;

        /// <summary>
        /// Entry storing an entity and its bounds in the QuadTree.
        /// </summary>
        public struct QuadTreeEntry
        {
            public Entity Entity;
            public Rectangle Bounds;

            public QuadTreeEntry(Entity entity, Rectangle bounds)
            {
                Entity = entity;
                Bounds = bounds;
            }
        }

        public QuadTree(Rectangle bounds, int maxEntities = 4, int maxDepth = 5, int depth = 0)
        {
            _bounds = bounds;
            _maxEntities = maxEntities;
            _maxDepth = maxDepth;
            _depth = depth;
            _entities = new List<QuadTreeEntry>();
            _children = null;
            _isDivided = false;
        }

        /// <summary>
        /// Inserts an entity with its bounds into the QuadTree.
        /// </summary>
        public void Insert(Entity entity, Rectangle bounds)
        {
            // If bounds don't intersect this node, ignore
            if (!_bounds.Intersects(bounds))
                return;

            // If we have space and haven't subdivided, add here
            if (_entities.Count < _maxEntities || _depth >= _maxDepth)
            {
                _entities.Add(new QuadTreeEntry(entity, bounds));
                return;
            }

            // Need to subdivide
            if (!_isDivided)
            {
                Subdivide();
            }

            // Try to insert into children
            // If entity fits entirely in a child, use that child
            bool insertedInChild = false;
            foreach (var child in _children)
            {
                if (child._bounds.Contains(bounds))
                {
                    child.Insert(entity, bounds);
                    insertedInChild = true;
                    break;
                }
            }

            // If doesn't fit entirely, insert into child with largest intersection
            if (!insertedInChild)
            {
                int largestIntersection = 0;
                QuadTree bestChild = null;

                foreach (var child in _children)
                {
                    if (child._bounds.Intersects(bounds))
                    {
                        Rectangle intersection = Rectangle.Intersect(child._bounds, bounds);
                        int intersectionArea = intersection.Width * intersection.Height;

                        if (intersectionArea > largestIntersection)
                        {
                            largestIntersection = intersectionArea;
                            bestChild = child;
                        }
                    }
                }

                // Insert into child with best overlap, or keep at this level if no intersection
                if (bestChild != null)
                {
                    bestChild.Insert(entity, bounds);
                }
                else
                {
                    _entities.Add(new QuadTreeEntry(entity, bounds));
                }
            }
        }

        /// <summary>
        /// Queries the QuadTree for entities within or intersecting a rectangular area.
        /// </summary>
        public List<QuadTreeEntry> Query(Rectangle area)
        {
            var result = new List<QuadTreeEntry>();

            // If area doesn't intersect this node, return empty
            if (!_bounds.Intersects(area))
                return result;

            // Add entities from this node that intersect the area
            foreach (var entry in _entities)
            {
                if (entry.Bounds.Intersects(area))
                {
                    result.Add(entry);
                }
            }

            // Query children if subdivided
            if (_isDivided)
            {
                foreach (var child in _children)
                {
                    result.AddRange(child.Query(area));
                }
            }

            return result;
        }

        /// <summary>
        /// Clears all entities from the QuadTree.
        /// </summary>
        public void Clear()
        {
            _entities.Clear();
            _isDivided = false;
            _children = null;
        }

        /// <summary>
        /// Gets all node bounds for visualization (debug drawing).
        /// </summary>
        public List<Rectangle> GetAllNodeBounds()
        {
            var bounds = new List<Rectangle> { _bounds };

            if (_isDivided)
            {
                foreach (var child in _children)
                {
                    bounds.AddRange(child.GetAllNodeBounds());
                }
            }

            return bounds;
        }

        /// <summary>
        /// Subdivides this node into 4 quadrants.
        /// </summary>
        private void Subdivide()
        {
            int halfWidth = _bounds.Width / 2;
            int halfHeight = _bounds.Height / 2;
            int x = _bounds.X;
            int y = _bounds.Y;

            _children = new QuadTree[4];

            // Top-left
            _children[0] = new QuadTree(
                new Rectangle(x, y, halfWidth, halfHeight),
                _maxEntities, _maxDepth, _depth + 1
            );

            // Top-right
            _children[1] = new QuadTree(
                new Rectangle(x + halfWidth, y, halfWidth, halfHeight),
                _maxEntities, _maxDepth, _depth + 1
            );

            // Bottom-left
            _children[2] = new QuadTree(
                new Rectangle(x, y + halfHeight, halfWidth, halfHeight),
                _maxEntities, _maxDepth, _depth + 1
            );

            // Bottom-right
            _children[3] = new QuadTree(
                new Rectangle(x + halfWidth, y + halfHeight, halfWidth, halfHeight),
                _maxEntities, _maxDepth, _depth + 1
            );

            _isDivided = true;

            // Redistribute existing entities to children using Insert
            // This will use the same logic as Insert (best overlap child)
            var entitiesToRedistribute = new List<QuadTreeEntry>(_entities);
            _entities.Clear();

            foreach (var entry in entitiesToRedistribute)
            {
                // Use Insert to place entity (will use best-fit logic)
                Insert(entry.Entity, entry.Bounds);
            }
        }
    }
}
