using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIElementsDeath : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private string _textInRussian;
    [SerializeField] private string _textInEnglish;
    
    private UIDocument _uiDocument;

    private Label _dieLabel;
    private Button _loadButton;
    private Button _exitMenuButton;

    private VisualElement _safeArea;
    
    void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        
        InitializeUIElements();
        SetLanguage(MenuManager.Language);
        DisableScreenDeath();    
    }

    void InitializeEvents()
    {
        _loadButton.clicked += LoadGame;
        _exitMenuButton.clicked += ExitMenu;
    }

    void ExitMenu()
    {
        DisableScreenDeath();
        SceneManager.LoadScene("MainMenu");
    }
    void DisableEvents()
    {
        _loadButton.clicked -= LoadGame;
        _exitMenuButton.clicked -= ExitMenu;
    }
    private void SetLanguage(Language language)
    {
        if (!ResourcesLoad.GetIsLoadedSprites())
        {
            ResourcesLoad.LoadSprites();
        }
        _loadButton.style.backgroundImage = new StyleBackground(ResourcesLoad.GetSprite("LoadGame" + language));
        _exitMenuButton.style.backgroundImage = new StyleBackground(ResourcesLoad.GetSprite("ExitMenu" + language));
    }

    public void StartScreenDeath()
    {
        InitializeEvents();
        StartCoroutine(AppearanceScreen());
    }

    private void LoadGame()
    {
        DisableScreenDeath();

        if (SaveManager.IsWasSave)
        {
            SceneManager.LoadScene(SaveManager.GetLastNameScene());
            return;
        }
        
        SceneManager.LoadScene("DialogueKing");
        AudioManager.Instance.PlayMusic("MainMenu");
    }
    private IEnumerator AppearanceScreen()
    {
        float value = 0;
        while (_safeArea.style.opacity.value < 1f)
        {
            _safeArea.style.opacity = value;
            yield return new WaitForSeconds(0.05f);
            value += 0.05f;

            if (value > 1f)
            {
                _safeArea.style.opacity = 1f;
                break;
            }
        }
    }
    private void DisableScreenDeath()
    {
        _safeArea.style.opacity = 0f;
        DisableEvents();
    }
    void InitializeUIElements()
    {
        var root = _uiDocument.rootVisualElement;

        _safeArea = root.Q<VisualElement>("SafeArea");
        _dieLabel = root.Q<Label>("DeathLabel");
        _loadButton = root.Q<Button>("LoadGameButton"); 
        _exitMenuButton = root.Q<Button>("ExitMenuButton");
    }
}
