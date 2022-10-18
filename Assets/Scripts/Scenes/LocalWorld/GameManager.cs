using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private SellersManager _sellersManager;
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private GameObject _player;
    [SerializeField] private Missions _missions;
    
    private float _health;

    private Button _buttonPause;
    private Button _buttonSave;
    private Button _buttonUpLevel;

    private Label _questLabel;
    private Label _missionLabel;
    private Label[] _creditsLabel;
    private Label[] _garbagesLabel;

    private Label _helpMessage;
    private VisualElement _helpMessageClick;
    
    private void Start()
    {
        StartMusic();
        
        InitializeUIElements();
        InitializeEvent();
        SetLanguageLabel();
        SetMission((int)Missions.NumberMissions);

        SetMoneyLabelInterface();
        SetGarbageLabelInterface();
    }

    private void StartMusic()
    {
        AudioManager.Instance.PlayBackgroundMusic();
    }
    private void SetLanguageLabel()
    {
        _helpMessage.text = "Click 'E'";

        if (MenuManager.Language == Language.Rus)
        {
            _helpMessage.text = "Нажмите 'E'";
        }
    }
    private void InitializeEvent()
    {
        _playerController.OnMessageClick += ShowMessageClick;
        _playerController.OnSetMoney += SetMoneyLabelInterface;
        _playerController.OnSetGarbage += SetAmountGarbagePlayerControllerEvent;

        _sellersManager.OnPassItems += PassUserItems;
    }
    private void InitializeUIElements()
    {

        var root = _uiDocument.rootVisualElement;

        _creditsLabel = new Label[5];
        _garbagesLabel = new Label[5];
        _buttonPause = root.Q<Button>("PauseButton");
        _buttonSave = root.Q<Button>("SaveButton");
        _buttonUpLevel = root.Q<Button>("UpButton");

        _questLabel = root.Q<Label>("QuestLabel");
        _missionLabel = root.Q<Label>("MissionLabel");
        _helpMessage = root.Q<Label>("HelpMessageLabel");

        _helpMessageClick = root.Q<VisualElement>("HelpMessageClick");

        for (int i = 1; i < 5; ++i)
        {
            _creditsLabel[i - 1] = root.Q<Label>("CreditsNumber" + i);
            _garbagesLabel[i - 1] = root.Q<Label>("GarbageNumber" + i);
        }

        _creditsLabel[4] = root.Q<Label>("CreditsNumber5");
        _garbagesLabel[4] = root.Q<Label>("GarbageNumber5");
        
        _buttonSave.clicked += SaveGame;
        
        _sellersManager.SetSafeArea(root.Q<VisualElement>("SafeArea"));
    }
    private void SaveGame()
    {
        SaveManager.LoadData(_player.transform.position, _playerController.GetMoney(), _health,
            _playerController.GetLevelGun(), _playerController.GetAmountGarbage(), _playerController.GetAmountMedicine());
        SaveManager.SaveGame();
    }

    private void SetMission(int numberMissions)
    {

        Missions.SetNumberMission((uint)numberMissions);
       
        _questLabel.text = _missions.EnglishQuest;
        _missionLabel.text = _missions.EnglishMissions[numberMissions];
        
        if (MenuManager.Language == Language.Rus)
        {
            _questLabel.text = _missions.RussianQuest;
            _missionLabel.text = _missions.RussianMissions[numberMissions];
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

    public void SetAmountGarbagePlayerControllerEvent()
    {
        SetGarbageLabelInterface();
    }

    private void PassUserItems()
    {
        _playerController.SetAmountGarbage(_playerController.GetAmountGarbage());
        _playerController.SetMoney(_playerController.GetMoney());
        _playerController.SetAmountMedicine(_playerController.GetAmountMedicine());
        _playerController.SetLevelGun(_playerController.GetLevelGun());

    }

    private void SetMoneyLabelInterface()
    {
        int tempMoney = _playerController.GetMoney();
        
        for (short i = 4; i >= 0; --i)
        {
            _creditsLabel[i].text = (tempMoney % 10).ToString();
            tempMoney /= 10;
        }
    }

    private void SetGarbageLabelInterface()
    {
        int tempGarbages = _playerController.GetAmountGarbage();

        for (short i = 4; i >= 0; --i)
        {
            _garbagesLabel[i].text = (tempGarbages % 10).ToString();
            tempGarbages /= 10;
        }
    }
    
    
}
