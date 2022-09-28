using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GameOfPulpits.source
{
    public class VideoLoader
    {

        public event Action OnStopVideo;
        
        private MediaElement _mediaPlayer;
        
        private string _pathVideo;
        private double _volume;
        private bool _isActive;
        private int _duration;
        private System.Windows.Threading.DispatcherTimer _timer = new System.Windows.Threading.DispatcherTimer();

        
        public bool IsActive => _isActive;

        public VideoLoader(ref MediaElement mediaPlayer, UserPath.VideoInformation video, double volume = 1f)
        {
            _isActive = false;
            
            _pathVideo = video.Path;
            _duration = video.Duration;
            _volume = volume;

            _mediaPlayer = mediaPlayer;
            
            _mediaPlayer.Source = new Uri(_pathVideo); 
        }

        public void StartVideo()
        {
            _isActive = true;
            _mediaPlayer.Stretch = Stretch.Fill;
            _mediaPlayer.Play();
            StartTimer();
        }

        public void SetVolume(double volume = 1f)
        {
            _mediaPlayer.Volume = volume;
        }

        public void StopVideo()
        {
            _isActive = false;
            _mediaPlayer.Stretch = Stretch.None;
            _mediaPlayer.Stop();
            _mediaPlayer.Visibility = Visibility.Hidden;
            OnStopVideo?.Invoke();
        }

        private void StopVideoTick(object sender,EventArgs eventArgs )
        {
            StopVideo();
        }
        
        public void PauseVideo()
        {
            _isActive = false;
            _timer.Stop();
            _mediaPlayer.Pause();
        }

        public void ResumeVideo()
        {
            _isActive = true;
            _mediaPlayer.Play();
            _timer.Start();
        }

        public void StartTimer()
        {
            _timer.Tick += new EventHandler(StopVideoTick);
            _timer.Interval = new TimeSpan(0, 0, _duration);
            _timer.Start();
        }
        
        
    }
}