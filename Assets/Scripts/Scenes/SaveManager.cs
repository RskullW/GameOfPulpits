using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    private static bool _isHaveData;
    private static bool _isWasSave;
    private static bool _isHavePositionMap;
    private static Vector3 _positionPlayer;
    private static Vector3 _positionPlayerMap;
    private static int _money;
    private static float _health;
    private static int _levelGun;
    private static int _amountOfGarbage;
    private static int _amountOfMedicine;
    private static int _idScene;
    private static int _isSecondPhase;

    private static int _isFirstBoss;
    private static int _isSecondBoss;
    private static int _isThirdBoss;
    private static int _isFinalBoss;

    public static int IsFirstBoss => _isFirstBoss;
    public static int IsSecondBoss => _isSecondBoss;
    public static int IsThirdBoss => _isThirdBoss;
    public static int IsFinalBoss => _isFinalBoss;
    private static Dictionary<string, bool> _namesSave;
    public static Vector3 PositionPlayer => _positionPlayer;
    public static Vector3 PositionPlayerMap => _positionPlayerMap;
    public static float Health => _health;
    public static int LevelGun => _levelGun;
    public static int AmountOfGarbage => _amountOfGarbage;
    public static int AmountOfMedicine => _amountOfMedicine;
    public static bool IsHaveData => _isHaveData;
    public static bool IsWasSave => _isWasSave;
    public static bool IsHavePositionMap => _isHavePositionMap;
    public static int IsSecondPhase => _isSecondPhase;
    
    public static void LoadData(Vector3 positionPlayer, int money, float health, int levelGun, int amountOfGarbage, int amountOfMedicine)
    {
        _isHaveData = true;
        _positionPlayer = positionPlayer;
        _money = money;
        _health = health;
        _amountOfGarbage = amountOfGarbage;
        _amountOfMedicine = amountOfMedicine;
        _levelGun = levelGun;
    }

    public static void SetHaveData(bool isHaveData)
    {
        _isHaveData = isHaveData;
    }

    public static void LoadPositionMapFromSave()
    {
        if (PlayerPrefs.HasKey("PositionXMap"))
        {
            _isHavePositionMap = true;
            _positionPlayerMap.x = PlayerPrefs.GetFloat("PositionXMap");
            _positionPlayerMap.y = PlayerPrefs.GetFloat("PositionYMap");
            _positionPlayerMap.z = PlayerPrefs.GetFloat("PositionZMap");
        }
    }
    public static void SetIsHavePositionMap(bool isHavePositionMap)
    {
        _isHavePositionMap = isHavePositionMap;
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

    public static void SetAmountGarbage(int value)
    {
        _amountOfGarbage = value;
    }

    public static void SetAmountMedicine(int value)
    {
        _amountOfMedicine = value;
    }

    
    public static void SetMoney(int value)
    {
        _money = value;
    }

    public static void SetHealth(float value)
    {
        _health = value;
    }

    public static void SaveLevelGun(int levelGun)
    {
        _levelGun = levelGun;
        PlayerPrefs.SetInt("LevelGun", LevelGun);
    }

    public static void SaveMoney(int money)
    {
        _money = money;
        PlayerPrefs.SetInt("Money", _money);
    }

    public static int GetMoney()
    {
        return _money;
    }
    public static void SaveHealth(float health)
    {
        _health = health;
        PlayerPrefs.SetFloat("Health", Health);
    }
    public static void SaveGame()
    {
        _isWasSave = true;
        SaveLevel();        
        PlayerPrefs.SetFloat("PositionX", PositionPlayer.x);
        PlayerPrefs.SetFloat("PositionY", PositionPlayer.y);
        PlayerPrefs.SetFloat("PositionZ", PositionPlayer.z);
        
        PlayerPrefs.SetInt("Money", _money);
        PlayerPrefs.SetFloat("Health", Health);
        PlayerPrefs.SetInt("Language", (int)MenuManager.Language);
        PlayerPrefs.SetInt("NumberMission", (int)Missions.NumberMissions);
        
        PlayerPrefs.SetInt("LevelGun", LevelGun);
        PlayerPrefs.SetInt("AmountOfGarbage", AmountOfGarbage);
        PlayerPrefs.SetInt("AmountOfMedicine", AmountOfMedicine);
        
        PlayerPrefs.SetInt("IsFirstBoss", _isFirstBoss);
        PlayerPrefs.SetInt("IsSecondBoss", _isSecondBoss);
        PlayerPrefs.SetInt("IsThirdBoss", _isThirdBoss);
        PlayerPrefs.SetInt("IsFinalBoss", _isFinalBoss);
        
        PlayerPrefs.SetInt("IsSecondPhase", _isSecondPhase);
        
        Debug.Log("Game was saved");
    }

    public static bool LoadGame()
    {
        if (!PlayerPrefs.HasKey("Money") && !_isHaveData && !_isWasSave)
        {
            return false;
        }

        _isHaveData = true;
        _isWasSave = true;
        _money = PlayerPrefs.GetInt("Money");
        _health = PlayerPrefs.GetFloat("Health");

        _positionPlayer.x = PlayerPrefs.GetFloat("PositionX");
        _positionPlayer.y = PlayerPrefs.GetFloat("PositionY");
        _positionPlayer.z = PlayerPrefs.GetFloat("PositionZ");

        if (PlayerPrefs.HasKey("PositionXMap"))
        {
            _isHavePositionMap = true;
            _positionPlayerMap.x = PlayerPrefs.GetFloat("PositionXMap");
            _positionPlayerMap.y = PlayerPrefs.GetFloat("PositionYMap");
            _positionPlayerMap.z = PlayerPrefs.GetFloat("PositionZMap");
        }

        else
        {
            _isHavePositionMap = false;
        }
        Missions.SetNumberMission((uint)PlayerPrefs.GetInt("NumberMission"));
        
        _levelGun = PlayerPrefs.GetInt("LevelGun");
        _amountOfGarbage = PlayerPrefs.GetInt("AmountOfGarbage");
        _amountOfMedicine = PlayerPrefs.GetInt("AmountOfMedicine");
        
        _isFirstBoss = PlayerPrefs.GetInt("IsFirstBoss");
        _isSecondBoss = PlayerPrefs.GetInt("IsSecondBoss");
        _isThirdBoss = PlayerPrefs.GetInt("IsThirdBoss");
        _isFinalBoss = PlayerPrefs.GetInt("IsFinalBoss");
        
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
        _isHaveData = false;
        _isWasSave = false;
        
        _isSecondPhase = _isFirstBoss = _isSecondBoss = _isThirdBoss = _isFinalBoss = 0;

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
        PlayerPrefs.DeleteKey("SecondPhaseOpenMap");
        
        PlayerPrefs.DeleteKey("SecondPhase");
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

    public static void LoadMapPosition(Vector3 position)
    {
        _positionPlayerMap = position;
    }
    public static void SaveMap(Vector3 position, int money, float health, int levelGun, int amountOfGarbage, int amountOfMedicine)
    {
        _positionPlayerMap = position;
        
        PlayerPrefs.SetFloat("PositionXMap", PositionPlayerMap.x);
        PlayerPrefs.SetFloat("PositionYMap", PositionPlayerMap.y);
        PlayerPrefs.SetFloat("PositionZMap", PositionPlayerMap.z);
        
        _money = money;
        _health = health;
        _levelGun = levelGun;
        _amountOfGarbage = amountOfGarbage;
        _amountOfMedicine = amountOfMedicine;
        
        SaveGame();
        
        Debug.Log("Game Map was saved");

    }

    public static bool IsNotFirstStartMainMap()
    {
        return PlayerPrefs.HasKey("PositionXMap");
    }

    public static Vector3 GetMapPosition()
    {
        return _positionPlayerMap;
    }
    public static void SetInformationFirstBoss(int isFirstBoss = 1)
    {
        _isFirstBoss = isFirstBoss;
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

    public static void SetPhase(int value)
    {
        _isSecondPhase = value;
    }
    public static void SaveLevel()
    {
        PlayerPrefs.SetString("Level", SceneManager.GetActiveScene().name);
    }
}
