using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static Vector3 _positionPlayer;
    private static int _money;
    private static float _health;
    private static string _time = "0000";
    public static Vector3 PositionPlayer => _positionPlayer;
    public static int Money => _money;
    public static float Health => _health;
    public static string Time => _time;
    
    public static void LoadData(Vector3 positionPlayer, int money, float health, string time)
    {
        _positionPlayer = positionPlayer;
        _money = money;
        _health = health;
        _time = time;

        SaveGame();
    }

    private static void SaveGame()
    {
        PlayerPrefs.SetFloat("PositionX", PositionPlayer.x);
        PlayerPrefs.SetFloat("PositionY", PositionPlayer.y);
        PlayerPrefs.SetFloat("PositionZ", PositionPlayer.z);
        
        PlayerPrefs.SetInt("Money", Money);
        PlayerPrefs.SetFloat("Health", Health);
        PlayerPrefs.SetString("Time", Time);
        PlayerPrefs.SetInt("Language", (int)MenuManager.Language);
        
        Debug.Log("Game was saved");
    }

    public static bool LoadGame()
    {
        if (!PlayerPrefs.HasKey("Money"))
        {
            return false;
        }

        _money = PlayerPrefs.GetInt("Money");
        _health = PlayerPrefs.GetFloat("Health");
        _time = PlayerPrefs.GetString("Time");

        _positionPlayer.x = PlayerPrefs.GetFloat("PositionX");
        _positionPlayer.y = PlayerPrefs.GetFloat("PositionY");
        _positionPlayer.z = PlayerPrefs.GetFloat("PositionZ");
        
        return true;
    }

    public static Language GetLanguage()
    {
        if (PlayerPrefs.HasKey("Language"))
        {
            return (Language)PlayerPrefs.GetInt("Language");
        }

        return Language.Eng;
    }
    
    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey("Money");
        PlayerPrefs.DeleteKey("Health");
        PlayerPrefs.DeleteKey("Time");
        PlayerPrefs.DeleteKey("PositionX");
        PlayerPrefs.DeleteKey("PositionY");
        PlayerPrefs.DeleteKey("PositionZ");
        PlayerPrefs.DeleteKey("Language");
    }
}
