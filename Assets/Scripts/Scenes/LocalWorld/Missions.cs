using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "Missions", menuName = "Missions/Missions1", order = 1)]
public class Missions : ScriptableObject
{
    private static uint _numberMissions = 0;
    [SerializeField] private List<string> _englishMissions;
    [SerializeField] private List<string> _russianMissions;
    [SerializeField] private string _englishQuest;
    [SerializeField] private string _russianQuest;

    [SerializeField] private TMP_FontAsset _fontAssetEnglish;
    [SerializeField] private TMP_FontAsset _fontAssetRussian;

    public List<string> EnglishMissions => _englishMissions;
    public List<string> RussianMissions => _russianMissions;
    public string EnglishQuest => _englishQuest;
    public string RussianQuest => _russianQuest;
    
    public static uint NumberMissions => _numberMissions;
    
    public TMP_FontAsset FontAssetEnglish => _fontAssetEnglish;
    public TMP_FontAsset FontAssetRussian => _fontAssetRussian;

    public static void SetNumberMission(uint numberMissions)
    {
        _numberMissions = numberMissions;
    } 
}