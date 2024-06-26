using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _sfxSlider;
    public AudioMixer _audioMixerVolume;

    private void Start() {
        _musicSlider.value = DamageHolder.instance.musicVol;
        _sfxSlider.value = DamageHolder.instance.sfxVol;

        _audioMixerVolume.SetFloat("Music Volume", _musicSlider.value);
        _audioMixerVolume.SetFloat("Sfx Volume", _sfxSlider.value);

        _musicSlider.onValueChanged.AddListener(delegate { ChangeVolMusic(); });
        _sfxSlider.onValueChanged.AddListener(delegate { ChangeVolSfx(); });
    }


    private void ChangeVolMusic() { 
        _audioMixerVolume.SetFloat("Music Volume", _musicSlider.value);
        DamageHolder.instance.musicVol = _musicSlider.value;
    }

    private void ChangeVolSfx() { 
        _audioMixerVolume.SetFloat("Sfx Volume", _sfxSlider.value);
        DamageHolder.instance.sfxVol = _sfxSlider.value;

    }
}
