using System;
using GameLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Controls
{
    public class LinkLabel : Control
    {
        public Color SelectedColor { get; set; }

        public LinkLabel()
        {
            SelectedColor = Color.Red;
            TabStop = true;
            HasFocus = false;
            Position = Vector2.Zero;
        }

        public LinkLabel(String text) : this()
        {
            Text = text;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (HasFocus)
                spriteBatch.DrawString(SpriteFont, Text, Position, SelectedColor);
            else
                spriteBatch.DrawString(SpriteFont, Text, Position, Color);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            if (!HasFocus)
                return;

            if (InputHandler.KeyReleased(Keys.Enter) ||
                InputHandler.ButtonReleased(Buttons.A, playerIndex))
                base.OnSelected(null);
        }
    }
}
