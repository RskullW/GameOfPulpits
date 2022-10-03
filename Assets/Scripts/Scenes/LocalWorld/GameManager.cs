using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private GameObject _player;

    
    private int _money;
    private float _health;

    private Label[] _timeLabels; 
    private Button _buttonPause;
    private Button _buttonSave;
    private Button _buttonUpLevel;
    
    private void Start()
    {
        InitializeUIElements();

        if (SaveManager.LoadGame())
        {
            LoadGame();
        }
    }

    private void InitializeUIElements()
    {

        var root = _uiDocument.rootVisualElement;

        _timeLabels = new Label[4];
        _buttonPause = root.Q<Button>("PauseButton");
        _buttonSave = root.Q<Button>("SaveButton");
        _buttonUpLevel = root.Q<Button>("UpButton");

        for (int i = 1; i < 5; ++i)
        {
            _timeLabels[i-1] = root.Q<Label>("ClockNumber" + i);
        }
        
        _buttonSave.clicked += SaveGame;
    }

    private void SaveGame()
    {
        string time = "";

        foreach (var timeLabel in _timeLabels)
        {
            time += timeLabel.text;
        }

        SaveManager.LoadData(_player.transform.position, _money, _health, time);
    }

    private void LoadGame()
    {
        _money = SaveManager.Money;
        _health = SaveManager.Health;
        _player.transform.position = SaveManager.PositionPlayer;

        string time = SaveManager.Time;
        
        Debug.Log(time[0].ToString());
        _timeLabels[0].text = time[0].ToString();
        _timeLabels[1].text = time[1].ToString();
        _timeLabels[2].text = time[2].ToString();
        _timeLabels[3].text = time[3].ToString();

    }
}
