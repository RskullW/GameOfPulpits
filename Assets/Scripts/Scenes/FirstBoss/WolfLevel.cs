using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class WolfLevel : MonoBehaviour
{
    [SerializeField] private GameObject _finalReplicObject;
    [SerializeField] private TextMeshProUGUI _finalText;
    [SerializeField] private string _finalReplicsRussian;
    [SerializeField] private string _finalReplicsEnglish;
    [SerializeField] private UnityEngine.UI.Image _healthBarBoss;
    [SerializeField] private WolfDialogue _wolfDialogue;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private PlayerController _player;
    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private List<Enemy> _enemies;
    [Space] 

    private int _numberBossesKilled;
    private bool _isStartSecondPhaseFight;
    private GameObject _healthBar;
    void Start()
    {
        AudioManager.Instance.PlayMusic("FightMusic1");
        
        InitializeEvents();  
        _player.SetActiveDialogue(true);
        _numberBossesKilled = 0;
        
        _enemies.Add(_enemy);
        _healthBarBoss.fillAmount = 1f;
        _healthBar = _healthBarBoss.transform.parent.gameObject;
        _healthBar.SetActive(false);
        
        SetLanguage(MenuManager.Language);
        _finalReplicObject.SetActive(false);
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
        _wolfDialogue.OnEndDialogue += StartMovement;
        _wolfDialogue.OnEndLevel += ProcessEndLevel;

        _enemy.OnCauseDamage += GetDamagePlayer;
        _enemy.OnDied += SetNumberBossesKill;
        
        foreach (var enemy in _enemies)
        {
            enemy.OnDied += SetNumberBossesKill;
            enemy.OnCauseDamage += GetDamagePlayer;
        }
        _player.OnCauseDamage += GetDamageEnemy;
        _playableDirector.stopped += StopCutscene;
    }
    void StartMovement()
    {
        _enemy.SetMovement(true);
        _enemy.SetIsFirstVisiblePlayer(true);
        _player.SetActiveDialogue(false);

        _healthBar.SetActive(true);
    }

    void StartWolf(int index)
    {
        if (_enemies.Count > 0)
        {
            _enemies[index].SetMovement(true);
            _enemies[index].SetIsFirstVisiblePlayer(false);
        }
    }

    void StopMovement()
    {
        _enemy.SetMovement(false);
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
        SaveManager.SetInformationFirstBoss();
        SaveManager.SaveAmountOfGarbage(SaveManager.AmountOfGarbage + 10);
        SaveManager.SaveAmountOfMedicine(SaveManager.AmountOfMedicine + 2);
        SaveManager.SaveMoney(SaveManager.Money+12);
        SaveManager.SaveHealth(_player.GetHealth());
        SceneManager.LoadScene("MainMap");
    }

    void GetDamageEnemy()
    {

        if (_isStartSecondPhaseFight)
        {
            Debug.Log("GetDamageEnemy. Player deals " + _player.GetDamage() + " damage to the" +
                      _enemies[_numberBossesKilled].name);
            _enemies[_numberBossesKilled].SetHealth(_enemies[_numberBossesKilled].Health - _player.GetDamage());
            Debug.Log("Enemy health: " + _player.GetHealth());

            if (_numberBossesKilled == 2)
            {
                SetHealthBar();
            }
        }

        else
        {
            
            Debug.Log("GetDamageEnemy. Player deals " + _player.GetDamage() + " damage to the" + _enemy.name);
            _enemy.SetHealth(_enemy.Health - _player.GetDamage());
            SetHealthBar();
            Debug.Log("Enemy health: " + _player.GetHealth());
            
            
            if (_enemy.StartHealth / 2 >= _enemy.Health)
            {
                _isStartSecondPhaseFight = true;
                StartSecondPhaseFight();
            }
        }
    }

    void StartSecondPhaseFight()
    {

        if (_enemy.MovePoints.Count > 0)
        {
            _enemy.SetPosition(_enemy.MovePoints[0].transform.position);
            _enemy.RunSpeed *= 4;
            _enemy.WalkSpeed *= 4;
            _enemy.Damage *= 2;
            _enemy.MinCooldownAttack /= 2;
            _enemy.MaxCooldownAttack /= 2;
        }

        foreach (var enemy in _enemies)
        {
            enemy.gameObject.SetActive(true);
        }
        
        StopMovement();
        _playableDirector.Play();
        
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic("WolfSecondPhase");
        StartCoroutine(StartSoundWolfCall());
    }

    void StopCutscene(PlayableDirector playableDirector)
    {
        StartWolf(_numberBossesKilled);
        _player.SetActiveDialogue(false);
    }
    void GetDamagePlayer()
    {

        if (_player.GetIsBlock())
        {
            _player.SetHealth(_player.GetHealth()-_enemy.Damage/4);
            _player.SetLogsBar(_enemy.Damage/4);

            return;
        }
        
        _player.SetHealth(_player.GetHealth()-_enemy.Damage);
        _player.SetLogsBar(_enemy.Damage);

    }
    private IEnumerator StartSoundWolfCall()
    {
        AudioManager.Instance.PlaySound("Thunder");
        yield return new WaitForSeconds(2f);
        AudioManager.Instance.PlaySound("WolfCall");
        yield return new WaitForSeconds(5f);
        AudioManager.Instance.PlaySound("Thunder");

    }
    private void SetNumberBossesKill()
    {
        AudioManager.Instance.PlaySound("WolfDie");
        
        _enemies[_numberBossesKilled].gameObject.SetActive(false);
        
        _numberBossesKilled++;
        
        if (_numberBossesKilled == 3)
        {
            _healthBarBoss.fillAmount = 0;
            StartCoroutine(VictoryProcess());
            Debug.Log("WolfLevel.SetNumberBossesKill(): Victory");   
        }

        else
        {
            StartWolf(_numberBossesKilled);
        }
    }

    private void SetHealthBar()
    {
        var maxHp = _enemy.StartHealth;
        var hp = _enemy.Health;
        
        _healthBarBoss.fillAmount = hp / maxHp;
    }

    private IEnumerator VictoryProcess()
    {
        _finalReplicObject.SetActive(true);
        _player.SetActiveDialogue(true);
        
        AudioManager.Instance.PlayBackgroundMusic();
        yield return new WaitForSeconds(4);
        
        ProcessEndLevel();
    }
}
