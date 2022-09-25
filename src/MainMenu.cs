using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfPulpit.src
{
    public enum LanguageGame
    {
        Eng = 0,
        Rus
    };

    class MainMenu
    {
        public event Action OnDraw;

        private bool _isNewGame;
        private string _pathToResources;
        private static LanguageGame _languageGame;
        private Dictionary<string, Image> _images;
        private Graphics _graphics;
        private PictureBox _pictureBox;

        public MainMenu(PictureBox pictureBox)
        {
            _isNewGame = false;
            
            _images = new Dictionary<string, Image>();
            _pictureBox = pictureBox;

            SetRussiaLanguageGame();
        }

        public void StartMenu(string index)
        {
            Image image = new Bitmap(_images[index]);
            _pictureBox.BackgroundImage = image;
        }

        public void SetLanguage()
        {
            switch (_languageGame)
            {
                case LanguageGame.Eng: SetEnglishLanguageGame(); break;
                case LanguageGame.Rus: SetRussiaLanguageGame(); break;
                default: break;
            }
        }
        public LanguageGame GetLanguageGame()
        {
            return _languageGame;
        }
        private void SetEnglishLanguageGame()
        {
            _images["MainMenu"] = Properties.Resources.MainMenuEng;
            _images["Settings"] = Properties.Resources.SettingsEng;
            _images["Pause"] = Properties.Resources.PauseEng;
            _images["StartMenu"] = Properties.Resources.StartMenuEng;
        }
        private void SetRussiaLanguageGame()
        {
            _images["MainMenu"] = Properties.Resources.MainMenuRus;
            _images["Settings"] = Properties.Resources.SettingsRus;
            _images["Pause"] = Properties.Resources.PauseRus;
            _images["StartMenu"] = Properties.Resources.StartMenuRus;
        }

     
    }
}
