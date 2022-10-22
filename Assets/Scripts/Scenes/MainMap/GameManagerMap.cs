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
    [SerializeField] private PlayableDirector _secondCutscene;
    [SerializeField] private Vector3 _cameraPosition;
    [SerializeField] private float _orthographicSizeCamera;
    [Space]
    [Space]
    [SerializeField] private GameObject _objectsFirstPhase;
    [SerializeField] private GameObject _objectsSecondPhase;

    private Vector3 _horsePosition;
    private Label[] _creditsLabel;
    private Label[] _garbagesLabel;
    
    private Label _questLabel;
    private Label _missionLabel;
    private Button _saveButton;
    
    private Label _helpMessage;
    private VisualElement _helpMessageClick;
    
    void Start()
    {

        SpawnPlayer(); 
        InitializeEvent();
        StartMusic();

        if (SaveManager.IsSecondPhase >= 1)
        {
            StartSecondPhase();
        }

        else
        {
            _objectsFirstPhase.gameObject.SetActive(true);
            _objectsSecondPhase.gameObject.SetActive(false);
        }
    }

    void StartSecondPhase()
    {
        _objectsSecondPhase.gameObject.SetActive(true);
        _objectsFirstPhase.gameObject.SetActive(false);
        _playerControllerMap.SetPlayerCanMove(false);
        
        if ((SaveManager.IsHavePositionMap && SaveManager.IsHaveData) || SaveManager.IsSecondPhase != 0)
        {
            _playerControllerMap.gameObject.transform.position = SaveManager.GetMapPosition(); 
        }
        
        if (SaveManager.IsSecondPhase == 1)
        {
            _secondCutscene.Play();
            SaveManager.SetPhase(2);
        }

        else
        {
            _secondCutscene.Stop();
            _secondCutscene.gameObject.SetActive(false);
            Play();
        }

    }
    void StartMusic()
    {
        
        if (!AudioManager.Instance.GetBackgroundMusic() && !AudioManager.Instance.GetActiveMusic())
        {
            Debug.Log("GameManagerMap.StartMusic()");
            if (SaveManager.IsSecondPhase == 1)
            {
                AudioManager.Instance.PlaySecondPhaseBackgroundMusic();
            }

            else
            {
                AudioManager.Instance.PlayBackgroundMusic();
            }
        }
    }
    void SpawnPlayer()
    {

        _interfaceUIDocument.gameObject.SetActive(false);
        _playerControllerMap.SetPlayerCanMove(false);

        _isFirstOpenMap = SaveManager.GetStatsMissions("MainMap");
        
        Debug.Log("GameManagerMap.SpawnPlayer._isFirstOpenMap = " + _isFirstOpenMap);
        
        if (_isFirstOpenMap != 0) {
            _firstCutscene.Stop();

            if (SaveManager.IsSecondPhase == 0)
            {
                Play();
            }
        }
    }
    void InitializeEvent()
    {
        _firstCutscene.stopped += OnPlayableDirectorStopped;
        _secondCutscene.stopped += OnStopSecondCutscene;
        _playerControllerMap.OnHideMessage += HideMessage;
        _playerControllerMap.OnShowMessage += ShowMessage;
    }

    void InitializeUIElements()
    {
        _creditsLabel = new Label[5];
        _garbagesLabel = new Label[5];
        
        int tempMoney = SaveManager.GetMoney();
        int tempGarbage = SaveManager.AmountOfGarbage;
        var root = _interfaceUIDocument.rootVisualElement;

        for (int i = 4; i >= 0; --i)
        {
            _creditsLabel[i] = root.Q<Label>("CreditsNumber" + (i+1));
            _garbagesLabel[i] = root.Q<Label>("GarbageNumber" + (i+1));
            
            _creditsLabel[i].text = (tempMoney % 10).ToString();
            _garbagesLabel[i].text = (tempGarbage % 10).ToString();
            
            tempMoney /= 10;
            tempGarbage /= 10;
        }
        
        _questLabel = root.Q<Label>("QuestLabel");
        _missionLabel = root.Q<Label>("MissionLabel");
        _saveButton = root.Q<Button>("SaveButton");         
        _saveButton.clicked += Save;

        _helpMessage = root.Q<Label>("HelpMessageLabel");
        _helpMessageClick = root.Q<VisualElement>("HelpMessageClick");
        
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (_firstCutscene == aDirector)
        {
            Play();
        }
    }
    
    void OnStopSecondCutscene(PlayableDirector aDirector)
    {
        if (_secondCutscene == aDirector)
        {
            Play();
        }
    }

    void Play()
    {
        Debug.Log("GameManagerMap.Play()");
        _interfaceUIDocument.gameObject.SetActive(true);
        _playerControllerMap.SetPlayerCanMove(true);
        _mainCamera.transform.position = _cameraPosition;
        _mainCamera.orthographicSize = _orthographicSizeCamera;
            
        if ((SaveManager.IsHavePositionMap && SaveManager.IsHaveData) || SaveManager.IsSecondPhase != 0)
        {
            _playerControllerMap.gameObject.transform.position = SaveManager.GetMapPosition(); 
        }

        InitializeUIElements();
        SetLanguageLabel();
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
        SaveManager.SaveMap(_playerControllerMap.gameObject.transform.position, _playerControllerMap.GetMoney(),
            _playerControllerMap.GetHealth(), _playerControllerMap.GetLevelGun(),
            _playerControllerMap.GetAmountGarbage(), _playerControllerMap.GetAmountMedicine());
        SaveManager.SaveStatsMission("MainMap");

        if (SaveManager.IsSecondPhase >= 1)
        {
            SaveManager.SaveStatsMission("IsSecondPhase");
        }
        SaveManager.SaveLevel();

    }
    
    private void SetLanguageLabel()
    {
        if (_interfaceUIDocument.gameObject.activeSelf)
        {
            _helpMessage.text = "Click 'E'";

            if (MenuManager.Language == Language.Rus)
            {
                _helpMessage.text = "Нажмите 'E'";
            }
        }
    }
    
    private void ShowMessage()
    {
        _helpMessage.visible = _helpMessageClick.visible = true;
        _helpMessage.SetEnabled(true);
        _helpMessageClick.SetEnabled(true);
    }

    private void HideMessage()
    {
        _helpMessage.visible = _helpMessageClick.visible = false;
        _helpMessage.SetEnabled(false);
        _helpMessageClick.SetEnabled(false);
    }
}
