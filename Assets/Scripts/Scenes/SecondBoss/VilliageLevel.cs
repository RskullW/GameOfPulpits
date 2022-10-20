using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class VilliageLevel : MonoBehaviour
{
    [SerializeField] private VilliageDialog _villiageDialogue;
    [SerializeField] private PlayerController _player;

    public static bool IS_SECOND_PHASE_LEVEL;
    void Start()
    {
        InitializeEvents();  
        _player.SetActiveDialogue(true);

        if (IS_SECOND_PHASE_LEVEL)
        {
            StartSecondPhase();
        }
    }
    
    void InitializeEvents()
    {
        _villiageDialogue.OnContinue += StartMovement;
    }
    void StartMovement()
    {
        _player.SetActiveDialogue(false);
    }

    void StopMovement()
    {
        _player.SetActiveDialogue(true);
        _player.DisableAnimations();
    }

    void StartSecondPhase()
    {
        AudioManager.Instance.PlaySecondPhaseBackgroundMusic("Background2");
        SaveManager.SetPhase(1);
        StopMovement();
    }
}
