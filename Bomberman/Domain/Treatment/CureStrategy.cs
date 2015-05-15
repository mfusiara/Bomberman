using System;

namespace Domain.Treatment
{
    public class CureStrategy : ICure
    {
        public double Cure(double hp, double maxHp, double hitpoints)
        {
            if (maxHp - hp >= 1) return 1;//return Math.Min(maxHp - hp, hitpoints);
            return 0;
        }
    }
}