namespace Domain
{
    public interface IAttack
    {
        double FirePower { get; }
        double Attack(IMortal element);
    }
}