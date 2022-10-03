using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] [ItemNotNull] private List<TextMeshProUGUI> _firstDialogue;
    [SerializeField] [ItemNotNull] private List<TextMeshProUGUI> _secondDialogue;
    [SerializeField] [ItemNotNull] private List<TextMeshProUGUI> _thirdDialogue;
    [SerializeField] [ItemNotNull] private List<Replics> _dialogues;
    [SerializeField] private float _durationVisibleText;
    [SerializeField] private List<bool> _numbersOfDialogue;
    [SerializeField] private List<TextMeshProUGUI> _nameLeft;
    [SerializeField] private List<TextMeshProUGUI> _nameRight;
    [SerializeField] private List<TextMeshProUGUI> _choises;
    [SerializeField] private TextMeshProUGUI _nameDialogue;
    
    private short _numberDialogue;

    private void Awake()
    {
        foreach (var name in _nameLeft)
        {
            name.text = _dialogues[0].NameLeftEnglish;

            if (MenuManager.Language == Language.Rus)
            {
                name.text = _dialogues[0].NameLeftRussian;
                name.font = _dialogues[0].FontAssetRussian;
                _nameDialogue.font = _dialogues[0].FontAssetRussian;
            }
            
            _nameDialogue.text = name.text;
            name.text += ":";
        }
        
        foreach (var name in _nameRight)
        {
            name.text = _dialogues[0].NameRightEnglish;

            if (MenuManager.Language == Language.Rus)
            {
                name.text = _dialogues[0].NameRightRussian;
                name.font = _dialogues[0].FontAssetRussian;

            }

        }
        
        for (int i = 0; i < _choises.Count; ++i)
        {
            _choises[i].text = _dialogues[0].EnglishChoise[i];

            if (MenuManager.Language == Language.Rus)
            {
                _choises[i].text = _dialogues[0].RussianChoise[i];
                _choises[i].font = _dialogues[0].FontAssetRussian;

            }
         }
        
        
    }

    void Start()
    {
        _numberDialogue = -1;
        
        HideText();
        StartCoroutine(StartVisibleText());
    }

    private void HideText()
    {
        foreach (var textMeshPro in _firstDialogue)
        {
            textMeshPro.gameObject.SetActive(false);
            textMeshPro.transform.parent.parent.gameObject.SetActive(false);
        }
        
        foreach (var textMeshPro in _secondDialogue)
        {
            textMeshPro.gameObject.SetActive(false);
            textMeshPro.transform.parent.parent.gameObject.SetActive(false);
        }

        
        foreach (var textMeshPro in _thirdDialogue)
        {
            textMeshPro.gameObject.SetActive(false);
            textMeshPro.transform.parent.parent.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (_numberDialogue >= 0)
        {

            if (_numbersOfDialogue[_numberDialogue])
            {
                if (Input.GetKey(KeyCode.Alpha1))
                {
                    if (_numbersOfDialogue[2])
                    {
                        SceneManager.LoadScene("Castle Player");
                        return;
                    }

                    HideText();
                    StartCoroutine(StartVisibleText());
                }
            }
        }
    }

    private void UpdateDialogues()
    {
        
    }
    private IEnumerator StartVisibleText()
    {
        List<TextMeshProUGUI> _dialogue;
        
        switch (_numberDialogue)
        {
            case -1: _dialogue = _firstDialogue; break;
            case 0: _dialogue = _secondDialogue; break;
            default: _dialogue = _thirdDialogue; break;
        }

        int indexText = -1;
        _numberDialogue++;

        foreach (var textMeshPro in _dialogue)
        {
            indexText++;

            int index = 0;
            string text = _dialogues[_numberDialogue].EnglishText[indexText];

            textMeshPro.transform.parent.parent.gameObject.SetActive(true);
            textMeshPro.gameObject.SetActive(true);
            
            
            if (MenuManager.Language == Language.Rus)
            {
                text = _dialogues[_numberDialogue ].RussianText[indexText];
                textMeshPro.font = _dialogues[_numberDialogue].FontAssetRussian;
            }

            textMeshPro.text = "";
            while (textMeshPro.text != text)
            {
                textMeshPro.text += text[index++];
                yield return new WaitForSeconds(_durationVisibleText);
            }
        }

        _numbersOfDialogue[_numberDialogue] = true;
    }
}
