using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioSource _audioSource;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        _audioSource = GetComponent<AudioSource>();
    }
 
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "3")
            GetComponent<AudioSource>().mute = true;
        else
            GetComponent<AudioSource>().mute = false;
    }
 
    void Destroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static void StopMusic()
    {
        _audioSource.Stop();
    }
}
