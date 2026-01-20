using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioOptionsUI : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider musicSlider;
    public Slider sfxSlider;

    public AudioSource sfxPreviewSource;
    public AudioClip sfxPreviewClip;

    const string MUSIC_PARAM = "VolumeMusic";
    const string SFX_PARAM = "VolumeSFX";

    float nextPreviewTime;

    void Start()
    {
        float value;

        if (mixer.GetFloat(MUSIC_PARAM, out value))
            musicSlider.value = value;

        if (mixer.GetFloat(SFX_PARAM, out value))
            sfxSlider.value = value;

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }
    public void SetMusicVolume(float value)
    {
        mixer.SetFloat(MUSIC_PARAM, value);
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat(SFX_PARAM, value);
    }

    public void PlaySFXPreviewThrottled()
    {
        if (sfxPreviewSource == null || sfxPreviewClip == null)
            return;

        sfxPreviewSource.PlayOneShot(sfxPreviewClip);
    }
}
