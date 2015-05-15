using System.Collections.Generic;
using System.Linq;
using Domain.WorldElements;

namespace Domain.Weapons
{
    public class BombSet : WorldElement
    {
        public ICollection<Bomb> Bombs { get; protected set; }

        public BombSet(Coordinates coordinates, IEnumerable<Bomb> bombs) : base(coordinates)
        {
            Bombs = bombs as ICollection<Bomb> ?? bombs.ToList();
        }

        public BombSet(Coordinates coordinates, int bombsCount) : base(coordinates)
        {
            Bombs = new List<Bomb>();
            for (int i = 0; i < bombsCount; i++)
            {
                Bombs.Add(new Bomb());
            }
        }
    }
}