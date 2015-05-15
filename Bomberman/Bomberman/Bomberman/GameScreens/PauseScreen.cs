using System;
using GameLibrary.Controls;
using Microsoft.Xna.Framework;

namespace Bomberman.GameScreens
{
    public class PauseScreen : MenuScreen
    {
        private LinkLabel _resumeGame;
        private LinkLabel _saveGame;
        private LinkLabel _settings;
        private LinkLabel _help;
        private LinkLabel _mainMenu;

        public PauseScreen(Game1 game, IGameStateManager gameStateManager) : base(game, gameStateManager)
        {

        }
        
        protected override void LoadContent()
        {
            base.LoadContent();

            _resumeGame = CreateMenuItem("Wznów grę", MenuItem_Selected);
            _saveGame = CreateMenuItem("Zapisz grę", MenuItem_Selected);
            _settings = CreateMenuItem("Opcje", MenuItem_Selected);
            _help = CreateMenuItem("Pomoc", MenuItem_Selected);
            _mainMenu = CreateMenuItem("Menu główne", MenuItem_Selected);

            ControlManager.Add(_resumeGame);
            ControlManager.Add(_saveGame);
            ControlManager.Add(_settings);
            ControlManager.Add(_help);
            ControlManager.Add(_mainMenu);

            ControlManager.NextControl();

            ControlManager.FocusChanged += ControlManager_SetArrowPositionAlligned;

            Vector2 position = new Vector2(420, 280);
            foreach (Control c in ControlManager)
            {
                if (c is LinkLabel)
                {
                    if (c.Size.X > MaxItemWidth)
                        MaxItemWidth = c.Size.X;

                    c.Position = position;
                    position.Y += c.Size.Y + 5f;
                }
            }

            ControlManager_SetArrowPositionAlligned(_resumeGame, null);
        }

        private void MenuItem_Selected(object sender, EventArgs eventArgs)
        {
            if (sender == _mainMenu)
            {
                GameStateManager.PopState();
                GameStateManager.PopState();
            }
            if(sender == _help)
                GameStateManager.PushState(ScreenType.Help);
            if(sender == _resumeGame)
                GameStateManager.PopState();
            if(sender == _saveGame)
                GameStateManager.PushState(ScreenType.SaveGame);
            if(sender == _settings)
                GameStateManager.PushState(ScreenType.Settings);
        }

        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, PlayerIndex.One);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _game.SpriteBatch.Begin();

            base.Draw(gameTime);
            ControlManager.Draw(_game.SpriteBatch);

            _game.SpriteBatch.End();
        }
    }
}