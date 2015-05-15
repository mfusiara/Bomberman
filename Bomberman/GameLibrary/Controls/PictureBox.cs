using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Controls
{
    public class PictureBox : Control
    {
        public Texture2D Image { get; set; }

        public Rectangle SourceRectangle { get; set; }

        public Rectangle DestinationRectangle { get; set; }

        public PictureBox(Texture2D image, Rectangle destination)
        {
            Image = image;
            DestinationRectangle = destination;
            SourceRectangle = new Rectangle(0, 0, image.Width, image.Height);
            Color = Color.White;
        }

        public PictureBox(Texture2D image, Rectangle destination, Rectangle source)
        {
            Image = image;
            DestinationRectangle = destination;
            SourceRectangle = source;
            Color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, DestinationRectangle, SourceRectangle, Color);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
        }

        public void SetPosition(Vector2 newPosition)
        {
            DestinationRectangle = new Rectangle(
                (int)newPosition.X,
                (int)newPosition.Y,
                SourceRectangle.Width,
                SourceRectangle.Height);
        }
    }
}
