using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TwoHeadBridge : MonoBehaviour
{
    [SerializeField]private GameObject twoHead;

    private void Update()
    {
        if (!twoHead.activeSelf)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            twoHead.GetComponent<TwoHead>().PlayerInTwoHeadArea();
        }
    }
}
