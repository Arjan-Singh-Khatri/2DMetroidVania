using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private float delay = 1f;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           
            Loadnext();
        }
    } 

    private void Loadnext()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int BuildIndex)
    {
        anim.SetTrigger("start");
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(BuildIndex);
    }
}
