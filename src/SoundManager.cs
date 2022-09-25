using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace GameOfPulpit.src
{
    static class SoundManager
    {
        private static Dictionary<string, SoundPlayer> _soundPlayers;

        public static void Awake()
        {
            _soundPlayers = new Dictionary<string, SoundPlayer>();
            LoadSound();
        }

        private static void LoadSound()
        {
            _soundPlayers["MainMenu"] = new SoundPlayer(Properties.Resources.MainMenu);
        }
        public static void StartAudio(string index)
        {
            if (_soundPlayers.ContainsKey(index))
            {
                _soundPlayers[index].Play();
            }

            else
            {
                MessageBox.Show("SoundManager.StartAudio(" + index + ") not finded");
            }
        }

        public static void StopAudio(string index)
        {
            if (_soundPlayers.ContainsKey(index))
            {
                _soundPlayers[index].Stop();
            }

            else
            {
                MessageBox.Show("SoundManager.StopAudio(" + index + ") not finded");
            }
        
        }
    }
}
