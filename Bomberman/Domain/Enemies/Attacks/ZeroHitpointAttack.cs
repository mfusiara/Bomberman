namespace Domain.Enemies.Attacks
{
    public class ZeroHitpointAttack : IAttack
    {
        public double FirePower
        {
            get { return 0; }
        }

        public double Attack(IMortal element)
        {
            return element.ReceiveAttack(FirePower);
        }
    }
}