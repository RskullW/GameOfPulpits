using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField] private Sound[] _musicSounds, _backgroundMusic, _sfxSounds;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource[] _sfxSource;

    private bool _isPlayBackgroundMusic;
    private void Start()
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
        bool isPlay = false;
        Sound s = Array.Find(_sfxSounds, x => x.Name == name);
        
        foreach (var source in _sfxSource)
        {
            if (source.isPlaying)
            {
                isPlay = false;
                continue;
            }

            else
            {
                if (s != null)
                {
                    source.PlayOneShot(s.Clip);
                }
                else
                {
                    Debug.LogError("Error. Sound not found");
                }
                
                isPlay = true;
                break;
            }
        }

        if (!isPlay)
        {
            if (s != null)
            {
                _sfxSource[0].PlayOneShot(s.Clip);
            }
            else
            {
                Debug.LogError("Error. Sound not found");
            }
        }
       
        
    }

    public void StopMusic()
    {
        _musicSource.Stop();

        if (_isPlayBackgroundMusic)
        {
            _isPlayBackgroundMusic = false;
        }
    }

    public void StopSounds()
    {
        foreach (var source in _sfxSource)
        {
            source.Stop();
        }
      
    }

    public void PlayBackgroundMusic()
    {
        int numberMusic = Random.Range(0, _backgroundMusic.Length - 1);

        Sound s = _backgroundMusic[numberMusic];

        if (s != null)
        {
            _musicSource.clip = s.Clip;
            _musicSource.Play();
            _isPlayBackgroundMusic = true;
            Invoke("AudioFinished", _musicSource.clip.length);
            
            Debug.Log("PlayBackgroundMusic(): name = " + _backgroundMusic[numberMusic].Name);
        }
        

        else
        {
            Debug.LogError("Error. Sound not found");
        }

    }

    private void AudioFinished()
    {
        if (_isPlayBackgroundMusic)
        {
            PlayBackgroundMusic();
        }
    }

    public bool GetBackgroundMusic()
    {
        return _isPlayBackgroundMusic;
    }
}
