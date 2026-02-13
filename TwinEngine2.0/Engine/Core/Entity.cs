using System;

namespace TwinEngine2._0.Engine.Core
{
    /// <summary>
    /// A lightweight entity identifier in the ECS architecture.
    /// Entities are simple IDs that group components together.
    /// </summary>
    public readonly struct Entity : IEquatable<Entity>
    {
        public readonly uint Id;

        public Entity(uint id)
        {
            Id = id;
        }

        public bool Equals(Entity other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is Entity entity && Equals(entity);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"Entity({Id})";
        }
    }
}
