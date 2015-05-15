using System.Collections.Generic;
using Domain.WorldElements;

namespace Domain.Enemies.Motion
{
    public class NoMotion : IMotion
    {
        public Direction Direction
        {
            get { return Direction.None; }
        }

        public ushort Speed
        {
            get { return 0; }
            set { }
        }

        public Coordinates Move(Coordinates coordinates, IEnumerable<Direction> availableDirections)
        {
            return coordinates;
        }

        public Coordinates Move(Coordinates coordinates, Coordinates direction)
        {
            throw new System.NotImplementedException();
        }
    }
}