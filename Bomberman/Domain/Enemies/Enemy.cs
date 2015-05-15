using System;
using System.Collections.Generic;
using Domain.Enemies.Motion;
using Domain.WorldElements;

namespace Domain.Enemies
{
    public abstract class Enemy : WorldElement, IMortal
    {
        protected IAttack _attack;
        protected IMotion _motion;
        private SpeedLevel _speed;
        public double Hitpoints { get; protected set; }

        public SpeedLevel Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                if (_speed == SpeedLevel.Normal) _motion.Speed = 1;
                else _motion.Speed = 2;
            }
        }

        public event Action<WorldElement> Dead;

        protected Enemy(IMotion motion)
        {
            _motion = motion;
            Hitpoints = 1;
        }

        protected Enemy(Coordinates coordinates, 
            IAttack attack, 
            IMotion motion, 
            int hitpoints, 
            int score) : base(coordinates, score)
        {
            _attack = attack;
            _motion = motion;
            Hitpoints = hitpoints;
        }

        public virtual double ReceiveAttack(double points = 1)
        {
            this.Hitpoints -= points;
            if (Hitpoints <= 0) OnDead();
            return points;
        }        

        public virtual double Attack(IMortal element)
        {
            return _attack.Attack(element);
        }

        public virtual void Move(IEnumerable<Direction> availableDirections)
        {
            var newCoordinates = _motion.Move(Coordinates, availableDirections);
            if(newCoordinates != null) Coordinates = newCoordinates;
        }

        protected virtual void OnDead()
        {
            var handler = Dead;
            if (handler != null) handler(this);
        }
    }
}