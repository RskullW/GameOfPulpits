using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class EnemyGuardDialog : DialogueManager
{
    public event Action OnEndDialogue;
    public event Action OnEndLevel;
    [Space] [SerializeField] private PlayerController _playerController; 
    [SerializeField] private GameObject _canvas;
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

        for (int i = 0; i < _dialogues[0].EnglishChoise.Count; ++i)
        {
            _choises[i].text = _dialogues[0].EnglishChoise[i];

            if (MenuManager.Language == Language.Rus)
            {
                _choises[i].text = _dialogues[0].RussianChoise[i];
                _choises[i].font = _dialogues[0].FontAssetRussian;

            }
        }
        
        _canvas.gameObject.SetActive(false);
    }

    public void StartDialog()
    {
        _numberDialogue = -1;
        _canvas.gameObject.SetActive(true);
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
                if (Input.GetKey(KeyCode.Alpha1))
                {

                    if (_numbersOfDialogue[1])
                    {
                        OnEndDialogue?.Invoke();
                        _canvas.gameObject.SetActive(false);
                        gameObject.SetActive(false);
                        return;
                    }

                    HideText();
                    StartCoroutine(StartVisibleText());
                }

                if (Input.GetKey(KeyCode.Alpha2) && SaveManager.IsSecondPhase >= 1)
                {
                    if (_numberDialogue == 0)
                    {
                        _numberDialogue++;
                        _numbersOfDialogue[_numberDialogue] = true;
                        
                        HideText();
                        StartCoroutine(StartVisibleText());
                    }
                }
            }
        }
    }

    private IEnumerator StartVisibleText()
    {
        List<TextMeshProUGUI> _dialogue = _firstDialogue;
        int numberTemp = 0;

        if (SaveManager.IsSecondPhase >= 1)
        {
            switch (_numberDialogue)
            {
                case -1:
                    _dialogue = _firstDialogue;
                    numberTemp = 1;
                    break;
                case 0:
                    _dialogue = _secondDialogue;
                    numberTemp = 2;
                    break;
                case 1:
                    _dialogue = _thirdDialogue;
                    numberTemp = 3;
                    break;
                default: break;
            }
        }

        else
        {
            switch (_numberDialogue)
            {
                case -1:
                    _dialogue = _firstDialogue;
                    numberTemp = 0;
                    break;
                case 0:
                    _dialogue = _secondDialogue;
                    numberTemp = 2;
                    break;
                default: break;
            }
        }


        int indexText = -1;
        _numberDialogue++;

        for (int i = 0; i < _dialogue.Count; ++i)
        {
            indexText++;

            int index = 0;
            string text = _dialogues[numberTemp].EnglishText[indexText];

            _dialogue[i].transform.parent.parent.gameObject.SetActive(true);
            _dialogue[i].gameObject.SetActive(true);


            if (MenuManager.Language == Language.Rus)
            {
                text = _dialogues[numberTemp].RussianText[indexText];
                _dialogue[i].font = _dialogues[numberTemp].FontAssetRussian;
            }

            _dialogue[i].text = "";

            while (_dialogue[i].text != text)
            {
                if ((i + 1 == _dialogue.Count - 1) && (_numberDialogue > 0) && index == 10)
                {
                    AudioManager.Instance.PlayMusic("CourtyardFight");
                }
                
                _dialogue[i].text += text[index++];
                yield return new WaitForSeconds(_durationVisibleText);
            }

        }

        for (int index = 0; index < _dialogues[numberTemp].EnglishChoise.Count; ++index)
        {
            _choises[index].gameObject.transform.parent.gameObject.SetActive(true);
            _choises[index].text = _dialogues[numberTemp].EnglishChoise[index];

            if (MenuManager.Language == Language.Rus)
            {
                _choises[index].text = _dialogues[numberTemp].RussianChoise[index];
            }
        }

        _numbersOfDialogue[_numberDialogue] = true;

    }
}
