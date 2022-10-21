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
    [SerializeField] private List<Enemy> _enemySecondPhase;
    [SerializeField] private Enemy _boss;
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

        foreach (var enemy in _enemySecondPhase)
        {
            enemy.OnDied += DeathOutlaw;
            enemy.gameObject.SetActive(false);
        }
        
        _boss.gameObject.SetActive(false);
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
        _player.OnDie += DeathProcess;

        _boss.OnCauseDamage += () => _player.TakeDamage(_boss.Damage);
        _boss.OnDied += DeathBoss;
        
        _playableDirector.stopped += StopCutscene;
    }

    void StartMovement()
    {
        if (!_isStartSecondPhaseFight)
        {
            foreach (var enemy in _enemies)
            {
                if (enemy.gameObject.activeSelf)
                {
                    enemy.SetMovement(true);
                    enemy.SetIsFirstVisiblePlayer(true);
                }
            }

        }
        else
        {
            foreach (var enemy in _enemySecondPhase)
            {
                if (enemy.gameObject.activeSelf)
                {
                    enemy.SetMovement(true);
                    enemy.SetIsFirstVisiblePlayer(true);
                }
            }
        }

        _player.SetActiveDialogue(false);
        _boss.SetMovement(true);
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

        if (_enemySecondPhase.Count > 0)
        {
            foreach (var enemy in _enemySecondPhase)
            {
                if (enemy.gameObject.activeSelf)
                {
                    enemy.SetMovement(false);
                }    
            }
        }
        
        _boss.SetMovement(false);
        
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
        
        _boss.SetActiveNavMeshAgent(false);

        foreach (var enemy in _enemySecondPhase)
        {
            if (!enemy.gameObject.activeSelf)
            {
                enemy.gameObject.SetActive(true);
            }
            
            enemy.SetMovement(false);
            enemy.gameObject.SetActive(false);
        }
        foreach (var enemy in _enemies)
        {
            enemy.gameObject.SetActive(false);
        }
        
        _isStartSecondPhaseFight = true;
        _iconBoss.sprite = _iconSecondBoss;

        _playableDirector.Play();
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
        _boss.SetActiveNavMeshAgent(true);
        _healthBar.SetActive(true);

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

        if (_enemySecondPhase.Count > 0)
        {
            foreach (var enemy in _enemySecondPhase)
            {
                if (!enemy.gameObject.activeSelf)
                {
                    enemy.gameObject.SetActive(true);
                }

                enemy.SetMovement(true);
            }
        }
        
        _boss.gameObject.SetActive(true);
        _boss.SetMovement(true);
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

    void DeathBoss()
    {
        StartCoroutine(VictoryProcess());
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
            _healthBarBoss.fillAmount = _boss.Health/_boss.StartHealth;
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
        
        yield return new WaitForSeconds(10);
        
        SaveManager.SetAmountGarbage(_player.GetAmountGarbage()+10);
        SaveManager.SetAmountMedicine(_player.GetAmountMedicine()+5);
        SaveManager.SetLevelGun(_player.GetLevelGun()+1);
        SaveManager.SetMoney(_player.GetMoney()+13);
        SaveManager.SetHealth(_player.GetHealth());
        SaveManager.SetIsHavePositionMap(true);
        VilliageLevel.IS_SECOND_PHASE_LEVEL = true;
        SceneManager.LoadScene("Village");
    }

    private void DeathProcess()
    {
        StopMovement();

        foreach (var enemy in _enemies)
        {
            enemy.SetMovement(false);
        }

        foreach (var enemy in _enemySecondPhase)
        {
            enemy.SetMovement(false);
        }

        if (_boss.gameObject.activeSelf)
        {
            _boss.SetMovement(false);
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
