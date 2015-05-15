using Domain.Enemies.Factories;

namespace GameLibrary.WorldManagement
{
    public class Scoring : IScoring
    {
        public int NextLevel { get; private set; }
        public int KeyFounded { get; private set; }
        public int KoalaKilled { get; private set; }
        public int WombatKilled { get; private set; }
        public int PlatypusKilled { get; private set; }
        public int TaipanKilled { get; private set; }
        public int KangarooKilled { get; private set; }

        public Scoring()
        {
            NextLevel = 50;
            WombatKilled = 10;
            PlatypusKilled = 10;
            TaipanKilled = 20;
            KangarooKilled = 20;
            KoalaKilled = -15;
            KeyFounded = 15;
        }
    }
}