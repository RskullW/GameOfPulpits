using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FinalMapLevelManager : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private PlayerController _player;
    [SerializeField] private List<Enemy> _enemies;

    private Label _helpMessage;
    private VisualElement _helpMessageClick;
    
    void Start()
    {
        InitializeEvents();
        InitializeEnemies();
        if (_uiDocument != null)
        {
            _helpMessage = _uiDocument.rootVisualElement.Q<Label>("HelpMessageLabel");
            _helpMessageClick = _uiDocument.rootVisualElement.Q<VisualElement>("HelpMessageClick");
            SetLanguageLabel();
        }
    }
    
    void InitializeEvents()
    {
        _player.OnMessageClick += ShowMessageClick;
    }

    void InitializeEnemies()
    {
        foreach (var enemy in _enemies)
        {
            enemy.OnCauseDamage += () => _player.TakeDamage(enemy.Damage);
        }
    }
    
    private void ShowMessageClick()
    {
        
        bool visible = _helpMessageClick.visible;
        
        _helpMessage.visible = _helpMessageClick.visible = !visible;
        _helpMessage.SetEnabled(!visible);
        _helpMessageClick.SetEnabled(!visible);
        
        Debug.Log("Show Message: " + _helpMessageClick.visible);
    }
    
    private void SetLanguageLabel()
    {
        _helpMessage.text = "Click 'E'";

        if (MenuManager.Language == Language.Rus)
        {
            _helpMessage.text = "Нажмите 'E'";
        }
    }
}
