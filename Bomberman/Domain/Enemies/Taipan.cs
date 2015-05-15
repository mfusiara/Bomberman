using Domain.WorldElements;

namespace Domain.Enemies
{
    public class Taipan : Enemy
    {

        public Taipan(IMotion motion) : base(motion)
        {
        }

        public Taipan(Coordinates coordinates, IAttack attack, IMotion motion, int hitpoints, int score) : base(coordinates, attack, motion, hitpoints, score)
        {
        }

    }
}