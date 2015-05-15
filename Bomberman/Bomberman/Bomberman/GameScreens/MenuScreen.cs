using System;
using GameLibrary.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bomberman.GameScreens
{
    public abstract class MenuScreen : GameState
    {
        protected PictureBox _arrowImage;
        private PictureBox _backgroundImage;
        protected float MaxItemWidth = 0f;

        protected MenuScreen(Game1 game, IGameStateManager gameStateManager) : base(game, gameStateManager)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ContentManager content = _game.Content;
            _backgroundImage = new PictureBox(
                content.Load<Texture2D>(@"Backgrounds\titlescreen"),
                _game.ScreenRectangle);
            ControlManager.Add(_backgroundImage);

            Texture2D arrowTexture = content.Load<Texture2D>(@"GUI\leftarrowUp");
            _arrowImage = new PictureBox(
                arrowTexture,
                new Rectangle(
                    0,
                    0,
                    arrowTexture.Width,
                    arrowTexture.Height));
            ControlManager.Add(_arrowImage);
        }

        protected virtual void ControlManager_SetArrowPositionAlligned(object sender, EventArgs eventArgs)
        {
            Control control = sender as Control;
            Vector2 position = new Vector2(control.Position.X + MaxItemWidth + 10f, control.Position.Y);
            _arrowImage.SetPosition(position);
        }

        protected virtual void ControlManager_SetArrowPosition(object sender, EventArgs eventArgs)
        {
            Control control = sender as Control;
            var width = control.GetWidth();
            Vector2 position = new Vector2(control.Position.X + width + 10f, control.Position.Y);
            _arrowImage.SetPosition(position);
        }

        protected virtual LinkLabel CreateMenuItem(String text, EventHandler handler)
        {
            var result = new LinkLabel(text);
            result.Size = result.SpriteFont.MeasureString(result.Text);
            result.Selected += handler;

            return result;
        }
    }
}