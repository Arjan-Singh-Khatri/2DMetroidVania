using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events instance;

    // STROM HEAD
    public Action StromAttackStart;
    public Action StromAttackEnd;
    public Action followPlayer;

    // TWO HEAD
    public Action playerInTwoHeadArea;

    // PLAYER UI 
    public Action onTakeDamagePlayer;
    public Action onItemCollectedPlayer;
    //public Action onHealthRecoveredPlayer;

    private void Start()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
