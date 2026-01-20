using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioMixer mainMixer;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip menuMusic;
    public AudioClip gameMusic;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        PlayMenuMusic();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MenuScene" || scene.name == "CreditsScene")
        {
            PlayMenuMusic();
        }
        else if (scene.name == "MainScene Eric")
        {
            PlayGameMusic();
        }
    }

    public void PlayMenuMusic()
    {
        if (menuMusic == null || musicSource == null)
        {
            return;
        }
        if (musicSource.clip == menuMusic && musicSource.isPlaying)
        {
            return;
        }

        musicSource.clip = menuMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayGameMusic()
    {
        if (gameMusic == null || musicSource == null)
        {
            return;
        }
        if (musicSource.clip == gameMusic && musicSource.isPlaying)
        {
            return;
        }

        musicSource.clip = gameMusic;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
        {
            return;
        }
        sfxSource.PlayOneShot(clip);
    }
    public void SetMusicVolume(float valueDb)
    {
        if (mainMixer != null)
        {
            mainMixer.SetFloat("VolumeMusic", valueDb);
        }

    }
    public void SetSFXVolume(float valueDb)
    {
        if (mainMixer != null)
        {
            mainMixer.SetFloat("VolumeSFX", valueDb);
        }
    }
}