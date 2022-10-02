using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "Dialogues", menuName = "Dialogues/Replics", order = 1)]
public class Replics : ScriptableObject
{
    [SerializeField] private List<string> _englishText;
    [SerializeField] private List<string> _russianText;
    [SerializeField] private List<string> _englishChoises;
    [SerializeField] private List<string> _russianChoises;
    [SerializeField] private string _nameLeftEnglish;
    [SerializeField] private string _nameRightEnglish;
    [SerializeField] private string _nameLeftRussian;
    [SerializeField] private string _nameRightRussian;
    [SerializeField] private TMP_FontAsset _fontAssetEnglish;
    [SerializeField] private TMP_FontAsset _fontAssetRussian;

    public List<string> EnglishText => _englishText;
    public List<string> RussianText => _russianText;
    public List<string> EnglishChoise => _englishChoises;
    public List<string> RussianChoise => _russianChoises;
    public string NameLeftEnglish => _nameLeftEnglish;
    public string NameRightEnglish => _nameRightEnglish;
    public string NameLeftRussian => _nameLeftRussian;
    public string NameRightRussian => _nameRightRussian;
    public TMP_FontAsset FontAssetEnglish => _fontAssetEnglish;
    public TMP_FontAsset FontAssetRussian => _fontAssetRussian;
}
