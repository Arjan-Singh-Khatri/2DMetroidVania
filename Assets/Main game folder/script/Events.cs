using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events instance;

    public Action OnDeathTwoHeadBridgeActive;
    private void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
