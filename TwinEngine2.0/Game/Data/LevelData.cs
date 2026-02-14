using System.Collections.Generic;

namespace TwinEngine2._0.Game.Data
{
    /// <summary>
    /// Root data structure for a level, containing all entities in the level.
    /// Serialized from/to JSON for level loading.
    /// </summary>
    public class LevelData
    {
        /// <summary>
        /// Name or identifier for this level.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Optional description of the level.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Size of each tile in pixels (default: 32).
        /// </summary>
        public int TileSize { get; set; } = 32;

        /// <summary>
        /// 2D tile map where each integer maps to a TileType enum.
        /// Represents static, grid-based level geometry.
        /// Array format: [row][column] where row 0 is the top of the level.
        /// </summary>
        public int[][] TileMap { get; set; } = null;

        /// <summary>
        /// List of dynamic entities to spawn (player, enemies, collectibles, etc.).
        /// Use this for entities that don't align to the tile grid.
        /// </summary>
        public List<EntityData> Entities { get; set; } = new List<EntityData>();
    }
}
