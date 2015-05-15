using Domain.Treatment;

namespace Domain.DTO
{
    public class WorldDTO
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public PlayerDTO Player { get; set; }
        public EnemyDTO[] Enemies { get; set; }
        public WallDTO[] Walls { get; set; }
        public BombDTO[] Bombs {get; set; }
        public KeyDTO Key { get; set; }
        public DoorDTO Door { get; set; }
        public BombSetDTO[] BombSets { get; set; }
        public AidKitDTO[] AidKits { get; set; }
    }
}