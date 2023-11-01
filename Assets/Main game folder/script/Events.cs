using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events instance;
    public Action bridgeIsVisible;

    private void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
