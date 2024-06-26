using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMusic : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    // Start is called before the first frame update
    void Start()
    {
        VagabondEvents.instance.onBossAwake += ActivateAudio;
    }

    void ActivateAudio() { 
        musicSource.Play();
    }

    private void OnDisable()
    {
        VagabondEvents.instance.onBossAwake -= ActivateAudio;
    }
}
