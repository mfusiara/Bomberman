using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Controls
{
    public abstract class Control
    {
        protected Vector2 position;

        public event EventHandler Selected;

        public string Name { get; set; }
        public string Text { get; set; }
        public virtual Vector2 Size { get; set; }
        public virtual Vector2 Position
        {
            get { return position; }
            set 
            { 
                position = value;
                position.Y = (int)position.Y;
            }
        }
        public object Value { get; set; }
        public bool HasFocus { get; set; }
        public bool Enabled { get; set; }
        public bool Visible { get; set; }
        public bool TabStop { get; set; }
        public SpriteFont SpriteFont { get; set; }
        public Color Color { get; set; }
        public string Type { get; set; }

        protected Control()
        {
            Color = Color.White;
            Enabled = true;
            Visible = true;
            SpriteFont = ControlManager.SpriteFont;
            Text = "";
        }

        public virtual float GetWidth()
        {
            var vector = SpriteFont.MeasureString(Text);
            return vector.X;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void HandleInput(PlayerIndex playerIndex);

        protected virtual void OnSelected(EventArgs e)
        {
            if (Selected != null)
            {
                Selected(this, e);
            }
        }
    }
}
