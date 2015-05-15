using System;
using System.Collections.Generic;
using Domain.WorldElements;

namespace Domain.Enemies.Motion
{
    public class UpDownMotion : IMotion
    {
        private bool _up;
        private int _oneDirectionMove = 1;
        public Direction Direction { get; private set; }

        public ushort Speed { get; set; }

        public UpDownMotion()
        {
            Speed = 2;
        }

        public Coordinates Move(Coordinates coordinates, IEnumerable<Direction> availableDirections)
        {
            if (--_oneDirectionMove == 0)
            {
                bool upExist = false;
                bool downExist = false;
                foreach (var availableDirection in availableDirections)
                {
                    if (availableDirection == Direction.Up) upExist = true;
                    if (availableDirection == Direction.Down) downExist = true;
                }

                if (!upExist && !downExist)
                {
                    _oneDirectionMove = 1;
                    return coordinates;
                }

                if (_up && upExist) _up = true;
                if (!_up && downExist) _up = false; 
                if (_up && !upExist) _up = false;
                if (!_up && !downExist) _up = true;

                _oneDirectionMove = 32;
            }

            if (_up) return new Coordinates(coordinates.X, coordinates.Y - Speed);
            return new Coordinates(coordinates.X, coordinates.Y + Speed);
        }

        public Coordinates Move(Coordinates coordinates, Coordinates direction)
        {
            throw new NotImplementedException();
        }
    }
}