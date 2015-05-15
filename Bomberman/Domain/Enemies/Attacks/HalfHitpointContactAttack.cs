namespace Domain.Enemies.Attacks
{
    public class HalfHitpointContactAttack : IAttack
    {
        public double FirePower
        {
            get { return 0.5; }
        }

        public double Attack(IMortal element)
        {
            return element.ReceiveAttack(FirePower);
        }
    }
}