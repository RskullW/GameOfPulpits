using Microsoft.DirectX.AudioVideoPlayback;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfPulpit.src
{
    public static class VideoLoader
    {
        private static AxWMPLib.AxWindowsMediaPlayer _player;
        private static bool isActive;
        private static bool isInitialize;

        public static void Initialize(ref AxWMPLib.AxWindowsMediaPlayer player)
        {

            if (isInitialize) return;

            isInitialize = true;

            _player = player;
            _player.uiMode = "none";
            _player.Ctlenabled = false;
        }
        public static void StartVideo(string url, int durationInSecond)
        {
            if (isActive)
            {
                return;
            }

            isActive = true;

            _player.URL = url;
            _player.Visible = true;

            _player.Ctlcontrols.play();

        }

        public static void StopVideo()
        {
            if (_player.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                _player.Ctlcontrols.stop();
            }

            _player.Visible = false;
            isActive = false;
        }
    }
}
