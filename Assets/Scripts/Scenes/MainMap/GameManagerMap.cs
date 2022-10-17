using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class GameManagerMap : MonoBehaviour
{
    private int _isFirstOpenMap;

    [SerializeField] private Missions _missions;
    [SerializeField] private UIDocument _interfaceUIDocument;
    [SerializeField] private PlayerControllerMap _playerControllerMap;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private PlayableDirector _firstCutscene;
    [SerializeField] private Vector3 _cameraPosition;
    [SerializeField] private float _orthographicSizeCamera;
    [SerializeField] private GameObject _spherePlayer;

    private Vector3 _horsePosition;
    private Label[] _creditsLabel;
    private Label[] _clockNumberLabels;

    private Label _questLabel;
    private Label _missionLabel;
    private Button _saveButton;
    
    void Start()
    {
        SpawnPlayer(); 
        InitializeEvent();
        StartMusic();
    }

    void StartMusic()
    {
        if (!AudioManager.Instance.GetBackgroundMusic())
        {
            AudioManager.Instance.PlayBackgroundMusic();
        }
    }
    void SpawnPlayer()
    {

        _interfaceUIDocument.gameObject.SetActive(false);
        _playerControllerMap.SetPlayerCanMove(false);

        _isFirstOpenMap = SaveManager.GetStatsMissions("MainMap");
      
        if (_isFirstOpenMap != 0) {
            _firstCutscene.Stop();
            Play();
            Debug.Log("GameManagerMap.SpawnPlayer()");
            _interfaceUIDocument.gameObject.SetActive(true);
            _playerControllerMap.SetPlayerCanMove(true);
            _mainCamera.gameObject.transform.position = _cameraPosition;
            _mainCamera.orthographicSize = _orthographicSizeCamera;

            if (SaveManager.IsNotFirstStartMainMap())
            {
                _playerControllerMap.gameObject.transform.position = SaveManager.GetMapPosition();
            }
        }
    }
    void InitializeEvent()
    {
        _firstCutscene.stopped += OnPlayableDirectorStopped;
    }

    void InitializeUIElements()
    {
        _creditsLabel = new Label[5];
        _clockNumberLabels = new Label[4];
        
        int tempMoney = SaveManager.Money;
        var root = _interfaceUIDocument.rootVisualElement;

        for (int i = 4; i >= 0; --i)
        {
            _creditsLabel[i] = root.Q<Label>("CreditsNumber" + (i+1));
            _creditsLabel[i].text = (tempMoney % 10).ToString();
            tempMoney /= 10;
        }

        for (short i = 3; i >= 0; --i)
        {
            _clockNumberLabels[i] = _interfaceUIDocument.rootVisualElement.Q<Label>("ClockNumber" + (i+1));
        }
        
        _questLabel = root.Q<Label>("QuestLabel");
        _missionLabel = root.Q<Label>("MissionLabel");
        _saveButton = root.Q<Button>("SaveButton");

        _saveButton.clicked += Save;
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (_firstCutscene == aDirector)
        {
            Play();
        }
    }

    void Play()
    {
        Debug.Log("GameManagerMap.Play()");
        _interfaceUIDocument.gameObject.SetActive(true);
        _playerControllerMap.SetPlayerCanMove(true);
        _spherePlayer.SetActive(true);
        _mainCamera.transform.position = _cameraPosition;
        InitializeUIElements();
        SetMission((int)Missions.NumberMissions);
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

    private void Save()
    {
        Debug.Log(_isFirstOpenMap);
        SaveManager.SaveMapPosition(_playerControllerMap.gameObject.transform.position);
        SaveManager.SaveStatsMission("MainMap");
        SaveManager.SaveLevel();

    }
}
