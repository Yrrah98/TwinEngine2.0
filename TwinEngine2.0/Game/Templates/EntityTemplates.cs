using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TwinEngine2._0.Game.Templates
{
    /// <summary>
    /// Registry of all entity templates/archetypes.
    /// Provides a centralized location for entity type definitions.
    /// </summary>
    public static class EntityTemplates
    {
        private static readonly Dictionary<string, EntityTemplate> _templates = new()
        {
            // Player - Fully interactive entity with all movement and physics
            ["Player"] = new EntityTemplate
            {
                Name = "Player",
                HasCollider = true,
                HasGravity = true,
                HasInput = true,
                HasVelocity = true,
                DefaultSize = new Vector2(32, 32),
                SpriteColor = Color.White,
                MoveSpeed = 200f,
                GravityAcceleration = 980f,
                JumpStrength = -400f
            },

            // Static NPC - Non-moving, collidable entity
            ["Static_NPC"] = new EntityTemplate
            {
                Name = "Static_NPC",
                HasCollider = true,
                HasGravity = false,
                HasInput = false,
                HasVelocity = false,
                DefaultSize = new Vector2(16, 16),
                SpriteColor = Color.Purple
            },

            // Platform - Solid ground/obstacle
            ["Platform"] = new EntityTemplate
            {
                Name = "Platform",
                HasCollider = true,
                HasGravity = false,
                HasInput = false,
                HasVelocity = false,
                DefaultSize = new Vector2(64, 16),
                SpriteColor = new Color(139, 69, 19) // Brown
            },

            // Wall - Vertical obstacle
            ["Wall"] = new EntityTemplate
            {
                Name = "Wall",
                HasCollider = true,
                HasGravity = false,
                HasInput = false,
                HasVelocity = false,
                DefaultSize = new Vector2(16, 64),
                SpriteColor = Color.Gray
            },

            // Floating Enemy - Hovering enemy without gravity
            ["Floating_Enemy"] = new EntityTemplate
            {
                Name = "Floating_Enemy",
                HasCollider = true,
                HasGravity = false,
                HasInput = false,
                HasVelocity = true,
                DefaultSize = new Vector2(16, 16),
                SpriteColor = Color.Red,
                MoveSpeed = 100f
            },

            // Ground Enemy - Walks on ground with gravity
            ["Ground_Enemy"] = new EntityTemplate
            {
                Name = "Ground_Enemy",
                HasCollider = true,
                HasGravity = true,
                HasInput = false,
                HasVelocity = true,
                DefaultSize = new Vector2(16, 16),
                SpriteColor = Color.Orange,
                MoveSpeed = 80f,
                GravityAcceleration = 980f,
                JumpStrength = -300f
            },

            // Decoration - Non-interactive visual element
            ["Decoration"] = new EntityTemplate
            {
                Name = "Decoration",
                HasCollider = false,
                HasGravity = false,
                HasInput = false,
                HasVelocity = false,
                DefaultSize = new Vector2(16, 16),
                SpriteColor = Color.LightGreen
            },

            // Collectible - Non-solid item that can be picked up
            ["Collectible"] = new EntityTemplate
            {
                Name = "Collectible",
                HasCollider = true,
                HasGravity = false,
                HasInput = false,
                HasVelocity = false,
                DefaultSize = new Vector2(24, 24),
                SpriteColor = Color.Yellow  // Brighter yellow for better visibility
            },

            // ===== TILE TEMPLATES =====
            // Used by the tile map system for grid-based level geometry

            // Grass Tile - Collidable ground surface
            ["Tile_Grass"] = new EntityTemplate
            {
                Name = "Tile_Grass",
                HasCollider = true,
                HasGravity = false,
                HasInput = false,
                HasVelocity = false,
                DefaultSize = new Vector2(32, 32),
                SpriteColor = new Color(34, 139, 34) // Forest Green
            },

            // Dirt Tile - Decorative underground (non-collidable)
            ["Tile_Dirt"] = new EntityTemplate
            {
                Name = "Tile_Dirt",
                HasCollider = false,
                HasGravity = false,
                HasInput = false,
                HasVelocity = false,
                DefaultSize = new Vector2(32, 32),
                SpriteColor = new Color(139, 69, 19) // Saddle Brown
            },

            // Stone Tile - Solid collidable block
            ["Tile_Stone"] = new EntityTemplate
            {
                Name = "Tile_Stone",
                HasCollider = true,
                HasGravity = false,
                HasInput = false,
                HasVelocity = false,
                DefaultSize = new Vector2(32, 32),
                SpriteColor = Color.Gray
            },

            // Brick Tile - Collidable wall/platform
            ["Tile_Brick"] = new EntityTemplate
            {
                Name = "Tile_Brick",
                HasCollider = true,
                HasGravity = false,
                HasInput = false,
                HasVelocity = false,
                DefaultSize = new Vector2(32, 32),
                SpriteColor = new Color(178, 34, 34) // Firebrick Red
            },

            // Water Tile - Decorative liquid (non-collidable)
            ["Tile_Water"] = new EntityTemplate
            {
                Name = "Tile_Water",
                HasCollider = false,
                HasGravity = false,
                HasInput = false,
                HasVelocity = false,
                DefaultSize = new Vector2(32, 32),
                SpriteColor = new Color(30, 144, 255) // Dodger Blue
            },

            // Sky Tile - Decorative background (non-collidable)
            ["Tile_Sky"] = new EntityTemplate
            {
                Name = "Tile_Sky",
                HasCollider = false,
                HasGravity = false,
                HasInput = false,
                HasVelocity = false,
                DefaultSize = new Vector2(32, 32),
                SpriteColor = new Color(135, 206, 235) // Sky Blue
            }
        };

        /// <summary>
        /// Gets an entity template by name.
        /// </summary>
        /// <param name="typeName">Name of the template</param>
        /// <returns>The entity template</returns>
        /// <exception cref="KeyNotFoundException">Thrown if template doesn't exist</exception>
        public static EntityTemplate Get(string typeName)
        {
            if (!_templates.TryGetValue(typeName, out var template))
            {
                throw new KeyNotFoundException($"Entity template '{typeName}' not found. Available templates: {string.Join(", ", _templates.Keys)}");
            }
            return template;
        }

        /// <summary>
        /// Checks if a template exists.
        /// </summary>
        /// <param name="typeName">Name of the template</param>
        /// <returns>True if the template exists</returns>
        public static bool Exists(string typeName)
        {
            return _templates.ContainsKey(typeName);
        }

        /// <summary>
        /// Gets all available template names.
        /// </summary>
        /// <returns>Collection of template names</returns>
        public static IEnumerable<string> GetAllTemplateNames()
        {
            return _templates.Keys;
        }
    }
}
