namespace TwinEngine2._0.Game.Data
{
    /// <summary>
    /// Enumeration of tile types used in the tile map.
    /// Each value maps to a specific entity template for rendering and collision.
    /// </summary>
    public enum TileType
    {
        /// <summary>
        /// Empty space - no tile rendered.
        /// </summary>
        Empty = 0,

        /// <summary>
        /// Grass tile - collidable ground surface.
        /// </summary>
        Grass = 1,

        /// <summary>
        /// Dirt tile - decorative underground block (non-collidable).
        /// </summary>
        Dirt = 2,

        /// <summary>
        /// Stone tile - solid collidable block.
        /// </summary>
        Stone = 3,

        /// <summary>
        /// Brick tile - collidable wall/platform block.
        /// </summary>
        Brick = 4,

        /// <summary>
        /// Water tile - decorative liquid (non-collidable for now).
        /// </summary>
        Water = 5,

        /// <summary>
        /// Sky tile - decorative background (non-collidable).
        /// </summary>
        Sky = 6
    }
}
