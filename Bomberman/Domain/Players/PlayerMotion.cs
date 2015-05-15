using System.Collections.Generic;
using Domain.WorldElements;

namespace Domain.Players
{
    public class PlayerMotion : IMotion
    {
        public Direction Direction { get; private set; }

        public ushort Speed { get; set; }

        public PlayerMotion()
        {
            Speed = 2;
        }

        public Coordinates Move(Coordinates coordinates, IEnumerable<Direction> availableDirections)
        {
            return coordinates;
        }

        public Coordinates Move(Coordinates coordinates, Coordinates direction)
        {
            return coordinates + direction * Speed;
        }
    }
}