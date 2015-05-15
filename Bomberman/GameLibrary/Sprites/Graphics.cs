using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Sprites
{
    public class Graphics : IDrawable
    {
        private readonly Texture2D _texture;
        private readonly Rectangle _rectangle = new Rectangle(0,0,30,30);
        public Vector2 Position { get; set; }

        public Graphics(Texture2D texture)
        {
            _texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                Position,
                _rectangle,
                Color.White);
        }
    }
}