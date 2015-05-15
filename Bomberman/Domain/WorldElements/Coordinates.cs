using System;
using System.Collections.Generic;

namespace Domain.WorldElements
{
    public class Coordinates : ICloneable
    {
        public Coordinates()
        {
        }

        public Coordinates(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public static Coordinates operator +(Coordinates first, Coordinates other)
        {
            return new Coordinates(first.X + other.X, first.Y + other.Y);
        }

        public static Coordinates operator +(Coordinates first, int other)
        {
            return new Coordinates(first.X + other, first.Y + other);
        }

        public static Coordinates operator *(Coordinates coordinates, int n)
        {
            return new Coordinates(coordinates.X * n, coordinates.Y * n);
        }

        public static Coordinates operator /(Coordinates coordinates, int n)
        {
            return new Coordinates(coordinates.X / n, coordinates.Y / n);
        }

        public int X { get; set; }
        public int Y { get; set; }
        
        public object Clone()
        {
            return new Coordinates(X, Y);
        }

        public Direction GetDirection(Coordinates destination)
        {
            if (destination.X == X - 1 && destination.Y == Y) return Direction.Left;
            if (destination.X == X + 1 && destination.Y == Y) return Direction.Right;
            if (destination.X == X && destination.Y == Y - 1) return Direction.Up;
            if (destination.X == X && destination.Y == Y + 1) return Direction.Down;

            return Direction.None;
        }

        public IEnumerable<Coordinates> GetAdjacents()
        {
            yield return new Coordinates(X - 1, Y);
            yield return new Coordinates(X + 1, Y);
            yield return new Coordinates(X, Y - 1);
            yield return new Coordinates(X, Y + 1);
        }

        public override string ToString()
        {
            return String.Format("({0},{1})", X, Y);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Coordinates;
            return X == other.X && Y == other.Y;
        }
    }
}