using System;
using System.Timers;
using Domain.DTO;
using Domain.WorldElements;

namespace Domain.Weapons
{
    public class Bomb : WorldElement
    {
        private readonly Timer _timer;
        private Random _rand = new Random();
        public TimeSpan TimeSpan { get; protected set; }
        public ushort DestructionField { get; protected set; }

        public event EventHandler Exploded;
        public Bomb()
        {
            DestructionField = 1;

            this._timer = new Timer();
        }

        public void SetUp()
        {
            TimeSpan = TimeSpan.FromSeconds(_rand.Next(2) + 1);
            _timer.Interval = TimeSpan.TotalSeconds * 1000;
            _timer.Start();
            _timer.Elapsed += (sender, args) => OnExploded();
        }

        protected virtual void OnExploded()
        {
            var handler = Exploded;
            if (handler != null) handler(this, EventArgs.Empty);

            if(_timer != null && _timer.Enabled)
                _timer.Stop();
        }
    }
}