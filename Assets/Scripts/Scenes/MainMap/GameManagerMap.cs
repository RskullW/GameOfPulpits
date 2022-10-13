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

    private Vector3 _horsePosition;
    private Label[] _creditsLabel;
    private Label[] _clockNumberLabels;

    private Label _questLabel;
    private Label _missionLabel;
    
    void Start()
    {
        SpawnPlayer(); 
        InitializeEvent();
    }

    void SpawnPlayer()
    {
        PlayerPrefs.DeleteKey("MainMap");

        _interfaceUIDocument.gameObject.SetActive(false);
        _playerControllerMap.SetPlayerCanMove(false);

        _isFirstOpenMap = SaveManager.GetStatsMissions("MainMap");

        if (_isFirstOpenMap != 0) {
            _firstCutscene.Stop();
            
            _interfaceUIDocument.gameObject.SetActive(true);
            _playerControllerMap.SetPlayerCanMove(true);
            _mainCamera.gameObject.transform.position = _cameraPosition;
            _mainCamera.orthographicSize = _orthographicSizeCamera;
            
            _playerControllerMap.gameObject.transform.position = SaveManager.PositionHorse;
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
        string time = SaveManager.Time;
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
            _clockNumberLabels[i].text = time[i].ToString();
        }
        
        _questLabel = root.Q<Label>("QuestLabel");
        _missionLabel = root.Q<Label>("MissionLabel");
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
        _interfaceUIDocument.gameObject.SetActive(true);
        _playerControllerMap.SetPlayerCanMove(true);
            
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
}
