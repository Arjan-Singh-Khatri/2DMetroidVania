using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events instance;

    public Action OnDeathTwoHeadBridgeActive;
    public Action StromAttackStart;
    public Action StromAttackEnd;
    public Action followPlayer;
    private void Start()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
