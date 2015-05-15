using Domain.WorldElements;

namespace Domain.Treatment
{
    public class AidKit : WorldElement
    {
        private readonly ICure _cureStrategy;

        public int Hitpoints { get; private set; }

        public AidKit(Coordinates coordinates, ICure cureStrategy) : base(coordinates)
        {
            Hitpoints = 3; // todo: podać jakoś sensowanie
            _cureStrategy = cureStrategy;
        }

        public double Cure(double HP, double maxHP)
        {
            return _cureStrategy.Cure(HP, maxHP, Hitpoints);
        }
    }
}