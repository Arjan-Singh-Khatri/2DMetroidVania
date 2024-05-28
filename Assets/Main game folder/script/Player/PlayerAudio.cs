using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioClip[]sfxClips;
    private AudioSource sfxSource;

    private void Awake()
    {
        sfxSource = GetComponent<AudioSource>();
    }


}
