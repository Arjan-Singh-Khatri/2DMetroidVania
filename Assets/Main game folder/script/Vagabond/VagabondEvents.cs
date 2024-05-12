using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VagabondEvents : MonoBehaviour
{
    public static VagabondEvents instance;


    public Action<float> onBossHealthChange;
    public Action onBossAwake;
    public Action onBossDead;
    public Action onReturnToHub;

    private void Awake()
    {
        if (instance != null) {
            Destroy(this.gameObject);
        }
        instance = this;
    }

}
