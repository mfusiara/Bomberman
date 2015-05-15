namespace Domain.Enemies.Attacks
{
    public class TwoHitpointsContactAttack : IAttack
    {
        public double FirePower
        {
            get { return 2; }
        }

        public double Attack(IMortal element)
        {
            return element.ReceiveAttack(FirePower);
        }
    }
}