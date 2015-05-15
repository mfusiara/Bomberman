using System;
using DataStorage.Services;
using GameLibrary.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bomberman.GameScreens
{
    public class GameOverScreen : MenuScreen
    {
        private readonly IBestScoresService _bestScoresService;
        private LinkLabel _back;

        public GameOverScreen(Game1 game, IGameStateManager gameStateManager, IBestScoresService bestScoresService) 
            : base(game, gameStateManager)
        {
            _bestScoresService = bestScoresService;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _back = new LinkLabel("Koniec gry!");
            _back.SpriteFont = _game.Content.Load<SpriteFont>(@"Fonts\ControlFont50");
            _back.Size = _back.SpriteFont.MeasureString(_back.Text);
            _back.Selected += MenuItem_Selected;

            ControlManager.Add(_back);
            ControlManager.NextControl();
            _back.Position = new Vector2((_game.ScreenRectangle.Right - _back.GetWidth())/2, _game.ScreenRectangle.Bottom/2);
        }


        private void MenuItem_Selected(object sender, EventArgs eventArgs)
        {
            if (sender == _back)
            {
                _bestScoresService.Update(_game.User.Id, _game.GameInfo.Stats.Score);
                GameStateManager.PopState();
                GameStateManager.PushState(ScreenType.BestScores);
            }
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