namespace Domain.Enemies.Attacks
{
    public class OneHitpointContactAttack : IAttack
    {
        public double FirePower
        {
            get { return 1; }
        }

        public double Attack(IMortal element)
        {
            return element.ReceiveAttack(FirePower);
        }
    }
}