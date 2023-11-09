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
        Events.instance.OnDeathTwoHeadBridgeActive += BridgeActive;
        animator = GetComponent<Animator>();
    }

    private void BridgeActive()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("Fade");
    }

    
}
