using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class CourtyardLevelManager : MonoBehaviour
{
    [SerializeField] private UIElementsDeath _uiElementsDeath;
    [SerializeField] private UIDocument _helpMessageUIDocument;
    [Space]
    [SerializeField] private GameObject _finalReplicObject;
    [SerializeField] private TextMeshProUGUI _finalText; 
    [Space]
    [SerializeField] private string _finalReplicsRussian;
    [SerializeField] private string _finalReplicsEnglish;
    [Space] 
    [SerializeField] private EnemyGuardDialog _dialog;
    [SerializeField] private PlayerController _player;
    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private GameObject _border;
    private Label _helpMessage;
    private VisualElement _helpMessageClick;
    [Space] 
    
    private int _numbersOfSpawnsEnemy;
    private int _numbersOfDie;
    void Start()
    {
        _numbersOfDie = _numbersOfSpawnsEnemy = 0;
        
        InitializeEvents();
        StopMovement();
        
        _finalReplicObject.SetActive(false);

        if (MenuManager.Language == Language.Eng)
        {
            _finalText.text = _finalReplicsEnglish;
        }

        else
        {
            _finalText.text = _finalReplicsRussian;
        }
        
        if (_helpMessageUIDocument != null)
        {
            _helpMessage = _helpMessageUIDocument.rootVisualElement.Q<Label>("HelpMessageLabel");
            _helpMessageClick = _helpMessageUIDocument.rootVisualElement.Q<VisualElement>("HelpMessageClick");
            SetLanguageLabel();
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
    void InitializeEvents()
    {
        _dialog.OnEndDialogue += StartFirstPhase;
        _player.OnDie += DeathProcess;
        _player.OnShowMessageClick += ShowMessageClick;
        _player.OnHideMessageClick += HideMessageClick;
        _player.OnSellerGun += ShowMessageClick;
        _player.OnSellerItems += ShowMessageClick;
        _player.OnSellerMedic += ShowMessageClick;

        foreach (var enemy in _enemies)
        {
            enemy.OnCauseDamage += () => _player.TakeDamage(enemy.Damage);

            if (enemy.TypeEnemy == TypeEnemy.People)
            {
                enemy.OnDied += DeathPeople;
            }

            else if (enemy.TypeEnemy == TypeEnemy.Outlaw)
            {
                enemy.OnDied += DeathOutlaw;
            }


        }

        _playableDirector.stopped += StopCutscene;
    }

    void StartMovement()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy.gameObject.activeSelf)
            {
                enemy.SetMovement(true);
                enemy.SetIsFirstVisiblePlayer(true);
            }
        }

        _player.SetActiveDialogue(false);
    }

    void StartFirstPhase()
    {
        _player.SetActiveDialogue(false);

        _enemies[_numbersOfSpawnsEnemy].SetMovement(true);
        _enemies[_numbersOfSpawnsEnemy++].gameObject.SetActive(true);
        
        _enemies[_numbersOfSpawnsEnemy].SetMovement(true);
        _enemies[_numbersOfSpawnsEnemy++].gameObject.SetActive(true);
    }
    
    void StopMovement()
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

    void StopCutscene(PlayableDirector playableDirector)
    {
        _dialog.StartDialog();
        _playableDirector.gameObject.SetActive(false);
    }

    void DeathOutlaw()
    {
        int number = Random.Range(1, 4);
        AudioManager.Instance.PlaySound("OutlawDeath" + number);
        _numbersOfDie++;

        if (_numbersOfDie % 2 == 0 && _numbersOfDie <=6)
        {
            _enemies[_numbersOfSpawnsEnemy].SetMovement(true);
            _enemies[_numbersOfSpawnsEnemy++].gameObject.SetActive(true);
            _enemies[_numbersOfSpawnsEnemy].SetMovement(true);
            _enemies[_numbersOfSpawnsEnemy++].gameObject.SetActive(true);

            if (_numbersOfDie == 6)
            {
                _enemies[_numbersOfSpawnsEnemy].SetMovement(true);
                _enemies[_numbersOfSpawnsEnemy++].gameObject.SetActive(true);
            }
        }

        if (_numbersOfDie == 9)
        {
            StartCoroutine(VictoryProcess());
        }
    }
    
    void DeathPeople()
    {
        int number = Random.Range(1, 3);
        AudioManager.Instance.PlaySound("OutlawDeath" + number);
        _numbersOfDie++;
        
        if (_numbersOfDie % 2 == 0 && _numbersOfDie <=6)
        {
            _enemies[_numbersOfSpawnsEnemy].SetMovement(true);
            _enemies[_numbersOfSpawnsEnemy++].gameObject.SetActive(true);
            _enemies[_numbersOfSpawnsEnemy].SetMovement(true);
            _enemies[_numbersOfSpawnsEnemy++].gameObject.SetActive(true);

            if (_numbersOfDie == 6)
            {
                _enemies[_numbersOfSpawnsEnemy].SetMovement(true);
                _enemies[_numbersOfSpawnsEnemy++].gameObject.SetActive(true);
            }
        }

        if (_numbersOfDie == 9)
        {
            StartCoroutine(VictoryProcess());
        }
    }
    
    private IEnumerator VictoryProcess()
    {
        _finalReplicObject.SetActive(true);
        _player.SetActiveDialogue(true);
        AudioManager.Instance.PlaySecondPhaseBackgroundMusic();

        yield return new WaitForSeconds(10);
        
        _finalReplicObject.SetActive(false);
        _player.SetActiveDialogue(false);
        _border.gameObject.SetActive(false);
    }

    private void DeathProcess()
    {
        StopMovement();

        foreach (var enemy in _enemies)
        {
            enemy.SetMovement(false);
        }
        
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
    

}
