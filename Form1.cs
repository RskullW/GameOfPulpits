using GameOfPulpit.src;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfPulpit
{
    public partial class Form1 : Form
    {
        public static int SCREEN_WIDTH;
        public static int SCREEN_HEIGHT;
        private Bitmap _bitmap;
        private Graphics _graphics;
        private src.MainMenu _mainMenu;

        public Form1()
        {
            Awake();
        }

        private void Awake()
        {
            InitializeComponent();
            Initialize();
            InitializeManagers();
        }
        private void Initialize()
        {

            SCREEN_WIDTH = 1280;
            SCREEN_HEIGHT = 720;

            _bitmap = new Bitmap(SCREEN_WIDTH, SCREEN_HEIGHT);
            _graphics = Graphics.FromImage(_bitmap);

            VideoLoader.Initialize(ref axWindowsMediaPlayer1);
            StartVideo();

            _mainMenu = new src.MainMenu(pictureBox1);
        }

        private void InitializeManagers()
        {
            SoundManager.Awake();
        }

        public void FinishDraw(Bitmap bitmap) {
            pictureBox1.Image = bitmap;
        }
        private void StartVideo() {
            axWindowsMediaPlayer1.enableContextMenu = false;
            Width = 1920;
            Height = 1080;
            Cursor.Clip = Rectangle.Empty;
            Cursor.Hide();
            VideoLoader.StartVideo(Path.GetFullPath("../../Assets/Video/Trailer.wmv"), 92);
        }
        private void StopVideo()
        {
            Width = SCREEN_WIDTH;
            Height = SCREEN_HEIGHT;
            VideoLoader.StopVideo();
            _mainMenu.StartMenu("StartMenu");
            
            Cursor.Show();
        }
        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {

            switch (axWindowsMediaPlayer1.playState)
            {
                case WMPLib.WMPPlayState.wmppsPlaying: axWindowsMediaPlayer1.fullScreen = true; break;
                case WMPLib.WMPPlayState.wmppsPaused: axWindowsMediaPlayer1.Ctlcontrols.play(); break;
                case WMPLib.WMPPlayState.wmppsStopped: StopVideo(); break;
                default: break;

            }
        }
        private void axWindowsMediaPlayer1_KeyDownEvent(object sender, AxWMPLib._WMPOCXEvents_KeyDownEvent e)
        {
            switch(e.nKeyCode)
            {
                case (short)Keys.Space: StopVideo(); break;
                case (short)Keys.Escape: StopVideo(); break;
                default: break;
            }
        }
    }
}
