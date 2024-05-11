using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StromHeadEvents : MonoBehaviour
{
    public static StromHeadEvents instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        instance = this;
    }

    public Action StromAttackStart;
    public Action StromAttackEnd;
    public Action onStromHeadKilled;
    public Action<float> onStromHeadHealthChanged;
    public Action activateUI;
    public Action onBossDead;
}
