using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{

    private void Update(){
        if (Input.GetKeyUp(KeyCode.Escape)) {
            LoadMainMenu();
        }
    }

    private void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }
}
