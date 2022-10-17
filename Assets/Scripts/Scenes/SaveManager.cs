using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    private static Vector3 _positionPlayer;
    private static int _money;
    private static float _health;
    private static string _time = "0000";
    private static int _levelGun;
    private static int _amountOfGarbage;
    private static int _amountOfMedicine;
    private static int _idScene;

    private static int _isFirstBoss;
    private static int _isSecondBoss;
    private static int _isThirdBoss;
    private static int _isFinalBoss;
    private static Dictionary<string, bool> _namesSave;
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

    public static void SaveAmountOfMedicine(int amountOfMedicine)
    {
        _amountOfMedicine = amountOfMedicine;
        PlayerPrefs.SetInt("AmountOfMedicine", AmountOfMedicine);
        
    }
    public static void SaveAmountOfGarbage(int amountOfGarbage)
    {
        _amountOfGarbage = amountOfGarbage;
        PlayerPrefs.SetInt("AmountOfGarbage", AmountOfGarbage);
    }

    public static void SaveLevelGun(int levelGun)
    {
        _levelGun = levelGun;
        PlayerPrefs.SetInt("LevelGun", LevelGun);
    }

    public static void SaveMoney(int money)
    {
        _money = money;
        PlayerPrefs.SetInt("Money", Money);
    }

    public static void SaveHealth(float health)
    {
        _health = health;
        PlayerPrefs.SetFloat("Health", Health);
    }

    public static void SaveTime(string time)
    {
        _time = time;
        PlayerPrefs.SetString("Time", Time);
    }
    private static void SaveGame()
    {
        // MAIN MAP
        
        //
        SaveLevel();        
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
        // LOCAL MAP
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
        
        // MAIN MAP
        PlayerPrefs.DeleteKey("PositionXMap");
        PlayerPrefs.DeleteKey("PositionYMap");
        PlayerPrefs.DeleteKey("PositionZMap");
        
        // SCENE
        PlayerPrefs.DeleteKey("Level");
        
        // BOSS
        PlayerPrefs.DeleteKey("IsFinalBoss");
        PlayerPrefs.DeleteKey("IsFirstBoss");
        PlayerPrefs.DeleteKey("IsSecondBoss");
        PlayerPrefs.DeleteKey("IsThirdBoss");
        PlayerPrefs.DeleteKey("MainMap");
    }

    public static int GetStatsMissions(string name)
    {
        if (PlayerPrefs.HasKey(name))
        {
            if (_namesSave == null)
            {
                _namesSave = new Dictionary<string, bool>();
            }
            
            if (!_namesSave.ContainsKey(name))
            {
                _namesSave[name] = true;
            }
            
            return PlayerPrefs.GetInt(name);
        }
        
        return 0;
    }

    public static void SaveStatsMission(string name, int isFirstOpen = 1)
    {
        Debug.Log("SaveStatsMission(string " + name + ", int " + isFirstOpen + ")");

        if (_namesSave == null)
        {
            _namesSave = new Dictionary<string, bool>();
        }
        if (!_namesSave.ContainsKey(name))
        {
            _namesSave[name] = true;
        }
        
        PlayerPrefs.SetInt(name, isFirstOpen);
    }

    public static string GetLastNameScene()
    {
        string nameScene = PlayerPrefs.GetString("Level");
        return nameScene;
    }

    public static void SaveMapPosition(Vector3 position)
    {
        PlayerPrefs.SetFloat("PositionXMap", position.x);
        PlayerPrefs.SetFloat("PositionYMap", position.y);
        PlayerPrefs.SetFloat("PositionZMap", position.z);
    }

    public static bool IsNotFirstStartMainMap()
    {
        return PlayerPrefs.HasKey("PositionXMap");
    }

    public static Vector3 GetMapPosition()
    {
        Vector3 vector3 = new Vector3(PlayerPrefs.GetFloat("PositionXMap"), PlayerPrefs.GetFloat("PositionYMap"),
            PlayerPrefs.GetFloat("PositionZMap"));

        return vector3;
    }

    public static void SetInformationFirstBoss()
    {
        PlayerPrefs.SetInt("IsFirstBoss", 1);
    }
    public static bool GetInformationFirstBoss()
    {
        if (!PlayerPrefs.HasKey("IsFirstBoss"))
        {
            return false;
        }

        return true;
    }
    
    public static void SetInformationSecondBoss()
    {
        PlayerPrefs.SetInt("IsSecondBoss", 1);
    }
    public static bool GetInformationSecondBoss()
    {
        if (!PlayerPrefs.HasKey("IsSecondBoss"))
        {
            return false;
        }

        return true;
    }
    
    public static void SetInformationThirdBoss()
    {
        PlayerPrefs.SetInt("IsThirdBoss", 1);
    }
    public static bool GetInformationThirdBoss()
    {
        if (!PlayerPrefs.HasKey("IsThirdBoss"))
        {
            return false;
        }

        return true;
    }

    public static void SetInformationFinalBoss()
    {
        PlayerPrefs.SetInt("IsFinalBoss", 1);
    }
    public static bool GetInformationFinalBoss()
    {
        if (!PlayerPrefs.HasKey("IsFinalBoss"))
        {
            return false;
        }

        return true;
    }

    public static void SaveLevel()
    {
        PlayerPrefs.SetString("Level", SceneManager.GetActiveScene().name);
    }

}
