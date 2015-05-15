using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DataStorage;
using Domain;
using Domain.WorldElements;
using GameLibrary.Controls;
using GameLibrary.Extensions;
using GameLibrary.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = GameLibrary.Sprites.IDrawable;

namespace GameLibrary.WorldManagement
{
    public class ExtendedWorld
    {
        private readonly BombAssembler _bombAssembler;
        private Label _levelLabel;
        private Label _scoreLabel;
        public Scoring Scoring { get; set; }

        public readonly object _animationsSync = new object();
        private GameStats _stats;
        private IDictionary<WorldElement, AnimatedSprite> _animations;
        public Texture2D[] BombTextures { get; set; }
        public World World { get; private set; }
        public event Action Paused;

        public ICollection<IDrawable> Graphics { get; private set; }
        public ICollection<Control> Controls { get; private set; }
        public bool SFX { get; set; }

        public int Score
        {
            get { return Stats.Score; }
            set
            {
                Stats.Score = value;
                _scoreLabel.Text = _stats.Score.ToString();
            }
        }

        public ushort Level
        {
            get { return Stats.Level; }
            set
            {
                Stats.Level = value;
                _levelLabel.Text = String.Format("Poziom: {0}", Stats.Level);
            }
        }

        public GameStats Stats
        {
            get { return _stats; }
            set
            {
                _stats = value;
                Score = _stats.Score;
                Level = _stats.Level;
            }
        }

        public IDictionary<WorldElement, AnimatedSprite> Animations
        {
            get
            {
                lock (_animationsSync)
                {
                    return _animations;
                }   
            }
        }

        public LifesGraphics Lifes { get; set; }
        public SoundEffect BombSound { get; set; }
        public SoundEffect PlayerAtackReceiveSound { get; set; }
        public SoundEffect DoorUnlockSound { get; set; }
        public SoundEffect KeysSound { get; set; }


        protected ExtendedWorld()
        {
            _bombAssembler = new BombAssembler();
            Controls = new Collection<Control>();
            _levelLabel = new Label
            {
                Position = new Vector2(10, 1),
                Text = "Poziom",
                Size = new Vector2(10),
                Color = Color.Black,
            };
            _scoreLabel = new Label
            {
                Position = new Vector2(400, 1),
                Text = "00000",
                Size = new Vector2(10),
                Color = Color.Gray,
            };

            Controls.Add(_levelLabel);
            Controls.Add(_scoreLabel);
        }

        public ExtendedWorld(World world, 
            IDictionary<WorldElement, 
            AnimatedSprite> animations, 
            ICollection<IDrawable> graphics) : this()
        {
            AddWorld(world);
            _animations = animations;
            Graphics = graphics;
            world.BombExploding += () => SoundEffectPlayer.Instance.Play(BombSound);
            world.PlayerAttacked += () => SoundEffectPlayer.Instance.Play(PlayerAtackReceiveSound);
            world.GameWin += () => SoundEffectPlayer.Instance.Play(DoorUnlockSound);
            world.KeyCollected += () => SoundEffectPlayer.Instance.Play(KeysSound);
        }

        public void ForEachAnimiation(Action<AnimatedSprite> action)
        {
            lock (_animationsSync)
            {
                foreach (var animatedSprite in Animations)
                {
                    action(animatedSprite.Value);
                }
            }
        }

        public void PlaceBomb(Coordinates coordinates)
        {
            var bomb = World.Player.PlaceBomb();
            if (bomb == null || !World.PlaceBomb(bomb, coordinates)) { return; }
            lock (_animationsSync)
            {
                _animations.Add(bomb, _bombAssembler.Get(BombTextures));
            }
        }

        public void AddWorld(World world)
        {
            if (World != null)
            {
                World.WorldElementRemoved -= RemoveWorldElement;
            }

            World = world;
            World.WorldElementRemoved += RemoveWorldElement;
            World.GameWin += WorldOnGameWin;
            World.KeyCollected += () => Score += Scoring.KeyFounded;
        }

        private void WorldOnGameWin()
        {
            
        }

        public void UpdateSprites(GameTime gameTime)
        {
            Lifes.HeartCount = (int) World.Player.Hitpoints;
            Stats.LifeCount = (ushort) Lifes.HeartCount;

            lock (_animationsSync)
            {
                foreach (var animatedSprite in _animations)
                {
                    animatedSprite.Value.Position = animatedSprite.Key.Coordinates.ToVector2();
                    animatedSprite.Value.Update(gameTime);
                }
            }
        }

        public void MovePlayer(Coordinates coordinates)
        {
            World.MovePlayer(coordinates);
        }

        public void MoveEnemies()
        {
            World.MoveEnemies();
        }

        private void RemoveWorldElement(WorldElement worldElement)
        {
            Score += worldElement.Score;
            lock (_animationsSync)
            {
                _animations.Remove(worldElement);
            }
        }

        public void Pause()
        {
            OnPaused();
        }

        protected virtual void OnPaused()
        {
            var handler = Paused;
            if (handler != null) handler();
        }
    }

    internal class BombAssembler
    {
        public AnimatedSprite Get(Texture2D[] bombTextures)
        {
            return CreateSprite(bombTextures[0]);
        }

        private AnimatedSprite CreateSprite(Texture2D texture2D)
        {
            var animations = new Dictionary<AnimationKey, Animation>();
            var animation = new Animation(1, 64, 64, 0, 0);
            animations.Add(AnimationKey.Down, animation);
            var sprite = new AnimatedSprite(texture2D, animations);

            return sprite;
        }
    }
}