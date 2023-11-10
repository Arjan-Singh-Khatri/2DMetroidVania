using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TwoHeadBridge : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Events.instance.OnDeathTwoHeadBridgeActive += BridgeActive;

    }

    private void BridgeActive()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("Fade");
    }

    
}
