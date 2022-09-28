using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameOfPulpits.source;

namespace GameOfPulpits
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        private static double SIZE_WIDTH;
        private static double SIZE_HEIGHT;

        private VideoLoader _introLoader;

        public MainWindow()
        {
            UserPath.Initialize();
            InitializeComponent();
            SIZE_WIDTH = Width;
            SIZE_HEIGHT = Height;

            InitializeIntro();
            InitializeMenu();
            InitializeManager();
            InitializeButtons();
            Awake();

        }

        private void Awake()
        {
            StartIntro();

        }

        private void InitializeIntro()
        {
            _introLoader = new VideoLoader(ref MediaPlayer, UserPath.GetVideo("Intro"));
            _introLoader.OnStopVideo += StopIntro;
        }

        private void InitializeManager()
        {
            AudioManager.Initialize(ref AudioPanel);
        }

        private void InitializeButtons()
        {
            Buttons.AddButton("StartEng", ref StartEngButton);
            Buttons.AddButton("StartRus", ref StartRusButton);
            Buttons.AddButton("SettingsEng", ref SettingsEngButton);
            Buttons.AddButton("SettingsRus", ref SettingsRusButton);
            Buttons.AddButton("ExitEng", ref ExitEngButton);
            Buttons.AddButton("ExitRus", ref ExitRusButton);
            Buttons.AddButton("SetEng", ref ChangeLanguageEngButton);
            Buttons.AddButton("SetRus", ref ChangeLanguageRusButton);
            Buttons.AddButton("BackEng", ref BackEngButton);
            Buttons.AddButton("BackRus", ref BackRusButton);

            
        }
        
        private void InitializeMenu()
        {
            MainMenu.Initialize(ref MainMenuImage);
        }

        private void StartIntro()
        {
            _introLoader.StartVideo();
            WindowState = WindowState.Maximized;
        }

        private void StopIntro()
        {
            MainMenu.OpenMenu();
            AudioManager.SetAudio(UserPath.GetPath("MainTheme"));
            // WindowState = WindowState.Normal;
        }
        
        private void KeyDown_Event(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    if (_introLoader.IsActive) _introLoader.StopVideo(); break;
                default: break;
            }
        }
    }
}