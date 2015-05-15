using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Sprites
{
    public class AnimatedSprite
    {
        private readonly Dictionary<AnimationKey, Animation> _animations;
        private readonly Texture2D _texture;
        private Vector2 _velocity;
        private float _speed = 2.0f;
        public AnimationKey CurrentAnimation { get; set; }
        public bool IsAnimating { get; set; }

        public int Width
        {
            get { return _animations[CurrentAnimation].FrameWidth; }
        }

        public int Height
        {
            get { return _animations[CurrentAnimation].FrameHeight; }
        }

        public float Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                _speed = MathHelper.Clamp(_speed, 1.0f, 16.0f);
            }
        }

        public Vector2 Position { get; set; }
        public Vector2 Velocity
        {
            get { return _velocity; }
            set
            {
                _velocity = value;
                if (_velocity != Vector2.Zero)
                    _velocity.Normalize();
            }
        }

        public AnimatedSprite(Texture2D sprite, Dictionary<AnimationKey, Animation> animation)
        {
            _texture = sprite;
            _animations = new Dictionary<AnimationKey, Animation>();

            foreach (AnimationKey key in animation.Keys)
                _animations.Add(key, (Animation)animation[key].Clone());
            IsAnimating = true;
        }

        public void Update(GameTime gameTime)
        {
            if (IsAnimating)
                _animations[CurrentAnimation].Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                Position,
                _animations[CurrentAnimation].CurrentFrameRect,
                Color.White);
        }
    }

}