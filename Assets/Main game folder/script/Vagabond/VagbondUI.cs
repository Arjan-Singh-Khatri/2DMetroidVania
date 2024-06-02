using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VagbondUI : MonoBehaviour
{
    [SerializeField] private GameObject bossHealthUIPanel;
    [SerializeField] private GameObject returning;
    [SerializeField] private Slider _bossHealthSlider;
    [SerializeField] private LevelLoader _levelLoader;

    private void Awake()
    {
        VagabondEvents.instance.onBossAwake += ToggleUIOn;
        VagabondEvents.instance.onBossDead += ToggleUIOff;
        VagabondEvents.instance.onBossHealthChange += UpdateHealth;
        VagabondEvents.instance.onReturnToHub += BackToMainHub;
    }


    void UpdateHealth(float health)
    {
        _bossHealthSlider.value = health;
    }

    void ToggleUIOn()
    {
        bossHealthUIPanel.SetActive(true);
    }

    void ToggleUIOff()
    {
        bossHealthUIPanel.SetActive(false);
    }

    void  BackToMainHub() {
        DataPersistanceManager.Instance.SaveGame();
        StartCoroutine(LoadMainHub());
    }

    IEnumerator LoadMainHub() {
        yield return new WaitForSeconds(15f);
        _levelLoader.LoadLevel();
        yield return new WaitForSeconds(.8f);
        SceneManager.LoadScene("End"); // NOT THIS BUT END CREDITS
    }
    private void OnDisable()
    {
        VagabondEvents.instance.onBossAwake -= ToggleUIOn;
        VagabondEvents.instance.onBossDead -= ToggleUIOff;
        VagabondEvents.instance.onBossHealthChange -= UpdateHealth;
        VagabondEvents.instance.onReturnToHub -= BackToMainHub;
    }
}