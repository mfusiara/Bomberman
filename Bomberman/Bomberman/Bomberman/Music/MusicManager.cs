using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Media;

namespace Bomberman.Music
{
    public interface IMusicManager
    {
        IList<Song> Songs { get; set; }
        void Play();
        void Stop();
    }

    public class MusicManager : IMusicManager
    {
        private bool _playing;
        public IList<Song> Songs { get; set; }

        public void Play()
        {
            if(!_playing) MediaPlayer.Play(Songs.First());
            _playing = true;
        }

        public void Stop()
        {
            _playing = false;
            MediaPlayer.Stop();
        }
    }
}