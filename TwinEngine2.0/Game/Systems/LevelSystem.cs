using System;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwinEngine2._0.Engine.Core;
using TwinEngine2._0.Game.Data;

namespace TwinEngine2._0.Game.Systems
{
    /// <summary>
    /// System responsible for loading levels from JSON files and spawning entities.
    /// Provides a data-driven approach to level creation.
    /// </summary>
    public class LevelSystem
    {
        private readonly World _world;
        private readonly Texture2D _whiteTexture;
        private LevelData _currentLevel;

        /// <summary>
        /// Gets the currently loaded level data.
        /// </summary>
        public LevelData CurrentLevel => _currentLevel;

        public LevelSystem(World world, Texture2D whiteTexture)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
            _whiteTexture = whiteTexture ?? throw new ArgumentNullException(nameof(whiteTexture));
        }

        /// <summary>
        /// Loads a level from a JSON file and spawns all entities.
        /// </summary>
        /// <param name="levelPath">Path to the level JSON file</param>
        /// <returns>True if the level was loaded successfully, false otherwise</returns>
        public bool LoadLevel(string levelPath)
        {
            try
            {
                // Read and deserialize the JSON file
                string jsonContent = File.ReadAllText(levelPath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };

                _currentLevel = JsonSerializer.Deserialize<LevelData>(jsonContent, options);

                if (_currentLevel == null)
                {
                    Console.WriteLine($"Failed to deserialize level: {levelPath}");
                    return false;
                }

                // Spawn tiles from the tile map (if present)
                if (_currentLevel.TileMap != null && _currentLevel.TileMap.Length > 0)
                {
                    SpawnTileMap();
                }

                // Spawn dynamic entities from the entity list
                SpawnEntities();

                Console.WriteLine($"Level loaded: {_currentLevel.Name} ({_currentLevel.Entities.Count} entities)");
                return true;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Level file not found: {levelPath}");
                return false;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON parsing error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading level: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Spawns all entities from the current level data.
        /// </summary>
        private void SpawnEntities()
        {
            if (_currentLevel == null)
                return;

            foreach (var entityData in _currentLevel.Entities)
            {
                SpawnEntity(entityData);
            }
        }

        /// <summary>
        /// Spawns all tiles from the tile map.
        /// Each tile is positioned based on its row/column and the tile size.
        /// </summary>
        private void SpawnTileMap()
        {
            if (_currentLevel?.TileMap == null)
                return;

            int tileSize = _currentLevel.TileSize;
            int tilesSpawned = 0;

            for (int row = 0; row < _currentLevel.TileMap.Length; row++)
            {
                int[] rowData = _currentLevel.TileMap[row];
                if (rowData == null)
                    continue;

                for (int col = 0; col < rowData.Length; col++)
                {
                    int tileValue = rowData[col];
                    TileType tileType = (TileType)tileValue;

                    // Skip empty tiles
                    if (!TileRegistry.ShouldSpawnEntity(tileType))
                        continue;

                    // Calculate world position from tile coordinates
                    var position = new Vector2(col * tileSize, row * tileSize);

                    // Get the template name for this tile type
                    string templateName = TileRegistry.GetTemplate(tileType);
                    if (templateName != null)
                    {
                        EntityFactory.CreateFromTemplate(_world, templateName, position, _whiteTexture);
                        tilesSpawned++;
                    }
                }
            }

            Console.WriteLine($"Spawned {tilesSpawned} tiles from tile map");
        }

        /// <summary>
        /// Spawns a single entity based on its data using the template system.
        /// </summary>
        /// <param name="entityData">Data describing the entity to spawn</param>
        private void SpawnEntity(EntityData entityData)
        {
            var position = new Vector2(entityData.X, entityData.Y);
            EntityFactory.CreateFromTemplate(_world, entityData.Type, position, _whiteTexture, entityData.Properties);
        }

        /// <summary>
        /// Clears all entities from the world (useful for level transitions).
        /// Note: This is a basic implementation. Consider a more sophisticated approach
        /// if you need to preserve certain entities across level changes.
        /// </summary>
        public void ClearLevel()
        {
            _currentLevel = null;
            // Note: Clearing entities from the world would require additional World API support
            // For now, this just clears the level data reference
        }
    }
}
