using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Sprites
{
    public interface IDrawable
    {
        void Draw(SpriteBatch spriteBatch);
    }
}