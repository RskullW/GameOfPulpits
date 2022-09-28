using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using GameOfPulpits.source;

namespace GameOfPulpits
{
    public static class Buttons
    {
        private static Dictionary<string, Button> _buttons = new Dictionary<string, Button>();

        public static void SetVisibility(string index, bool visible = false)
        {
            if (_buttons.ContainsKey(index))
            {
                _buttons[index].Visibility = visible?Visibility.Visible:Visibility.Hidden;
                _buttons[index].IsEnabled = visible;
            }
        }

        public static void AddButton(string index, ref Button button)
        {
            _buttons[index] = button;

            switch (index)
            {
                case "StartEng" : _buttons[index].Click += ClickedStart; break;
                case "StartRus" : _buttons[index].Click += ClickedStart; break;
                case "SettingsRus": _buttons[index].Click += MainMenu.StartSettings; break;
                case "SettingsEng": _buttons[index].Click += MainMenu.StartSettings; break;
                case "ExitEng": _buttons[index].Click += ClickedExit; break;
                case "ExitRus": _buttons[index].Click += ClickedExit; break;
                case "SetEng": _buttons[index].Click +=  MainMenu.SetLanguage; break;
                case "SetRus": _buttons[index].Click +=  MainMenu.SetLanguage; break;
                case "BackEng": _buttons[index].Click += MainMenu.HideSettings; break;
                case "BackRus": _buttons[index].Click += MainMenu.HideSettings; break;
                default: break;
            }

            _buttons[index].IsEnabled = false;
            _buttons[index].Visibility = Visibility.Hidden;
        }

        public static Button GetButton(string index)
        {
            if (_buttons.ContainsKey(index))
            {
                return _buttons[index];
            }

            throw new Exception("GetButton(string index) error");
        }
        private static void ClickedStart(object sender, RoutedEventArgs eventHandler)
        {
        }
        private static void ClickedExit(object sender, RoutedEventArgs eventHandler)
        {
            Application.Current.Shutdown();
        }
    }
}