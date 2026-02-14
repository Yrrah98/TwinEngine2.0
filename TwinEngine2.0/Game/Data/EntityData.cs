using System.Collections.Generic;

namespace TwinEngine2._0.Game.Data
{
    /// <summary>
    /// Data structure representing an entity to be spawned in a level.
    /// </summary>
    public class EntityData
    {
        /// <summary>
        /// Type of entity (e.g., "Player", "NPC", "Structure").
        /// Used to determine which factory method to call.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// X-coordinate for the entity's spawn position.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y-coordinate for the entity's spawn position.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Optional properties for entity customization.
        /// Allows future extensibility without modifying the data structure.
        /// </summary>
        public Dictionary<string, object> Properties { get; set; }
    }
}
