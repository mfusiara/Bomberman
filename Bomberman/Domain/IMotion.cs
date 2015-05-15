using System.Collections.Generic;
using Domain.WorldElements;

namespace Domain
{
    public interface IMotion
    {
        Direction Direction { get; }
        ushort Speed { get; set; }
        Coordinates Move(Coordinates coordinates, IEnumerable<Direction> availableDirections);
        Coordinates Move(Coordinates coordinates, Coordinates direction);
    }
}