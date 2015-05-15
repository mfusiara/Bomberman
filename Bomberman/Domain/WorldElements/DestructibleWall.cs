using System;
using Domain.Enemies;

namespace Domain.WorldElements
{
    public class DestructibleWall : Wall, IMortal
    {
        public double Hitpoints { get; protected set; }

        public DestructibleWall(Coordinates coordinates) : base(coordinates)
        {
        }

        public double ReceiveAttack(double points = 1)
        {
            throw new NotImplementedException();
        }

        public event Action<WorldElement> Dead;

        protected virtual void OnDead()
        {
            var handler = Dead;
            if (handler != null) handler(this);
        }
    }
}