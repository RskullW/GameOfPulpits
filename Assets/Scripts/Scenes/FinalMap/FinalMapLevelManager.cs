using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FinalMapLevelManager : MonoBehaviour
{
    [SerializeField] private UIElementsDeath _uiElementsDeath;
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
        _player.OnShowMessageClick += ShowMessageClick;
        _player.OnHideMessageClick += HideMessageClick;
        _player.OnDie += DeathProcess;
    }

    void InitializeEnemies()
    {
        foreach (var enemy in _enemies)
        {
            enemy.OnCauseDamage += () => _player.TakeDamage(enemy.Damage);
        }
    }

    private void DeathProcess()
    {
        StopMovement();
        _player.SetAnimation("isDeath", true); 
        AudioManager.Instance.PlaySoundDeath();
        _uiElementsDeath.StartScreenDeath();

        if (!SaveManager.IsWasSave)
        {
            SaveManager.SetHaveData(false);
            SaveManager.SetIsHavePositionMap(false);
            SaveManager.SaveStatsMission("MainMap", 0);
        }

        else
        {
            SaveManager.SetHaveData(true);
        }
    }

    private void StopMovement()
    {
        _player.SetActiveDialogue(true);
        _player.DisableAnimations();
        
        if (_enemies.Count > 0)
        {
            foreach (var enemy in _enemies)
            {
                if (enemy.gameObject.activeSelf)
                {
                    enemy.SetMovement(false);
                }
            }
        }
    }
    private void ShowMessageClick()
    {
        
        _helpMessage.visible = _helpMessageClick.visible = true;
        _helpMessage.SetEnabled(true);
        _helpMessageClick.SetEnabled(true);
    }

    protected void HideMessageClick()
    {
        _helpMessage.visible = _helpMessageClick.visible = false;
        _helpMessage.SetEnabled(false);
        _helpMessageClick.SetEnabled(false);
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
