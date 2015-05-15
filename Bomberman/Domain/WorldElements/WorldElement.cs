using System;
using System.Threading;

namespace Domain.WorldElements
{
    public class WorldElement : IWorldElement
    {
        public int Score { get; protected set; }
        public String Name { get; set; }
        public Coordinates Coordinates { get; set; }

        public WorldElement()
        {
            Coordinates = new Coordinates(0, 0);
            Score = 0;
        }

        public WorldElement(Coordinates coordinates)
        {
            Coordinates = coordinates;
            Score = 0;
        }

        public WorldElement(Coordinates coordinates, int score)
        {
            Score = score;
            Coordinates = coordinates;
        }
    }
}