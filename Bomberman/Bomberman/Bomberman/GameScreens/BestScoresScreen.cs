using System;
using DataStorage;
using DataStorage.Services;
using GameLibrary.Controls;
using Microsoft.Xna.Framework;

namespace Bomberman.GameScreens
{
    public class BestScoresScreen : MenuScreen
    {
        private readonly IBestScoresService _bestScoresService;
        private LinkLabel _back;
        private ListView<BestScore> _bestScoresListView; 

        public BestScoresScreen(Game1 game, 
            IGameStateManager gameStateManager,
            IBestScoresService bestScoresService) 
            : base(game, gameStateManager)
        {
            _bestScoresService = bestScoresService;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _bestScoresListView = new ListView<BestScore>(_bestScoresService.GetBestScores());
            _bestScoresListView.DisplayFormat(b => String.Format("{0,-10} {1}", b.UserName, b.Score));
            _bestScoresListView.Position = new Vector2(420, 340);
            _bestScoresListView.FontColor = Color.Aqua;

            _back = CreateMenuItem("Powrót", MenuItem_Selected);

            ControlManager.Add(_back);

            ControlManager.NextControl();

            ControlManager.FocusChanged += ControlManager_SetArrowPosition;

            _back.Position = new Vector2(_game.ScreenRectangle.Left + 100, _game.ScreenRectangle.Bottom - 100);

            ControlManager_SetArrowPosition(_back, null);
        }

        private void MenuItem_Selected(object sender, EventArgs eventArgs)
        {
            if (sender == _back)
                GameStateManager.PopState();
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
            _bestScoresListView.Draw(_game.SpriteBatch);

            _game.SpriteBatch.End();
        }
    }
}