using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VagabondEvents : MonoBehaviour
{
    public static VagabondEvents instance;

    private void Awake()
    {
        if (instance != null) {
            Destroy(this.gameObject);
        }
        instance = this;
    }

    public Action<float> onBossHealthChange;
    public Action onBossAwake;
    public Action onBossDead;
}
