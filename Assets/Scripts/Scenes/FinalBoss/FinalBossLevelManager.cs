using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class FinalBossLevelManager : MonoBehaviour
{
    [SerializeField] private UIElementsDeath _uiElementsDeath;
    [SerializeField] private PlayerController3D _player;
    [SerializeField] private Enemy3D _enemy;
    [SerializeField] private KingDialog _dialog;
    void Start()
    {
        Cursor.visible = false;
        StartDialog();
        InitializeEvents();
        StopMovement();
    }

    private void StartDialog()
    {
        _dialog.gameObject.SetActive(true);
        _dialog.StartDialog();
        _player.SetActiveDialog(true);
    }
    private void InitializeEvents()
    {
        _enemy.OnDeath += VictoryProcess;
        _player.OnDeath += DeathProcess;
        _dialog.OnEndDialogue += StartFight;
    }

    private void StartFight()
    {
        AudioManager.Instance.PlayMusic("FinalBoss");
        StartMovement();
    }

    private void StartMovement()
    {
        _player.SetActiveDialog(false);
        _player.SetMovement(true);
        _enemy.SetMovement(true);
    }

    private void StopMovement()
    {
        _player.SetMovement(false);
        _enemy.SetMovement(false);
    }
    private void VictoryProcess()
    {
        // AUDIOMANAGER DEATH
        _enemy.gameObject.SetActive(false);
        Debug.Log("Victory");
    }

    private void DeathProcess()
    {
        _enemy.SetMovement(false);
        _player.SetMovement(false);
        _uiElementsDeath.StartScreenDeath();
    }
}
