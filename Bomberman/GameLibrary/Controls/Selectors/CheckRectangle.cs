using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Controls.Selectors
{
    public class CheckRectangle
    {
        public bool IsChecked { get; set; }
        private Texture2D _checked;
        private Texture2D _unchecked;
        private const float _size = 40;
        public Vector2 Position { get; set; }

        public float Width
        {
            get { return _size; }
        }

        public CheckRectangle(GraphicsDevice graphicsDevice)
        {
            _checked = BitmapToTexture2D(graphicsDevice, Resources._checked);
            _unchecked = BitmapToTexture2D(graphicsDevice, Resources._unchecked);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(IsChecked ? _checked : _unchecked, Position, Color.White);
        }

        public static Texture2D BitmapToTexture2D(
            GraphicsDevice GraphicsDevice,
            System.Drawing.Bitmap image)
        {
            // Buffer size is size of color array multiplied by 4 because   
            // each pixel has four color bytes  
            int bufferSize = image.Height * image.Width * 4;

            // Create new memory stream and save image to stream so   
            // we don't have to save and read file  
            System.IO.MemoryStream memoryStream =
                new System.IO.MemoryStream(bufferSize);
            image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

            // Creates a texture from IO.Stream - our memory stream  
            Texture2D texture = Texture2D.FromStream(
                GraphicsDevice, memoryStream);

            return texture;
        }
    }
}