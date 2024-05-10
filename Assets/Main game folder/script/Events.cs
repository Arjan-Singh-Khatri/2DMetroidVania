using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events instance;


    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // STROM HEAD
    public Action StromAttackStart;
    public Action StromAttackEnd;
    public Action followPlayer;


    // PLAYER UI 
    public Action onItemCollectedPlayer;
    public Action<float> onHealthChangePlayer;
}
