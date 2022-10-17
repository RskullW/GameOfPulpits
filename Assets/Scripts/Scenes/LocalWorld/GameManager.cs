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

        if (SaveManager.LoadGame())
        {
            LoadGame();
        }
        
        SetMission((int)Missions.NumberMissions);

        _playerController.SetMoney(_money);
        _playerController.SetAmountMedicine(_amountOfMedicine);
        _playerController.SetAmountGarbage(_amountOfGarbage);
        _playerController.SetLevelGun(_levelGun);

        SetMoneyLabelInterface();
        SetGarbageLabelInterface();
    }

    private void StartMusic()
    {
        AudioManager.Instance.StopMusic();
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
        _playerController.OnSetMoney += SetMoneyPlayerControllerEvent;
        _playerController.OnSetMedicine += SetAmountMedicinePlayerControllerEvent;
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
        

        _money = _playerController.GetMoney();
        _amountOfMedicine = _playerController.GetAmountMedicine();
        _amountOfGarbage = _playerController.GetAmountGarbage();
        _levelGun = _playerController.GetLevelGun();
        
        SaveManager.LoadData(_player.transform.position, _money, _health, _levelGun, _amountOfGarbage, _amountOfMedicine);
    }
    private void LoadGame()
    {
        _money = SaveManager.Money;
        _health = SaveManager.Health;
        _player.transform.position = SaveManager.PositionPlayer;
        
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
    public void SetMoney(int money)
    {
        _money = money;
    }
    public void SetMoneyPlayerControllerEvent()
    {
        _money = _playerController.GetMoney();

        SetMoneyLabelInterface();
    }
    public int GetMoney()
    {
        return _money;
    }
    public void SetAmountMedicine(int amountOfMedicine)
    {
        _amountOfMedicine = amountOfMedicine;
    }
    public void SetAmountMedicinePlayerControllerEvent()
    {
        _amountOfMedicine = _playerController.GetAmountMedicine();
    }
    public int GetAmountMedicine()
    {
        return _amountOfMedicine;
    }
    public void SetLevelGun(int levelGun)
    {
        _levelGun = levelGun;
    }
    public int GetLevelGun()
    {
        return _levelGun;
    }
    public void SetAmountGarbage(int amountOfGarbage)
    {
        _amountOfMedicine = amountOfGarbage;
    }
    public void SetAmountGarbagePlayerControllerEvent()
    {
        _amountOfGarbage = _playerController.GetAmountGarbage();
        
        SetGarbageLabelInterface();
    }
    public int GetAmountGarbage()
    {
        return _amountOfGarbage;
    }

    private void PassUserItems()
    {
        _playerController.SetAmountGarbage(_amountOfGarbage);
        _playerController.SetMoney(_money);
        _playerController.SetAmountMedicine(_amountOfMedicine);
        _playerController.SetLevelGun(_levelGun);

    }

    private void SetMoneyLabelInterface()
    {
        int tempMoney = _money;
        
        for (short i = 4; i >= 0; --i)
        {
            _creditsLabel[i].text = (tempMoney % 10).ToString();
            tempMoney /= 10;
        }
    }

    private void SetGarbageLabelInterface()
    {
        int tempGarbages = _amountOfGarbage;

        for (short i = 4; i >= 0; --i)
        {
            _garbagesLabel[i].text = (tempGarbages % 10).ToString();
            tempGarbages /= 10;
        }
    }
    
    
}
