using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Kalavarda.Primitives.WPF;
using Room.Core.Abstract;
using Room.Core.Skills;

namespace Room
{
    public class SoundPlayer: ISoundPlayer
    {
        private readonly ICollection<PlayerTuple> _mediaPlayers = new List<PlayerTuple>();
        private readonly MediaPlayer _dispatcherObject = new MediaPlayer();

        public void Play(string soundKey)
        {
            _dispatcherObject.Do(() =>
            {
                switch (soundKey)
                {
                    case FireballProcess.FireballCreated:
                        PlayFile("Fireball_01.mp3");
                        break;

                    case RoundAreaProcess.Blow:
                        PlayFile("Blow_01.mp3");
                        break;

                    default:
                        throw new NotImplementedException();
                }
            });
        }

        private void PlayFile(string fileName)
        {
            var mediaPlayer = GetMediaPlayer(fileName);
            mediaPlayer.Position = TimeSpan.Zero;
            mediaPlayer.Play();
        }

        private MediaPlayer GetMediaPlayer(string fileName)
        {
            var tuple = _mediaPlayers.FirstOrDefault(t => t.FileName == fileName && t.IsFree);
            if (tuple != null)
                return tuple.Player;

            var mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri(App.GetResourceFullFileName(fileName)));
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded1;
            _mediaPlayers.Add(new PlayerTuple(fileName, mediaPlayer));
            return mediaPlayer;
        }

        private void MediaPlayer_MediaEnded1(object sender, EventArgs e)
        {
            var mediaPlayer = (MediaPlayer)sender;
            _mediaPlayers.First(t => t.Player == mediaPlayer).IsFree = true;
        }

        private class PlayerTuple
        {
            public string FileName { get; }

            public MediaPlayer Player { get; }

            public bool IsFree { get; set; }

            public PlayerTuple(string fileName, MediaPlayer player)
            {
                FileName = fileName;
                Player = player;
            }
        }
    }
}
