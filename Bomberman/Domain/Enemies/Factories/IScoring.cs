namespace Domain.Enemies.Factories
{
    public interface IScoring
    {
        int NextLevel { get; }
        int KeyFounded { get; }
        int KoalaKilled { get; }
        int WombatKilled { get; }
        int PlatypusKilled { get; }
        int TaipanKilled { get; }
        int KangarooKilled { get; }
    }
}