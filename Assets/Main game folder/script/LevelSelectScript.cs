using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectScript : MonoBehaviour
{
    private string _sceneName;
    void Start(){
        _sceneName = gameObject.name;
    }
    

    void LoadLevel() {
        SceneManager.LoadScene(_sceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("Player")) { 
            LoadLevel();    
        }
    }

}
