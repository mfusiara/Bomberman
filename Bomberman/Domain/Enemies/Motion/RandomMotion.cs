using System;
using System.Collections.Generic;
using System.Linq;
using Domain.WorldElements;

namespace Domain.Enemies.Motion
{
    public class RandomMotion : IMotion
    {
        private readonly Random _random = new Random();
        private int _oneDirectionMove = 1;
        private int _oneDirectionStep = 64;
        private ushort _defaultSpeed = 1;
        private int _currentSpeed;
        public Direction Direction { get; private set; }

        public ushort Speed { get; set; }

        public RandomMotion()
        {
            Speed = _defaultSpeed;
        }

        public Coordinates Move(Coordinates coordinates, IEnumerable<Direction> availableDirections)
        {
            if (--_oneDirectionMove == 0)
            {
                _currentSpeed = Speed;
                _oneDirectionMove = _oneDirectionStep/_currentSpeed;
                
                var directions = availableDirections.ToArray();
                if (directions.Length == 0)
                {
                    _oneDirectionMove++;
                    Direction = Direction.None;
                    return coordinates;
                }
                Direction = directions[_random.Next(directions.Length)];
            }
            
            switch (Direction)
            {
                case Direction.Up:
                    return new Coordinates(coordinates.X, coordinates.Y - _currentSpeed);
                case Direction.Down:
                    return new Coordinates(coordinates.X, coordinates.Y + _currentSpeed);
                case Direction.Left:
                    return new Coordinates(coordinates.X - _currentSpeed, coordinates.Y);
                case Direction.Right:
                    return new Coordinates(coordinates.X + _currentSpeed, coordinates.Y);
            }
            return null;
        }

        public Coordinates Move(Coordinates coordinates, Coordinates direction)
        {
            throw new NotImplementedException();
        }
    }
}