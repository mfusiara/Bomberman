using System;
using DataStorage;
using GameLibrary.Controls;
using GameLibrary.WorldManagement;
using Microsoft.Xna.Framework;

namespace Bomberman.GameScreens
{
    public class MainMenuScreen : MenuScreen
    {
        private readonly IWorldBuilder _worldBuilder;
        private LinkLabel _newgame;
        private LinkLabel _loadGame;
        private LinkLabel _bestScores;
        private LinkLabel _settings;
        private LinkLabel _help;
        private LinkLabel _exitGame;

        public MainMenuScreen(Game1 game, 
            IGameStateManager gameStateManager, 
            IWorldBuilder worldBuilder) : base(game, gameStateManager)
        {
            _worldBuilder = worldBuilder;
        }


        protected override void LoadContent()
        {
            base.LoadContent();

            _newgame = CreateMenuItem("Nowa gra", MenuItem_Selected);
            _loadGame = CreateMenuItem("Załaduj grę", MenuItem_Selected);
            _bestScores = CreateMenuItem("Najlepsze wyniki", MenuItem_Selected);
            _settings = CreateMenuItem("Opcje", MenuItem_Selected);
            _help = CreateMenuItem("Pomoc", MenuItem_Selected);
            _exitGame = CreateMenuItem("Wyjście z gry", MenuItem_Selected);

            ControlManager.Add(_newgame);
            ControlManager.Add(_loadGame);
            ControlManager.Add(_bestScores);
            ControlManager.Add(_settings);
            ControlManager.Add(_help);
            ControlManager.Add(_exitGame);

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

            ControlManager_SetArrowPositionAlligned(_newgame, null);
        }

        private void MenuItem_Selected(object sender, EventArgs eventArgs)
        {
            if (sender == _exitGame)
                _game.Exit();
            if(sender == _bestScores)
                GameStateManager.PushState(ScreenType.BestScores);
            if(sender == _help)
                GameStateManager.PushState(ScreenType.Help);
            if (sender == _newgame)
            {
                _game.GameInfo = new GameInfo()
                {
                    UserId = _game.User.Id,
                    Stats = new GameStats
                    {
                        Level = 1,
                        LifeCount = 3,
                        Score = 0,
                    }
                };
                _worldBuilder.PlayerHitpoints = null;
                _worldBuilder.FromFile(@"Content\Worlds\World1.xml");
                _worldBuilder.GenerateBombSets();
                _worldBuilder.GenerateAidKits();
                GameStateManager.PushState(ScreenType.GamePlay);
            }
            if(sender == _loadGame)
                GameStateManager.PushState(ScreenType.LoadGame);
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