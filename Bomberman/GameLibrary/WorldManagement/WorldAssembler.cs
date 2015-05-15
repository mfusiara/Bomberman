using System;
using System.Collections.Generic;
using Domain;
using Domain.Enemies;
using Domain.WorldElements;
using GameLibrary.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = GameLibrary.Sprites.IDrawable;

namespace GameLibrary.WorldManagement
{
    public class WorldAssembler
    {
        public readonly int FieldSize = 64;
        public readonly int Yoffset = 40;
        public Texture2D PlayerTexture { get; set; }
        public Texture2D[] EnemiesTextures { get; set; }
        public Texture2D[] WallsTextures { get; set; }
        public int RealWidth { get; set; }
        public int RealHeight { get; set; }
        public Texture2D KeyTexture { get; set; }
        public Texture2D DoorTexture { get; set; }
        public Texture2D[] BombTextures { get; set; }
        public Texture2D[] AidKitTextures { get; set; }
        public Texture2D LifeTexture { get; set; }
        public SoundEffect BombSound { get; set; }
        public SoundEffect PlayerAtackReceiveSound { get; set; }
        public SoundEffect DoorUnlockSound { get; set; }
        public SoundEffect KeysSound { get; set; }

        public ExtendedWorld Build(World world)
        {
            FitCoordinates(world);

            var animations = new Dictionary<WorldElement, AnimatedSprite>();

            var anima = new Dictionary<AnimationKey, Animation>();
            var animation = new Animation(4, FieldSize, FieldSize, 0, 0);
            anima.Add(AnimationKey.Down, animation);

            var sprite = new AnimatedSprite(PlayerTexture, anima);
            animations.Add(world.Player, sprite);
            foreach (var enemy in world.Enemies)
            {
                if(enemy is Kangaroo) animations.Add(enemy, CreateSprite(EnemiesTextures[0]));
                if(enemy is Taipan) animations.Add(enemy, CreateSprite(EnemiesTextures[1]));
                if(enemy is Koala) animations.Add(enemy, CreateSprite(EnemiesTextures[2]));
                if(enemy is Wombat) animations.Add(enemy, CreateSprite(EnemiesTextures[3]));
                if(enemy is Platypus) animations.Add(enemy, CreateSprite(EnemiesTextures[4]));
            }

            foreach (var wall in world.Walls)
                animations.Add(wall, CreateSprite(WallsTextures[wall is DestructibleWall ? 0 : 1]));
            if(world.Key != null) animations.Add(world.Key, CreateSprite(KeyTexture));
            animations.Add(world.Door, CreateSprite(DoorTexture));
            foreach(var bombset in world.BombSets)
                animations.Add(bombset, CreateSprite(BombTextures[1]));
            foreach (var aidKit in world.AidKits)
                animations.Add(aidKit, CreateSprite(AidKitTextures[0]));

            var graphics = new List<IDrawable>();
            var lifes = new LifesGraphics(LifeTexture, 3) { Position = new Vector2(RealWidth - 150, 4),};
            graphics.Add(lifes);
            
            var extendedWorld = new ExtendedWorld(world, animations, graphics){Lifes = lifes,};
            extendedWorld.BombTextures = BombTextures;
            extendedWorld.BombSound = BombSound;
            extendedWorld.PlayerAtackReceiveSound = PlayerAtackReceiveSound;
            extendedWorld.DoorUnlockSound = DoorUnlockSound;
            extendedWorld.KeysSound = KeysSound;
            return extendedWorld;
        }

        private void FitCoordinates(World world)
        {
            Func<Coordinates, Coordinates> multiple = c => new Coordinates(FieldSize*c.X, FieldSize*c.Y + Yoffset);
            world.FieldSize = FieldSize;
            world.Yoffset = Yoffset;
            world.Width *= FieldSize;
            world.Height = world.Height*FieldSize + Yoffset;

            foreach (var worldElement in world.WorldElements)
                worldElement.Coordinates = multiple(worldElement.Coordinates);
        }

        private AnimatedSprite CreateSprite(Texture2D texture2D, int frames = 1)
        {
            var animations = new Dictionary<AnimationKey, Animation>();
            var animation = new Animation(frames, FieldSize, FieldSize, 0, 0);
            animations.Add(AnimationKey.Down, animation);
            var sprite = new AnimatedSprite(texture2D, animations);

            return sprite;
        }
    }
}