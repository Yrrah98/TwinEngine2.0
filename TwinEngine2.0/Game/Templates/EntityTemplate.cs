using Microsoft.Xna.Framework;

namespace TwinEngine2._0.Game.Templates
{
    /// <summary>
    /// Defines a template/archetype for an entity type.
    /// Specifies which components an entity should have and their default values.
    /// </summary>
    public class EntityTemplate
    {
        /// <summary>
        /// Name/identifier for this template.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Whether the entity has a position component.
        /// </summary>
        public bool HasPosition { get; set; } = true;

        /// <summary>
        /// Whether the entity has a size component.
        /// </summary>
        public bool HasSize { get; set; } = true;

        /// <summary>
        /// Whether the entity has a sprite component.
        /// </summary>
        public bool HasSprite { get; set; } = true;

        /// <summary>
        /// Whether the entity has a collider component.
        /// </summary>
        public bool HasCollider { get; set; } = false;

        /// <summary>
        /// Whether the entity has a gravity component.
        /// </summary>
        public bool HasGravity { get; set; } = false;

        /// <summary>
        /// Whether the entity has an input component (player-controlled).
        /// </summary>
        public bool HasInput { get; set; } = false;

        /// <summary>
        /// Whether the entity has a velocity component (can move).
        /// </summary>
        public bool HasVelocity { get; set; } = false;

        // Component Configuration

        /// <summary>
        /// Default size for the entity (width, height).
        /// </summary>
        public Vector2 DefaultSize { get; set; } = new Vector2(16, 16);

        /// <summary>
        /// Sprite tint color.
        /// </summary>
        public Color SpriteColor { get; set; } = Color.White;

        /// <summary>
        /// Movement speed in pixels per second (for entities with velocity).
        /// </summary>
        public float MoveSpeed { get; set; } = 200f;

        /// <summary>
        /// Gravity acceleration in pixels per second squared (for entities with gravity).
        /// </summary>
        public float GravityAcceleration { get; set; } = 980f;

        /// <summary>
        /// Jump strength (negative = upward) for entities with gravity.
        /// </summary>
        public float JumpStrength { get; set; } = -400f;
    }
}
