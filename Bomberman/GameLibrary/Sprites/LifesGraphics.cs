using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Sprites
{
    public class LifesGraphics : IDrawable
    {
        private readonly Texture2D _texture;
        private readonly IList<Graphics> _hearts;
        private Vector2 _position;
        private readonly object _heartsSync = new object();

        public int HeartCount
        {
            get { return _hearts.Count; }
            set
            {
                lock (_heartsSync)
                {
                    _hearts.Clear();
                    for (int i = 0; i < value; i++) _hearts.Add(new Graphics(_texture));
                    UpdateHeartsPostion();
                }
            }
        }

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                UpdateHeartsPostion();
            }
        }

        public LifesGraphics(Texture2D texture, int numberOfLifes = 1)
        {
            _texture = texture;
            _hearts = new List<Graphics>();
            for (int i = 0; i < numberOfLifes; i++)
                _hearts.Add(new Graphics(_texture));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            lock (_heartsSync)
            {
                foreach (var heart in _hearts)
                    heart.Draw(spriteBatch);
            }
        }

        public void RemoveHeart()
        {
            lock (_heartsSync)
            {
                if(_hearts.Any()) _hearts.RemoveAt(0);
            }
        }

        public void AddHeart()
        {
            lock (_heartsSync)
            {
                
            }
        }

        private void UpdateHeartsPostion()
        {
            const int inc = 35;
            int i = 0;
            foreach (var heart in _hearts)
                heart.Position = new Vector2(_position.X + inc * i++, _position.Y);
        }
    }
}