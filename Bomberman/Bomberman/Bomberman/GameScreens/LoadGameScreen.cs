using System;
using System.Collections.Generic;
using System.Linq;
using DataStorage;
using DataStorage.Services;
using GameLibrary.Controls;
using GameLibrary.WorldManagement;
using Microsoft.Xna.Framework;

namespace Bomberman.GameScreens
{
    public class LoadGameScreen : MenuScreen
    {
        private readonly IWorldBuilder _worldBuilder;
        private readonly ILoadingGameService _loadingGameService;
        private LinkLabel _back;
        private IList<LinkLabel> _savedGames;
        private IList<GameInfo> _worlds;

        public LoadGameScreen(Game1 game, 
            IGameStateManager gameStateManager, 
            IWorldBuilder worldBuilder, 
            ILoadingGameService loadingGameService) : base(game, gameStateManager)
        {
            _worldBuilder = worldBuilder;
            _loadingGameService = loadingGameService;
            _savedGames = new List<LinkLabel>();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _worlds = _loadingGameService.GetSavedGames(_game.User).Take(8).ToList();

            _back = CreateMenuItem("Powrót", MenuItem_Selected);
            ControlManager.Add(_back);

            foreach (var gameInfo in _worlds)
            {
                var item = CreateGameLabel(gameInfo.Name);
                _savedGames.Add(item);
                ControlManager.Add(item);
            }
            
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

            _back.Position = new Vector2(_game.ScreenRectangle.Left + 100, _game.ScreenRectangle.Bottom - 100);

            ControlManager_SetArrowPositionAlligned(_back, null);
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

        private LinkLabel CreateGameLabel(String text)
        {
            var result = new LinkLabel(text);
            result.Size = result.SpriteFont.MeasureString(result.Text);
            result.Selected += SavedGameSelected;

            return result;
        }

        private void MenuItem_Selected(object sender, EventArgs eventArgs)
        {
            if (sender == _back)
                GameStateManager.PopState();
        }

        private void SavedGameSelected(object sender, EventArgs eventArgs)
        {
            var control = sender as Control;
            if (control == null) { return;}
            var gameInfo = _worlds.FirstOrDefault(w => w.Name == control.Text);
            if (gameInfo != null)
            {
                _game.GameInfo = gameInfo;
                _worldBuilder.WorldDto = gameInfo.World;
                GameStateManager.PopState();
                GameStateManager.PushState(ScreenType.GamePlay);
            }
        }
    }
}