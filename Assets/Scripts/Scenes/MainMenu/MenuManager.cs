using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;
using Cursor = UnityEngine.Cursor;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer _intro;
    [SerializeField] private UIDocument _uiDocument;
    private Dictionary<string, Button> _buttons;
    private static bool _isNewGame = true;
    private static bool _isOpenGame;
    private static Language _language;

    public static Language Language => _language;
    public static bool IsNewGame => _isNewGame;
    void Awake()
    {
        _isNewGame = true;
        _buttons = new Dictionary<string, Button>();
        _uiDocument.enabled = false;
        _intro.loopPointReached += OnVideoEnd;

        if (_isOpenGame)
        {
            _intro.Stop();
            StartMenu();
        }
    }

    void Start()
    {
        if (!Cursor.visible)
        {
            Cursor.visible = true;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Escape))
        {
            OnVideoEnd(_intro);
        }
    }

    private void OnVideoEnd(VideoPlayer causedVideoPlayer)
    {
        if (!_isOpenGame)
        {
            _intro.Stop();
            _intro.loopPointReached -= OnVideoEnd;
            _intro.enabled = false;
            StartMenu();

            _isOpenGame = true;
        }
    }
    private void StartMenu()
    {
        Debug.Log("StartMenu()");
        _uiDocument.enabled = true; ;
        AudioManager.Instance.PlayMusic("MainMenu");
        InitializeButtons();
        
        if (SaveManager.LoadGame())
        {
            _isNewGame = false;
            _language = SaveManager.GetLanguage();
            LocalSetLanguage();
        }
    }
    private void InitializeButtons()
    {
        var root = _uiDocument.rootVisualElement;

        _buttons["Start"] = root.Q<Button>("StartButton");
        _buttons["Settings"] = root.Q<Button>("SettingsButton");
        _buttons["Exit"] = root.Q<Button>("ExitButton");
        _buttons["Set"] = root.Q<Button>("SetButton");
        _buttons["Back"] = root.Q<Button>("BackButton");
        _buttons["Delete"] = root.Q<Button>("DeleteButton");

        _buttons["Start"].clicked += StartGame;
        _buttons["Set"].clicked += SetLanguage;
        _buttons["Back"].clicked += StartBackButton;
        _buttons["Delete"].clicked += DeleteSave;
        _buttons["Exit"].clicked += ExitGame;
        _buttons["Settings"].clicked += StartSettings;
        _buttons["Set"].SetEnabled(false);
        _buttons["Delete"].SetEnabled(false);
        _buttons["Back"].SetEnabled(false);
        _buttons["Set"].visible = false;
        _buttons["Delete"].visible = false;
        _buttons["Back"].visible = false;
    }

    private void StartBackButton()
    {
        SetVisibleButtons();
    }
    private void StartSettings()
    {
        SetVisibleButtons();
    }
    private void DeleteSave()
    {
        Debug.Log("Saves was deleted!");
        _isNewGame = true;
        SaveManager.DeleteSave();
        
        _buttons["Start"].style.backgroundImage = new StyleBackground(ResourcesLoad.GetSprite("Start" + _language));
    }
    private void SetVisibleButtons()
    {
        bool visible = _buttons["Start"].visible;
        
        _buttons["Start"].visible = !visible;
        _buttons["Start"].SetEnabled(!visible);
        
        _buttons["Settings"].visible = !visible;
        _buttons["Settings"].SetEnabled(!visible);

        _buttons["Exit"].visible = !visible;
        _buttons["Exit"].SetEnabled(!visible);

        _buttons["Set"].visible = visible;
        _buttons["Set"].SetEnabled(visible);

        _buttons["Delete"].visible = visible;
        _buttons["Delete"].SetEnabled(visible);

        _buttons["Back"].visible = visible;
        _buttons["Back"].SetEnabled(visible);
    }
    private void StartGame()
    {
        if (!_isNewGame)
        {
            SceneManager.LoadScene(SaveManager.GetLastNameScene());
            return;
        }
        
        SceneManager.LoadScene("DialogueKing");
    }
    private void ExitGame()
    {
        Application.Quit();
    }
    private void SetLanguage()
    {
        if (_language == Language.Eng)
        {
            _language = Language.Rus;
        }

        else
        {
            _language = Language.Eng;
        }

        LocalSetLanguage();
    }

    private void LocalSetLanguage()
    {
        _buttons["Start"].style.backgroundImage = new StyleBackground(ResourcesLoad.GetSprite("Start" + _language));

        if (!_isNewGame)
        {
            _buttons["Start"].style.backgroundImage = new StyleBackground(ResourcesLoad.GetSprite("Continue" + _language));
        }
        _buttons["Settings"].style.backgroundImage = new StyleBackground(ResourcesLoad.GetSprite("Settings" + _language));
        _buttons["Exit"].style.backgroundImage = new StyleBackground(ResourcesLoad.GetSprite("Exit" + _language));
        _buttons["Set"].style.backgroundImage = new StyleBackground(ResourcesLoad.GetSprite("Set" + _language));
        _buttons["Delete"].style.backgroundImage = new StyleBackground(ResourcesLoad.GetSprite("Delete" + _language));
        _buttons["Back"].style.backgroundImage = new StyleBackground(ResourcesLoad.GetSprite("Back" + _language));
    }
    public static void SetLanguage(Language language)
    {
        _language = language;
    }
    
}
