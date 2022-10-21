using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class OutlawLevel : MonoBehaviour
{
    [SerializeField] private UIElementsDeath _uiElementsDeath;
    [Space]
    [SerializeField] private GameObject _finalReplicObject;
    [SerializeField] private TextMeshProUGUI _finalText;
    [Space]
    [SerializeField] private string _finalReplicsRussian;
    [SerializeField] private string _finalReplicsEnglish;
    [Space] 
    [SerializeField] private Sprite _iconSecondBoss;
    [SerializeField] private Image _iconBoss;
    [Space]
    [SerializeField] private UnityEngine.UI.Image _healthBarBoss;
    [SerializeField] private OutlawDialogue _outlawDialogue;
    [SerializeField] private PlayerController _player;
    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private List<Enemy> _enemies;
    [Space] 
    
    private int _numberOfDieOutlaws;
    private bool _isStartSecondPhaseFight;
    private float _maxHealthOutlaws;
    private GameObject _healthBar;
    void Start()
    {
        AudioManager.Instance.SetIsPlayBackgroundMusic(false);
        AudioManager.Instance.PlayMusic("FightMusic2");
        
        InitializeEvents();
        InitializeConditions();
        StopMovement();
        
        SetLanguage(MenuManager.Language);
        _finalReplicObject.SetActive(false);
        _healthBarBoss.fillAmount = 1f;
    }

    void InitializeConditions()
    {
        _maxHealthOutlaws = 0f;
        foreach (var enemy in _enemies)
        {
            enemy.gameObject.SetActive(true);
            _maxHealthOutlaws += enemy.Health;
            enemy.gameObject.SetActive(false);
        }
    }
    void SetLanguage(Language language)
    {
        if (language == Language.Rus)
        {
            _finalText.text = _finalReplicsRussian;
        }

        else
        {
            _finalText.text = _finalReplicsEnglish;
        }
    }
    void InitializeEvents()
    {
        _outlawDialogue.OnEndDialogue += StartFirstPhase;
        _outlawDialogue.OnEndLevel += ProcessEndLevel;
        _player.OnCauseDamage += SetHealthBar;

        _playableDirector.stopped += StopCutscene;
    }
    void StartMovement()
    {
        foreach (var enemy in _enemies)
        {
            enemy.SetMovement(true);
            enemy.SetIsFirstVisiblePlayer(true);
        }
        
        _player.SetActiveDialogue(false);

        _healthBar.SetActive(true);
    }

    void StartFirstPhase()
    {
        _player.SetActiveDialogue(false);
        _healthBar.SetActive(true);

        foreach (var enemy in _enemies)
        {
            enemy.SetMovement(true);
            enemy.gameObject.SetActive(false);
        }
        
        StartCoroutine(SpawnEnemies());
    }
    void StopMovement()
    {
        _player.SetActiveDialogue(true);
        _player.DisableAnimations();
        
        _healthBar = _healthBarBoss.transform.parent.gameObject;
        _healthBar.SetActive(false);

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

    void ProcessEndLevel()
    {
        SaveManager.SetAmountGarbage(0);
        
        SaveManager.SetMoney(0);
        SaveManager.SetHealth(_player.GetHealth());
        SaveManager.SetIsHavePositionMap(true);
        
        AudioManager.Instance.PlayBackgroundMusic();
        SceneManager.LoadScene("MainMap");
    }
    
    void StartSecondPhaseFight()
    {
        
        Debug.Log("SecondBoss: Start 2 Phase");
        AudioManager.Instance.PlayMusic("OutlawSecondPhase");
        
        foreach (var enemy in _enemies)
        {
            enemy.gameObject.SetActive(false);
        }
        
        _isStartSecondPhaseFight = true;
        _iconBoss.sprite = _iconSecondBoss;

        StopMovement();
        
    }

    IEnumerator SpawnEnemies()
    {
        float time = 10;
        foreach (var enemy in _enemies)
        {
            enemy.gameObject.SetActive(true);
            enemy.OnDied += DeathOutlaw;
            
            yield return new WaitForSeconds(time);
            time -= 2f;

            if (time <= 0f)
            {
                time = 2f;
            }

            if (_isStartSecondPhaseFight)
            {
                break;
            }
        }
    }
    void StopCutscene(PlayableDirector playableDirector)
    {
        _player.SetActiveDialogue(false);
    }

    void DeathOutlaw()
    {
        _numberOfDieOutlaws++;
        int number = Random.Range(1, 4);
        AudioManager.Instance.PlaySound("OutlawDeath" + number);

        if (_numberOfDieOutlaws == _enemies.Count && !_isStartSecondPhaseFight)
        {
            StartSecondPhaseFight();
        }
    }
    private void SetHealthBar()
    {

        if (!_isStartSecondPhaseFight)
        {
            float health = 0f;

            foreach (var enemy in _enemies)
            {
                health += enemy.Health;
            }

            _healthBarBoss.fillAmount = 0.5f + health/(2*_maxHealthOutlaws);
        }

        else
        {
            // TODO: Добавить изменение хп у босса
            _healthBarBoss.fillAmount = 1f;
        }
        if (_healthBarBoss.fillAmount <= 0.5f && !_isStartSecondPhaseFight)
        {
            _healthBarBoss.fillAmount = 1f;
            StartSecondPhaseFight();

            Debug.Log("SecondBoss: Start 2 Boss Phase");
        }
    }

    private IEnumerator VictoryProcess()
    {
        _finalReplicObject.SetActive(true);
        _player.SetActiveDialogue(true);
        
        AudioManager.Instance.PlayBackgroundMusic();
        yield return new WaitForSeconds(4);
        
        ProcessEndLevel();
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
}
