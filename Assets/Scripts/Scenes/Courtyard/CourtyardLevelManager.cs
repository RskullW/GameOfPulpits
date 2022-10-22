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
    [Space] 
    
    private int _numberOfDieOutlaws;
    private GameObject _healthBar;
    void Start()
    {
        AudioManager.Instance.SetIsPlayBackgroundMusic(false);
        AudioManager.Instance.PlayMusic("FightMusic2");
        
        InitializeEvents();
        InitializeConditions();
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

    }

    void InitializeConditions()
    {
        foreach (var enemy in _enemies)
        {
            enemy.gameObject.SetActive(true);
            enemy.gameObject.SetActive(false);
        }
    }

    void InitializeEvents()
    {
        _dialog.OnEndDialogue += StartFirstPhase;
        _player.OnDie += DeathProcess;

        foreach (var enemy in _enemies)
        {
            if (enemy.TypeEnemy == TypeEnemy.People)
            {
                enemy.OnCauseDamage += () => _player.TakeDamage(enemy.Damage);
                
                if (enemy.TypeEnemy == TypeEnemy.Outlaw)
                {
                    enemy.OnDied += DeathOutlaw;
                }
            
                else if (enemy.TypeEnemy == TypeEnemy.Wolf)
                {
                    enemy.OnDied += DeathWolf;
                }
            
                else if (enemy.TypeEnemy == TypeEnemy.People)
                {
                    enemy.OnDied += DeathPeople;
                }
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
        
        Debug.Log("Courtyard: Start 2 Phase");
        AudioManager.Instance.PlayMusic("OutlawSecondPhase");

        _playableDirector.Play();
        StopMovement();
        
    }

    IEnumerator SpawnEnemies()
    {
        float time = 10;
        foreach (var enemy in _enemies)
        {
            enemy.gameObject.SetActive(true);

            yield return new WaitForSeconds(time);
            time -= 2f;

            if (time <= 0f)
            {
                time = 2f;
            }
        }
    }
    void StopCutscene(PlayableDirector playableDirector)
    {
        _player.SetActiveDialogue(false);
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
    }

    void DeathOutlaw()
    {
        int number = Random.Range(1, 4);
        AudioManager.Instance.PlaySound("OutlawDeath" + number);
    }
    
    void DeathWolf()
    {
        int number = Random.Range(1, 4);
        AudioManager.Instance.PlaySound("WolfDie" + number);
    }
    
    void DeathPeople()
    {
        int number = Random.Range(1, 3);
        AudioManager.Instance.PlaySound("OutlawDeath" + number);
    }

    void DeathBoss()
    {
        StartCoroutine(VictoryProcess());
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
