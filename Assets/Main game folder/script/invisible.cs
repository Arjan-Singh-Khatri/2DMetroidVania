using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class invisible : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        Invoke("fall", 2f);
        
    }

    private void fall()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
