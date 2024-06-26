using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StromHeadUI : MonoBehaviour
{
    [SerializeField] private GameObject bossHealthUIPanel;
    [SerializeField] private Slider _bossHealthSlider;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private GameObject returning;

    private void Start(){
        StromHeadEvents.instance.activateUI += ToggleUIOn;
        StromHeadEvents.instance.onBossDead += ToggleUIOff;
        StromHeadEvents.instance.onStromHeadHealthChanged += UpdateHealth;
        StromHeadEvents.instance.onReturnToHub += BackToMainHub;
    }


    void UpdateHealth(float health) { 
        _bossHealthSlider.value = health;
    }

    void ToggleUIOn(){
        bossHealthUIPanel.SetActive(true);
    }

    void ToggleUIOff() {
        bossHealthUIPanel.SetActive(false);
    }

    void BackToMainHub()
    {
        DataPersistanceManager.Instance.SaveGame();
        StartCoroutine(LoadMainHub());
    }

    IEnumerator LoadMainHub()
    {
        yield return new WaitForSeconds(2f);
        returning.SetActive(true);
        yield return new WaitForSeconds(15f);
        _levelLoader.LoadLevel();
        yield return new WaitForSeconds(.8f);
        SceneManager.LoadScene(1); // LOAD VAGABOND?
    }


    private void OnDisable(){
        StromHeadEvents.instance.activateUI -= ToggleUIOn;
        StromHeadEvents.instance.onBossDead = ToggleUIOff;
        StromHeadEvents.instance.onStromHeadHealthChanged -= UpdateHealth;
        StromHeadEvents.instance.onReturnToHub -= BackToMainHub;
    }
}
