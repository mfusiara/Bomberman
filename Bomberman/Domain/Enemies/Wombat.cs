using Domain.WorldElements;

namespace Domain.Enemies
{
    public class Wombat : Enemy
    {
        public Wombat(IMotion motion) : base(motion)
        {
        }

        public Wombat(Coordinates coordinates, IAttack attack, IMotion motion, int hitpoints, int score) : base(coordinates, attack, motion, hitpoints, score)
        {
        }

    }
}