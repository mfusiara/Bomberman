using System;
using System.Linq;
using GameLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Controls
{
    public class TextBox : Control
    {
        public Color SelectedColor { get; set; }
        public int MaxLength { get; set; }

        public TextBox()
        {
            TabStop = true;
            SelectedColor = Color.Red;
            HasFocus = false;
            Position = Vector2.Zero;
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
            if (!HasFocus) return;

            if (InputHandler.KeyReleased(Keys.Enter) ||
                InputHandler.ButtonReleased(Buttons.A, playerIndex))
            {
                OnSelected(null);
                return;
            }

            foreach (var key in InputHandler.GetSinglePressedKeys())
            {
                if (key == Keys.Back && Text != String.Empty)
                {
                    Text = Text.Substring(0, Text.Length - 1);
                    continue;
                }

                if(Text.Length >= MaxLength) return;
                int keyValue = (int) key;
                if ((keyValue >= 0x41 && keyValue <= 0x5A) // letters
                    || (keyValue >= 0x60 && keyValue <= 0x69))
                {
                    Text += key.ToString();
                }
                if (keyValue >= 0x30 && keyValue <= 0x39) // numbers
                {
                    var txt = key.ToString().TrimStart('D');
                    Text += txt;
                } 
                
            }
        }
    }
}