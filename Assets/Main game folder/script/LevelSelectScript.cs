using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectScript : MonoBehaviour, IFade
{
    [SerializeField] LevelLoader levelLoader;
    private string _sceneName;

    void Start(){
        _sceneName = gameObject.name;
    }
    

    IEnumerator LoadLevel() {
        // Fade In
        //Events.instance.onLoadingLevel();
        //DataPersistanceManager.Instance.SaveGame();
        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadScene(_sceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("Player")) { 
            LoadLevel();    
        }
    }


    public void FadeOut()
    {
        levelLoader.LoadLevel();
    }

}
