using System;
using GameLibrary.Input;
using GameLibrary.Sprites;
using GameLibrary.WorldManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bomberman.GameScreens
{
    public class GamePlayScreen : GameState
    {
        private Texture2D _backgroundImage;
        private Texture2D _gameStateStripe;
        private AnimatedSprite _sprite;
        private Texture2D _spriteSheet;

        private WorldManager _worldManager;
        private readonly IWorldBuilder _worldBuilder;
        private readonly WorldManagerBuilder _worldManagerBuilder;
        private WorldAssembler _worldAssembler;

        public GamePlayScreen(Game1 game, 
            IGameStateManager manager, 
            IWorldBuilder worldBuilder,
            WorldManagerBuilder worldManagerBuilder)
            : base(game, manager)
        {
            _worldBuilder = worldBuilder;
            _worldManagerBuilder = worldManagerBuilder;
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            ContentManager content = _game.Content;
            _backgroundImage = content.Load<Texture2D>(@"Backgrounds\gameScreen");
            _gameStateStripe = content.Load<Texture2D>(@"Backgrounds\game_state_stripe");
            
            _worldAssembler = new WorldAssembler
            {
                RealWidth = _game.ScreenRectangle.Width,
                RealHeight = _game.ScreenRectangle.Height - 100,
                PlayerTexture = content.Load<Texture2D>(@"Sprites\Player\download"),
                EnemiesTextures = new[]
                {
                    content.Load<Texture2D>(@"Sprites\Enemies\kangaroo"),
                    content.Load<Texture2D>(@"Sprites\Enemies\taipan"),
                    content.Load<Texture2D>(@"Sprites\Enemies\koala"),
                    content.Load<Texture2D>(@"Sprites\Enemies\wombat"),
                    content.Load<Texture2D>(@"Sprites\Enemies\platypus"),
                },
                WallsTextures = new[]
                {
                    content.Load<Texture2D>(@"Sprites\Walls\wall"),
                    content.Load<Texture2D>(@"Sprites\Walls\wall2"),
                },
                KeyTexture = content.Load<Texture2D>(@"Sprites\Key\key"),
                DoorTexture = content.Load<Texture2D>(@"Sprites\Door\door"),
                BombTextures = new[]
                {
                    content.Load<Texture2D>(@"Sprites\Bomb\bomb"),
                    content.Load<Texture2D>(@"Sprites\Bomb\bombset"),
                },
                LifeTexture = content.Load<Texture2D>(@"heart"),
                AidKitTextures = new []
                {
                    content.Load<Texture2D>(@"Sprites\Treatment\aidkit"),
                },
                BombSound = content.Load<SoundEffect>(@"Music\bomb"),
                PlayerAtackReceiveSound = content.Load<SoundEffect>(@"Music\punch"),
                DoorUnlockSound = content.Load<SoundEffect>(@"Music\door-unlock"),
                KeysSound = content.Load<SoundEffect>(@"Music\keys"),
                
            };
            
            InitNewWorld();

            base.LoadContent();
        }

        private void WorldOnGameLost()
        {
            GameStateManager.PopState();
            GameStateManager.PushState(ScreenType.GameOver);
        }

        private void WorldOnGameWin()
        {
            _game.GameInfo.Stats.Level++;
            _game.GameInfo.Stats.Score += _game.Scoring.NextLevel;
            if (!_worldBuilder.FromFile(String.Concat(@"Content\Worlds\World", _game.GameInfo.Stats.Level, ".xml")))
            {
                WorldOnGameLost();
                return;
            }

            _worldBuilder.PlayerHitpoints = _game.GameInfo.Stats.LifeCount;
            _worldManager.Paused -= WorldManagerOnPaused;
            InitNewWorld();
        }

        private void InitNewWorld()
        {
            _game.World = _worldBuilder.Build();
            _worldManagerBuilder.GameInfo = _game.GameInfo;
            _worldManagerBuilder.Scoring = _game.Scoring;
            _worldManagerBuilder.World = _game.World;
            _worldManagerBuilder.WorldAssembler = _worldAssembler;
            _worldManager = _worldManagerBuilder.Build();
            _worldManager.Paused += WorldManagerOnPaused;
            _game.World.GameLost += WorldOnGameLost;
            _game.World.GameWin += WorldOnGameWin;
        }

        private void WorldManagerOnPaused()
        {
            GameStateManager.PushState(ScreenType.Pause);
        }

        public override void Update(GameTime gameTime)
        {
            _worldManager.Update(gameTime, InputHandler.GetPressedKeys(), InputHandler.GetSinglePressedKeys());
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _game.SpriteBatch.Begin();
            _game.SpriteBatch.Draw(_backgroundImage, _game.ScreenRectangle, Color.White);
            _game.SpriteBatch.Draw(_gameStateStripe, new Rectangle(0, 0, _game.ScreenRectangle.Width, 40), Color.White);
            _worldManager.Draw(_game.SpriteBatch, gameTime);
            base.Draw(gameTime);
            
            _game.SpriteBatch.End();
        }
    }
}