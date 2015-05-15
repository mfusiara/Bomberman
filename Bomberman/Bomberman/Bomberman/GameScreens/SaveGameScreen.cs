using System;
using DataStorage.Services;
using GameLibrary.Controls;
using Microsoft.Xna.Framework;

namespace Bomberman.GameScreens
{
    public class SaveGameScreen : MenuScreen
    {
        private LinkLabel _back;
        private LinkLabel _save;
        private TextBox _gameName;
        private readonly ISavingGameService _savingGameService;
        private readonly IBestScoresService _bestScoresService;

        public SaveGameScreen(Game1 game, 
            IGameStateManager gameStateManager, 
            ISavingGameService savingGameService,
            IBestScoresService bestScoresService) : base(game, gameStateManager)
        {
            _savingGameService = savingGameService;
            _bestScoresService = bestScoresService;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _gameName = new TextBox();
            _gameName.Size = _gameName.SpriteFont.MeasureString("DUMMY TEXT");
            _gameName.MaxLength = 10;
            _save = CreateMenuItem("Zapisz", MenuItem_Selected);
            _back = CreateMenuItem("Powrót", MenuItem_Selected);
            

            ControlManager.Add(_gameName);
            ControlManager.Add(_save);
            ControlManager.Add(_back);

            ControlManager.NextControl();

            ControlManager.FocusChanged += ControlManager_SetArrowPositionAlligned;

            Vector2 position = new Vector2(420, 280);
            foreach (Control c in ControlManager)
            {
                if (c is LinkLabel || c is TextBox)
                {
                    if (c.Size.X > MaxItemWidth)
                        MaxItemWidth = c.Size.X;
                    c.Position = position;
                    position.Y += c.Size.Y + 5f;
                }
            }
            _back.Position = new Vector2(_game.ScreenRectangle.Left + 100, _game.ScreenRectangle.Bottom - 100);
            ControlManager_SetArrowPositionAlligned(_gameName, null);
        }

        private void MenuItem_Selected(object sender, EventArgs eventArgs)
        {
            if (sender == _back)
                GameStateManager.PopState();
            if (sender == _save)
            {
                _savingGameService.Save(_game.World, _game.GameInfo, _gameName.Text);
                _bestScoresService.Update(_game.GameInfo.UserId, _game.GameInfo.Stats.Score);
                GameStateManager.PopState();
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