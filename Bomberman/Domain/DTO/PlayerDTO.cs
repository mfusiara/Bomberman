using Domain.Players;
using Domain.WorldElements;

namespace Domain.DTO
{
    public class PlayerDTO : WorldElementDTO
    {
        public KeyDTO Key { get; set; }
        public BombDTO[] Bombs { get; set; }
        public double Hitpoints { get; set; }

    }
}