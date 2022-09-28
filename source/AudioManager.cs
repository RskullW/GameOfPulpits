using System;
using System.Windows;
using System.Windows.Controls;

namespace GameOfPulpits
{
    public static class AudioManager
    {
        private static bool _isActive;
        private static MediaElement _audioPlayer;

        public static void Initialize(ref MediaElement audioElement)
        {
            _audioPlayer = audioElement;
            _audioPlayer.MediaEnded += EndedMusic;
            _isActive = false;
        }

        public static void SetAudio(string path)
        {
            _audioPlayer.Source = new Uri(path);
        }

        public static void StartAudio(bool isLoop = false)
        {
            _audioPlayer.Visibility = Visibility.Visible;
            _audioPlayer.Play();
            _isActive = true;
        }

        public static void StopAudio()
        {
            _audioPlayer.Stop();
            _audioPlayer.Visibility = Visibility.Hidden;
            _isActive = false;
        }

        private static void EndedMusic(object sender, RoutedEventArgs e)
        {
            if (_isActive)
            {
                _audioPlayer.Position = TimeSpan.Zero;
                _audioPlayer.LoadedBehavior = MediaState.Play;
            }
        }
    }
}