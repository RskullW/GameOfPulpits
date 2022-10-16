using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WolfLevel : MonoBehaviour
{
    [SerializeField] private WolfDialogue _wolfDialogue;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private PlayerController _player;
    void Start()
    {
        AudioManager.Instance.PlayMusic("FightMusic1");
        InitializeEvents();  
        _player.SetActiveDialogue(true);
    }

    void InitializeEvents()
    {
        _wolfDialogue.OnEndDialogue += StartMovement;
        _wolfDialogue.OnEndLevel += ProcessEndLevel;

        _enemy.OnCauseDamage += GetDamagePlayer;
        _player.OnCauseDamage += GetDamageEnemy;
    }
    void StartMovement()
    {
        _enemy.SetMovement(true);
        _player.SetActiveDialogue(false);
    }

    void ProcessEndLevel()
    {
        SaveManager.SetInformationFirstBoss();
        SceneManager.LoadScene("MainMap");
    }

    void GetDamageEnemy()
    {
        Debug.Log("GetDamageEnemy. Player deals " + _player.GetDamage() + " damage to the" + _enemy.name);
        _enemy.Health -= _player.GetDamage();
        Debug.Log("Enemy health: " + _player.GetHealth());
    }

    void GetDamagePlayer()
    {
        Debug.Log("GetDamageEnemy. "+ _enemy.name + " deals " + _enemy.Damage + " damage to the player");
        _player.SetHealth((int)(_player.GetHealth()-_enemy.Damage));
        Debug.Log("Player Health: " + _enemy.Health);
    }
}
