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
        _wolfDialogue.OnEndDialogue += StartMovement;
        _wolfDialogue.OnEndLevel += ProcessEndLevel;
        _player.SetActiveDialogue(true);
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
