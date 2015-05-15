using System;
using System.Collections.Generic;
using GameLibrary.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Controls
{
    public class ControlManager : List<Control>
    {
        int selectedControl = 0;

        public static SpriteFont SpriteFont { get; private set; }

        public event EventHandler FocusChanged;

        public ControlManager(SpriteFont spriteFont) 
            : base() 
        {
            SpriteFont = spriteFont;
        }

        public ControlManager(SpriteFont spriteFont, int capacity) 
            : base(capacity) 
        {
            SpriteFont = spriteFont;
        }

        public ControlManager(SpriteFont spriteFont, IEnumerable<Control> collection) : 
            base(collection) 
        {
            SpriteFont = spriteFont;
        }

        public void Update(GameTime gameTime, PlayerIndex playerIndex)
        {
            if (Count == 0)
                return;

            foreach (Control c in this)
            {
                if (c.Enabled)
                    c.Update(gameTime);

                if (c.HasFocus)
                    c.HandleInput(playerIndex);
            }

            if (InputHandler.ButtonPressed(Buttons.LeftThumbstickUp, playerIndex) ||
                InputHandler.ButtonPressed(Buttons.DPadUp, playerIndex) ||
                InputHandler.KeyPressed(Keys.Up))
                PreviousControl();

            if (InputHandler.ButtonPressed(Buttons.LeftThumbstickDown, playerIndex) ||
                InputHandler.ButtonPressed(Buttons.DPadDown, playerIndex) ||
                InputHandler.KeyPressed(Keys.Down))
                NextControl();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Control c in this)
            {
                if (c.Visible)
                    c.Draw(spriteBatch);
            }
        }

        public void NextControl()
        {
            if (Count == 0)
                return;

            int currentControl = selectedControl;

            this[selectedControl].HasFocus = false;

            do
            {
                selectedControl++;

                if (selectedControl == Count)
                    selectedControl = 0;

                if (this[selectedControl].TabStop && this[selectedControl].Enabled)
                {
                    if (FocusChanged != null)
                        FocusChanged(this[selectedControl], null);

                    break;
                }

            } while (currentControl != selectedControl);

            this[selectedControl].HasFocus = true;
        }

        public void PreviousControl()
        {
            if (Count == 0)
                return;

            int currentControl = selectedControl;

            this[selectedControl].HasFocus = false;

            do
            {
                selectedControl--;

                if (selectedControl < 0)
                    selectedControl = Count - 1;

                if (this[selectedControl].TabStop && this[selectedControl].Enabled)
                {
                    if (FocusChanged != null)
                        FocusChanged(this[selectedControl], null);

                    break;
                }
            } while (currentControl != selectedControl);

            this[selectedControl].HasFocus = true;
        }
    }
}
