using System;
using Microsoft.Xna.Framework;

namespace GameLibrary.Sprites
{
    public enum AnimationKey
    {
        Down,
        Left,
        Right,
        Up
    }

    public class Animation : ICloneable
    {
        #region Field Region

        private readonly Rectangle[] frames;
        private int framesPerSecond;
        private TimeSpan frameLength;
        private TimeSpan frameTimer;
        private int currentFrame;
        private int frameWidth;
        private int frameHeight;

        #endregion

        public int FramesPerSecond
        {
            get { return framesPerSecond; }
            set
            {
                if (value < 1)
                    framesPerSecond = 1;
                else if (value > 60)
                    framesPerSecond = 60;
                else
                    framesPerSecond = value;
                frameLength = TimeSpan.FromSeconds(1/(double) framesPerSecond);
            }
        }

        public Rectangle CurrentFrameRect
        {
            get { return frames[currentFrame]; }
        }

        public int CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = (int) MathHelper.Clamp(value, 0, frames.Length - 1); }
        }

        public int FrameWidth
        {
            get { return frameWidth; }
        }

        public int FrameHeight
        {
            get { return frameHeight; }
        }

        public Animation(int frameCount, int frameWidth, int frameHeight, int xOffset, int yOffset)
        {
            frames = new Rectangle[frameCount];
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;

            for (int i = 0; i < frameCount; i++)
            {
                frames[i] = new Rectangle(
                    xOffset + (frameWidth*i),
                    yOffset,
                    frameWidth,
                    frameHeight);
            }
            FramesPerSecond = 5;
            Reset();
        }

        private Animation(Animation animation)
        {
            this.frames = animation.frames;
            FramesPerSecond = 5;
        }

        public void Update(GameTime gameTime)
        {
            frameTimer += gameTime.ElapsedGameTime;

            if (frameTimer >= frameLength)
            {
                frameTimer = TimeSpan.Zero;
                currentFrame = (currentFrame + 1)%frames.Length;
            }
        }

        public void Reset()
        {
            currentFrame = 0;
            frameTimer = TimeSpan.Zero;
        }

        public object Clone()
        {
            Animation animationClone = new Animation(this);

            animationClone.frameWidth = this.frameWidth;
            animationClone.frameHeight = this.frameHeight;
            animationClone.Reset();

            return animationClone;
        }
    }
}