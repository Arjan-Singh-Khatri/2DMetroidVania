using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMainHubFromBoss : MonoBehaviour
{
    [SerializeField] GameObject boss;

    private void Update()
    {
        if (!boss.activeSelf)
        {
            gameObject.SetActive(true);
        }
        else
            gameObject.SetActive(false);
    }
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene("MainHUB");
    }
}
