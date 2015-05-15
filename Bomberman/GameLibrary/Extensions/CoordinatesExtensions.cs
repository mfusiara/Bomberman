using Domain;
using Domain.WorldElements;
using Microsoft.Xna.Framework;

namespace GameLibrary.Extensions
{
    public static class CoordinatesExtensions
    {
        public static Vector2 ToVector2(this Coordinates coordinates)
        {
            return new Vector2(coordinates.X, coordinates.Y);
        }
    }
}