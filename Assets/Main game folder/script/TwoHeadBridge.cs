using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TwoHeadBridge : MonoBehaviour
{
    private GameObject twoHead;
    // Start is called before the first frame update
    void Start()
    {
        twoHead = GameObject.FindGameObjectWithTag("TwoHead");
        Events.instance.bridgeIsVisible += BridgeActive;
    }

    private void BridgeActive()
    {
        gameObject.SetActive(true);
    }
}
