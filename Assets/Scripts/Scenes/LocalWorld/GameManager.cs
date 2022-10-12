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
    
    [SerializeField] private int _money;
    [SerializeField] private int _levelGun;
    [SerializeField] private int _amountOfGarbage;
    [SerializeField] private int _amountOfMedicine;

    private float _health;

    private Label[] _timeLabels; 
    private Button _buttonPause;
    private Button _buttonSave;
    private Button _buttonUpLevel;

    private Label _questLabel;
    private Label _missionLabel;

    private Label _helpMessage;
    private VisualElement _helpMessageClick;
    
    private void Start()
    {
        InitializeUIElements();
        InitializeEvent();
        SetLanguageLabel();

        if (SaveManager.LoadGame())
        {
            LoadGame();
        }
        
        SetMission((int)Missions.NumberMissions);
        _playerController.SetMoney(_money);
        _playerController.SetAmoundMedicine(_amountOfMedicine);

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
    }
    private void InitializeUIElements()
    {

        var root = _uiDocument.rootVisualElement;

        _timeLabels = new Label[4];
        _buttonPause = root.Q<Button>("PauseButton");
        _buttonSave = root.Q<Button>("SaveButton");
        _buttonUpLevel = root.Q<Button>("UpButton");

        _questLabel = root.Q<Label>("QuestLabel");
        _missionLabel = root.Q<Label>("MissionLabel");
        _helpMessage = root.Q<Label>("HelpMessageLabel");

        _helpMessageClick = root.Q<VisualElement>("HelpMessageClick");

        for (int i = 1; i < 5; ++i)
        {
            _timeLabels[i-1] = root.Q<Label>("ClockNumber" + i);
        }
        
        _buttonSave.clicked += SaveGame;
        
        _sellersManager.SetSafeArea(root.Q<VisualElement>("SafeArea"));
    }
    private void SaveGame()
    {
        string time = "";

        foreach (var timeLabel in _timeLabels)
        {
            time += timeLabel.text;
        }

        _money = _playerController.GetMoney();
        _amountOfMedicine = _playerController.GetAmountMedicine();
        SaveManager.LoadData(_player.transform.position, _money, _health, time, _levelGun, _amountOfGarbage, _amountOfMedicine);
    }
    private void LoadGame()
    {
        _money = SaveManager.Money;
        _health = SaveManager.Health;
        _player.transform.position = SaveManager.PositionPlayer;

        string time = SaveManager.Time;
        
        _timeLabels[0].text = time[0].ToString();
        _timeLabels[1].text = time[1].ToString();
        _timeLabels[2].text = time[2].ToString();
        _timeLabels[3].text = time[3].ToString();
        
        _amountOfGarbage = SaveManager.AmountOfGarbage;
        _amountOfMedicine = SaveManager.AmountOfMedicine;
        _levelGun = SaveManager.LevelGun;
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
}
