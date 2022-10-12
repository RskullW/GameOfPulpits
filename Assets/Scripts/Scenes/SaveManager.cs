using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static Vector3 _positionPlayer;
    private static int _money;
    private static float _health;
    private static string _time = "0000";
    private static int _levelGun;
    private static int _amountOfGarbage;
    private static int _amountOfMedicine;
    public static Vector3 PositionPlayer => _positionPlayer;
    public static int Money => _money;
    public static float Health => _health;
    public static string Time => _time;
    public static int LevelGun => _levelGun;
    public static int AmountOfGarbage => _amountOfGarbage;
    public static int AmountOfMedicine => _amountOfMedicine;
    
    public static void LoadData(Vector3 positionPlayer, int money, float health, string time, int levelGun, int amountOfGarbage, int amountOfMedicine)
    {
        _positionPlayer = positionPlayer;
        _money = money;
        _health = health;
        _time = time;
        _amountOfGarbage = amountOfGarbage;
        _amountOfMedicine = amountOfMedicine;
        _levelGun = levelGun;
             
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
        PlayerPrefs.SetInt("NumberMission", (int)Missions.NumberMissions);
        
        PlayerPrefs.SetInt("LevelGun", LevelGun);
        PlayerPrefs.SetInt("AmountOfGarbage", AmountOfGarbage);
        PlayerPrefs.SetInt("AmountOfMedicine", AmountOfMedicine);
        
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
        Missions.SetNumberMission((uint)PlayerPrefs.GetInt("NumberMission"));
        
        _levelGun = PlayerPrefs.GetInt("LevelGun");
        _amountOfGarbage = PlayerPrefs.GetInt("AmountOfGarbage");
        _amountOfMedicine = PlayerPrefs.GetInt("AmountOfMedicine");
        
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
        PlayerPrefs.DeleteKey("NumberMission");
        PlayerPrefs.DeleteKey("AmountOfGarbage");
        PlayerPrefs.DeleteKey("AmountOfMedicine");
        PlayerPrefs.DeleteKey("LevelGun");
    }
}
