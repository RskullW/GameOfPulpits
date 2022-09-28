using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GameOfPulpits.source
{
    public static class MainMenu
    {
        private static bool _isActive;
        private static bool _isInitialize;
        private static Image _imageMenu;
        private static string _languageGame;
        
        public static bool IsActive => _isActive;
        public static string LanguageGame => _languageGame;
        
        public static void Initialize(ref Image image)
        {
            if (_isInitialize)
            {
                return;
            }

            
            _isInitialize = true;
            _imageMenu = image;

            _languageGame = "Eng";

        }
        
        public static void OpenMenu()
        {
            var uri = new Uri(UserPath.GetPath("StartMenu"));
            
            _isActive = true;

            _imageMenu.Visibility = Visibility.Visible;
            _imageMenu.Source = new BitmapImage(uri);
            
            AudioManager.StartAudio();
            EnableButtons("Start" + LanguageGame);
            EnableButtons("Settings" + LanguageGame);
            EnableButtons("Exit" + LanguageGame);
        }

        private static void EnableButtons(string index)
        {
            Buttons.SetVisibility(index, true);
        }

        private static void HideButtons()
        {
            Buttons.SetVisibility("Start" + LanguageGame, false);
            Buttons.SetVisibility("Settings" + LanguageGame, false);
            Buttons.SetVisibility("Exit" + LanguageGame, false);
            Buttons.SetVisibility("Set" + LanguageGame, false);
            Buttons.SetVisibility("Back" + LanguageGame, false);
        } 

        public static void CloseMenu()
        {
            HideButtons();
            _imageMenu.Visibility = Visibility.Hidden;
            AudioManager.StopAudio();
            
        }
        
        public static void SetLanguage(object sender, RoutedEventArgs eventArgs)
        {
            HideButtons();
            
            if (_languageGame == "Eng")
            {
                _languageGame = "Rus";
            }

            else
            {
                _languageGame = "Eng";
            }
            
            EnableButtons("Set" + LanguageGame);
            EnableButtons("Back" + LanguageGame);
        }

        public static void StartSettings(object sender, RoutedEventArgs eventArgs)
        {
            HideButtons();
            
            EnableButtons("Set" + LanguageGame);
            EnableButtons("Back" + LanguageGame);
            
        }

        public static void HideSettings(object sender, RoutedEventArgs eventArgs)
        {
            HideButtons();
            
            EnableButtons("Start" + LanguageGame);
            EnableButtons("Settings" + LanguageGame);
            EnableButtons("Exit" + LanguageGame);
        }
    }
}