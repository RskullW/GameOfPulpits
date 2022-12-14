using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class WolfDialogue : DialogueManager
{
    public event Action OnEndDialogue;
    public event Action OnEndLevel;
    [SerializeField] private GameObject _canvas;
    private void Awake()
    {
        _startDuration = _durationVisibleText;

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
        base.HideText();
    }
    private void Update()
    {
        if (_numberDialogue >= 0)
        {

            if (_numbersOfDialogue[_numberDialogue])
            {
                _durationVisibleText = _startDuration;

                if (Input.GetKey(KeyCode.Alpha1))
                {
                    if (_numbersOfDialogue[_numbersOfDialogue.Count-1])
                    {
                        OnEndDialogue?.Invoke();
                        _canvas.gameObject.SetActive(false);
                        gameObject.SetActive(false);
                        return;
                    }
                }
                
                if (Input.GetKey(KeyCode.Alpha2))
                {
                    if (_numbersOfDialogue[_numbersOfDialogue.Count-1])
                    {
                        OnEndDialogue?.Invoke();

                        int chance = Random.Range(0, 100);
                        
                        if (chance <= 15)
                        {
                            OnEndLevel?.Invoke();
                            _canvas.gameObject.SetActive(false);
                            gameObject.SetActive(false);
                        }
                        
                        else
                        {
                            OnEndDialogue?.Invoke();
                            _canvas.gameObject.SetActive(false);
                            gameObject.SetActive(false);
                        }
                        
                        return;
                    }

                    HideText();
                    StartCoroutine(StartVisibleText());
                }
            }
            
            else if (Input.GetKey(KeyCode.Space))
            {
                _durationVisibleText = 0f;
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

        for (int index = 0; index < _dialogues[_numberDialogue].EnglishChoise.Count; ++index)
        {
            _choises[index].gameObject.transform.parent.gameObject.SetActive(true);
            _choises[index].text = _dialogues[_numberDialogue].EnglishChoise[index];
            
            if (MenuManager.Language == Language.Rus)
            {
                _choises[index].text = _dialogues[_numberDialogue].RussianChoise[index];
            }
        }
        _numbersOfDialogue[_numberDialogue] = true;
    }
    
}
