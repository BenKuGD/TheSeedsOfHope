using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jukebox : MonoBehaviour
{
    private AudioSource _audioSource;
    public string sceneName;

    public static Jukebox instance;
    public List<AudioClip> audioTracks = new List<AudioClip>();

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }

        _audioSource = GetComponent<AudioSource>();
        _audioSource.Stop();
    }

    private void Start()
    {
        _audioSource.clip = audioTracks[0];
        _audioSource.Play();
    }

    public void CheckTheCurrentScene()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }

    public void AudioTrackChanger()
    {
        if (sceneName == "Main Lab")
        {
            _audioSource.volume = 0.0f;
            _audioSource.Stop();
            _audioSource.clip = audioTracks[1];
            _audioSource.volume = 0.5f;
            _audioSource.Play();
        }
        else
            return;
    }

    public void ChangeTheTrack()
    {
        _audioSource.volume = 0.0f;
        _audioSource.Stop();
        _audioSource.clip = audioTracks[1];
        _audioSource.volume = 0.5f;
        _audioSource.Play();
    }
}
