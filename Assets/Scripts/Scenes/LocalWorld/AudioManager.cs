using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] [Range(0f, 1f)] private float _volumeMusic;
    [SerializeField] private Sound[] _musicSounds, _backgroundMusic, _deathSounds, _sfxSounds;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource[] _sfxSource;

    private bool _isPlayBackgroundMusic;
    private bool _isPlaySecondPhaseMusicWolf;

    private bool _lock;
    private void Start()
    {

        _lock = false;
        
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

    public void SetIsPlayBackgroundMusic(bool isPlayBackgroundMusic)
    {
        _isPlayBackgroundMusic = isPlayBackgroundMusic;
    } 
    public void PlayMusic(string name)
    {
        Debug.Log("PlayMusic");
        Sound s = Array.Find(_musicSounds, x => x.Name == name);

        if (s != null)
        {
            _musicSource.clip = s.Clip;
            StartCoroutine(PlayingMusic());
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

    public void PlaySoundDeath()
    {
        if (_deathSounds != null && _deathSounds.Length > 0)
        {
            Sound s = Array.Find(_deathSounds, x => x.Name == _deathSounds[Random.Range(0, _deathSounds.Length-1)].Name);
            
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
        if (_musicSource.isPlaying)
        {
            Debug.Log("StopMusic(): " + _musicSource.clip.name);
            StartCoroutine(StopingMusic());
        }

        if (_isPlayBackgroundMusic)
        {
            _isPlayBackgroundMusic = false;
        }
    }

    private IEnumerator StopingMusic()
    {
        if (_musicSource.isPlaying)
        {
            while (_musicSource.volume > 0f && _lock)
            {
                _musicSource.volume -= 0.1f;
                yield return new WaitForSeconds(0.5f);

                if (_musicSource.volume <= 0f)
                {
                    _musicSource.volume = 0f;
                    _musicSource.Stop();
                    _lock = false;
                    break;
                }
            }
        }
    }

    private IEnumerator PlayingMusic()
    {
        if (_musicSource.isPlaying)
        {
            _lock = true;
             StopMusic();
            while (_lock);
        }

        _musicSource.volume = 0;
        _musicSource.Play();

        while (_musicSource.volume < _volumeMusic)
        {
            _musicSource.volume += 0.05f;
            yield return new WaitForSeconds(0.5f);
        }

        _musicSource.volume = _volumeMusic;
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
            StartCoroutine(PlayingMusic());
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
