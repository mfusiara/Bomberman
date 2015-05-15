using Domain.WorldElements;

namespace Domain.Enemies
{
    public class Platypus : Enemy
    {
        public Platypus(IMotion motion) : base(motion)
        {
        }

        public Platypus(Coordinates coordinates, IAttack attack, IMotion motion, int hitpoints, int score) : base(coordinates, attack, motion, hitpoints, score)
        {
        }

    }
}