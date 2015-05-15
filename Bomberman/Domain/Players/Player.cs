using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Domain.Treatment;
using Domain.Weapons;
using Domain.WorldElements;

namespace Domain.Players
{
    public class Player : PlayerBase
    {
        private Timer _timer;
        private Timer _bombTimer;
        private const int _bombPlacingInterval = 400;
        private bool _bombEnabled = true;

        public override event Action<WorldElement> Dead;

        public Player(IMotion motion) : base(motion)
        {
            _timer = new Timer(ProtectionTime);
            _timer.Elapsed += ProtectionEnded;
            _bombTimer = new Timer(_bombPlacingInterval);
            _bombTimer.Elapsed += BombPlacingEnable;
        }
        
        public Player(IMotion motion, 
            Coordinates coordinates, 
            IEnumerable<Bomb> bombs, 
            double hitpoints, 
            Key key) : base(motion, coordinates, bombs, hitpoints, key)
        {
            _timer = new Timer(ProtectionTime);
            _timer.Elapsed += ProtectionEnded;
            _bombTimer = new Timer(_bombPlacingInterval);
            _bombTimer.Elapsed += BombPlacingEnable;
        }

        public override void UseAidKit(AidKit aidKit)
        {
            Hitpoints += aidKit.Cure(Hitpoints, MaxHitpoints);
        }

        public override Bomb PlaceBomb()
        {
            if (!_bombEnabled || !_bombs.Any()) return null;

            _bombEnabled = false;
            var result = _bombs[0];
            _bombs.RemoveAt(0);
            _bombTimer.Start();
            return result;
        }

        public override void CollectBombs(IEnumerable<Bomb> bombs)
        {
            foreach (var bomb in bombs)
                _bombs.Add(bomb);
        }

        public override void Move(Coordinates direction)
        {
            Coordinates = _motion.Move(Coordinates, direction);
        }

        public override double ReceiveAttack(double points = 1)
        {
            if(IsProtected) return 0;
            IsProtected = true;

            Hitpoints -= points;
            if(Hitpoints <=0) OnDead(this);
            _timer.Start();
            return points;
        }

        private void ProtectionEnded(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            IsProtected = false;
            _timer.Stop();
        }
        private void BombPlacingEnable(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _bombEnabled = true;
            _bombTimer.Stop();
        }

        protected virtual void OnDead(WorldElement obj)
        {
            var handler = Dead;
            if (handler != null) handler(obj);
        }
    }
}