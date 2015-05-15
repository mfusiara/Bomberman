using Microsoft.Xna.Framework.Audio;

namespace GameLibrary
{
    public class SoundEffectPlayer
    {
        static SoundEffectPlayer()
        {
            Instance = new SoundEffectPlayer();
        }

        public static SoundEffectPlayer Instance { get; private set; }

        public bool Enabled { get; set; }

        public void Play(SoundEffect sound)
        {
            if (Enabled) sound.Play();
        }
    }
}