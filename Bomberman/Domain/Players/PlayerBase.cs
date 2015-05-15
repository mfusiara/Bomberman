using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Treatment;
using Domain.Weapons;
using Domain.WorldElements;

namespace Domain.Players
{
    public abstract class PlayerBase : WorldElement, IMortal
    {
        protected IMotion _motion;
        protected IList<Bomb> _bombs;
        protected const int ProtectionTime = 3000;
        public IEnumerable<Bomb> Bombs { get { return _bombs; } } 
        public Key Key { get; protected set; }
        public bool IsProtected { get; protected set; }

        public int Speed
        {
            get { return _motion == null ? 0 : _motion.Speed; }
        }

        public double MaxHitpoints { get; protected set; }
        public double Hitpoints { get; protected set; }
        public abstract void UseAidKit(AidKit aidKit);
        public abstract Bomb PlaceBomb();
        public abstract void CollectBombs(IEnumerable<Bomb> bombs);
        public abstract void Move(Coordinates direction);

        protected PlayerBase(IMotion motion)
        {
            Hitpoints = 3;
            MaxHitpoints = 3;
            _motion = motion;
            Coordinates = new Coordinates(0, 0);
            _bombs = new List<Bomb>();
        }

        protected PlayerBase(IMotion motion, 
            Coordinates coordinates, 
            IEnumerable<Bomb> bombs, 
            double hitpoints, Key key) : base(coordinates)
        {
            _motion = motion;
            _bombs = bombs.ToList();
            Hitpoints = hitpoints;
            MaxHitpoints = 3;
            Key = key;
        }

        public virtual void CollectKey(Key key)
        {
            Key = key;
        }
        public abstract double ReceiveAttack(double points = 1);
        public abstract event Action<WorldElement> Dead;
    }
}