using Domain.WorldElements;

namespace Domain.Enemies
{
    public class Koala : Enemy
    {
        public Koala(IMotion motion) : base(motion)
        {
        }

        public Koala(Coordinates coordinates, IAttack attack, IMotion motion, int hitpoints, int score) : base(coordinates, attack, motion, hitpoints, score)
        {
            int a = 6;
            a++;
        }

    }
}