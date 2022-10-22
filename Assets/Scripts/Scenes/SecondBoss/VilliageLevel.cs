using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class VilliageLevel : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private VilliageDialog _villiageDialogue;
    [SerializeField] private PlayerController _player;

    public static bool IS_SECOND_PHASE_LEVEL;
    private Label _helpMessage;
    private VisualElement _helpMessageClick;
    
    void Start()
    {
        InitializeEvents();  

        _player.SetActiveDialogue(true);
        
        if (_uiDocument != null)
        {
            _helpMessage = _uiDocument.rootVisualElement.Q<Label>("HelpMessageLabel");
            _helpMessageClick = _uiDocument.rootVisualElement.Q<VisualElement>("HelpMessageClick");
            SetLanguageLabel();
            
        }

        if (IS_SECOND_PHASE_LEVEL)
        {
            StartSecondPhase();
        }
    }
    
    void InitializeEvents()
    {
        _villiageDialogue.OnContinue += StartMovement;
        _player.OnMessageClick += ShowMessageClick;
    }
    
    private void ShowMessageClick()
    {
        
        bool visible = _helpMessageClick.visible;
        
        _helpMessage.visible = _helpMessageClick.visible = !visible;
        _helpMessage.SetEnabled(!visible);
        _helpMessageClick.SetEnabled(!visible);
        
        Debug.Log("Show Message: " + _helpMessageClick.visible);
    }
    void StartMovement()
    {
        _player.SetActiveDialogue(false);
    }

    void StopMovement()
    {
        _player.SetActiveDialogue(true);
        _player.DisableAnimations();
    }

    void StartSecondPhase()
    {
        AudioManager.Instance.PlaySecondPhaseBackgroundMusic("Background2");
        SaveManager.SetPhase(1);
        StopMovement();
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
