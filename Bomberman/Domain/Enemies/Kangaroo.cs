using Domain.WorldElements;

namespace Domain.Enemies
{
    public class Kangaroo : Enemy
    {
        public Kangaroo(IMotion motion) : base(motion)
        {
        }

        public Kangaroo(Coordinates coordinates, IAttack attack, IMotion motion, int hitpoints, int score) : base(coordinates, attack, motion, hitpoints, score)
        {
        }
    }
}