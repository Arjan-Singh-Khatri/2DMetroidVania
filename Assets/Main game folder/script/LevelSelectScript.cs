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
    

    IEnumerator LoadBossLevel() {
        FadeOut();
        Events.instance.onLoadingLevel();
        //DataPersistanceManager.Instance.SaveGame();
        yield return new WaitForSeconds(.8f);
        SceneManager.LoadScene(_sceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("Player")) { 
            StartCoroutine(LoadBossLevel());    
        }
    }


    public void FadeOut()
    {
        levelLoader.LoadLevel();
    }

}
