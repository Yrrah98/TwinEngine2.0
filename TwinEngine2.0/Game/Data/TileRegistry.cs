using System.Collections.Generic;

namespace TwinEngine2._0.Game.Data
{
    /// <summary>
    /// Maps tile types to entity template names.
    /// Provides the bridge between the tile map integers and the entity system.
    /// </summary>
    public static class TileRegistry
    {
        private static readonly Dictionary<TileType, string> _tileToTemplate = new()
        {
            [TileType.Empty] = null,  // No entity spawned for empty tiles
            [TileType.Grass] = "Tile_Grass",
            [TileType.Dirt] = "Tile_Dirt",
            [TileType.Stone] = "Tile_Stone",
            [TileType.Brick] = "Tile_Brick",
            [TileType.Water] = "Tile_Water",
            [TileType.Sky] = "Tile_Sky"
        };

        /// <summary>
        /// Gets the entity template name for a given tile type.
        /// </summary>
        /// <param name="tileType">The tile type</param>
        /// <returns>Template name, or null if no entity should be spawned</returns>
        public static string GetTemplate(TileType tileType)
        {
            return _tileToTemplate.TryGetValue(tileType, out var template) ? template : null;
        }

        /// <summary>
        /// Checks if a tile type should spawn an entity.
        /// </summary>
        /// <param name="tileType">The tile type</param>
        /// <returns>True if an entity should be spawned</returns>
        public static bool ShouldSpawnEntity(TileType tileType)
        {
            return tileType != TileType.Empty && GetTemplate(tileType) != null;
        }
    }
}
