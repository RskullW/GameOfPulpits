using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField] private Sound[] _musicSounds, _sfxSounds;
    [SerializeField] private AudioSource _musicSource, _sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(_musicSounds, x => x.Name == name);

        if (s != null)
        {
            _musicSource.clip = s.Clip;
            _musicSource.Play();
        }

        else
        {
            Debug.LogError("Error. Sound not found");
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(_sfxSounds, x => x.Name == name);

        if (s != null)
        {
            _sfxSource.PlayOneShot(s.Clip);
        }

        else
        {
            Debug.LogError("Error. Sound not found");
        }
    }

    public void StopMusic(string name)
    {
        _musicSource.Stop();
    }

    public void StopSound(string name)
    {
        _sfxSource.Stop();
    }
}
