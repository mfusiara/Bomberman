namespace Domain.DTO
{
    public class WallDTO : WorldElementDTO
    {
        public bool IsDestructible { get; set; }
        public double Hitpoints { get; set; }
    }
}