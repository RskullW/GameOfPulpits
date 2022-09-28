using System;
using System.Collections.Generic;
using System.Windows;

namespace GameOfPulpits.source
{
    public static class UserPath
    {
        public struct VideoInformation
        {
            private string _path;
            private int _duration;

            public string Path => _path;
            public int Duration => _duration;
            
            public VideoInformation(string path, int duration)
            {
                _path = path;
                _duration = duration;
            }
        }
        
        public static Dictionary<string, VideoInformation> _path = new Dictionary<string, VideoInformation>();

        public static void Initialize()
        {
            _path["Intro"] = new VideoInformation("C:/Users/mailc/Documents/University/GameOfPulpits/GameOfPulpits/GameOfPulpits/Assets/Video/Trailer.wmv", 92);
            _path["StartMenu"] = new VideoInformation("C:/Users/mailc/Documents/University/GameOfPulpits/GameOfPulpits/GameOfPulpits/Assets/eng/Menu/MainMenu.png", 0);
            _path["MainTheme"] = new VideoInformation("C:/Users/mailc/Documents/University/GameOfPulpits/GameOfPulpits/GameOfPulpits/Assets/Sounds/MainMenu.wav", 137);
        }

        public static string GetPath(string index)
        {
            if (_path.ContainsKey(index))
            {
                return _path[index].Path;
            }

            MessageBox.Show("Not index in method GetPath: " + index);
            return "None";
        }
        
        public static int GetDuration(string index)
        {
            if (_path.ContainsKey(index))
            {
                return _path[index].Duration;
            }

            MessageBox.Show("Not index in method GetPath: " + index);
            return -1;
        }
        public static VideoInformation GetVideo(string index)
        {
            if (_path.ContainsKey(index))
            {
                return _path[index];
            }

            MessageBox.Show("Not index in method GetPath: " + index);
            throw new Exception("Error. Not available video");
        }
    }
}