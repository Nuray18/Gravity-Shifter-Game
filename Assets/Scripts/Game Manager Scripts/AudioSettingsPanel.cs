using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsPanel : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider voiceVolumeSlider;

    private void Start()
    {
        if (AudioManager.instance != null)
        {
            // 🎯 Mevcut ses seviyelerini slider’lara ata
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1);
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);
            voiceVolumeSlider.value = PlayerPrefs.GetFloat("VoiceVolume", 1);

            // 🎯 Slider’lara listener ekle
            masterVolumeSlider.onValueChanged.AddListener(AudioManager.instance.SetMasterVolume);
            sfxVolumeSlider.onValueChanged.AddListener(AudioManager.instance.SetSFXVolume);
            voiceVolumeSlider.onValueChanged.AddListener(AudioManager.instance.SetVoiceVolume);
        }
    }
}

