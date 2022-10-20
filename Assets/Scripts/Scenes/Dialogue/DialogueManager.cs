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
    [SerializeField] [ItemNotNull] protected List<TextMeshProUGUI> _firstDialogue;
    [SerializeField] [ItemNotNull] protected List<TextMeshProUGUI> _secondDialogue;
    [SerializeField] [ItemNotNull] protected List<TextMeshProUGUI> _thirdDialogue;
    [SerializeField] protected List<TextMeshProUGUI> _fourDialogue;
    [SerializeField] [ItemNotNull] protected List<Replics> _dialogues;
    [SerializeField] protected float _durationVisibleText;
    [SerializeField] protected List<bool> _numbersOfDialogue;
    [SerializeField] protected List<TextMeshProUGUI> _nameLeft;
    [SerializeField] protected List<TextMeshProUGUI> _nameRight;
    [SerializeField] protected List<TextMeshProUGUI> _choises;
    [SerializeField] protected TextMeshProUGUI _nameDialogue;
    
    protected short _numberDialogue;

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

    protected void HideText()
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
        
        foreach (var textMeshPro in _fourDialogue)
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
                    if (_numbersOfDialogue[^1])
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
    
    private IEnumerator StartVisibleText()
    {
        List<TextMeshProUGUI> _dialogue;
        
        switch (_numberDialogue)
        {
            case -1: _dialogue = _firstDialogue; break;
            case 0: _dialogue = _secondDialogue; break;
            case 1: _dialogue = _thirdDialogue; break;
            default: _dialogue = _fourDialogue; break;
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

            textMeshPro.font = _dialogues[_numberDialogue].FontAssetEnglish;
            _nameDialogue.text = _dialogues[_numberDialogue].NameLeftEnglish;
            _nameDialogue.font = _dialogues[_numberDialogue].FontAssetEnglish;
            
            if (MenuManager.Language == Language.Rus)
            {
                _nameDialogue.text = _dialogues[_numberDialogue].NameLeftEnglish;
                _nameDialogue.font = _dialogues[_numberDialogue].FontAssetRussian;
                
                text = _dialogues[_numberDialogue].RussianText[indexText];
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
