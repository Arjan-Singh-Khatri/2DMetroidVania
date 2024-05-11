using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VagbondUI : MonoBehaviour
{
    [SerializeField] private GameObject bossHealthUIPanel;
    [SerializeField] private Slider _bossHealthSlider;

    private void Start()
    {
        VagabondEvents.instance.onBossAwake += ToggleUIOn;
        VagabondEvents.instance.onBossAwake += ToggleUIOff;
        VagabondEvents.instance.onBossHealthChange += UpdateHealth;
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

    private void OnDisable()
    {
        VagabondEvents.instance.onBossAwake -= ToggleUIOn;
        VagabondEvents.instance.onBossAwake -= ToggleUIOff;
        VagabondEvents.instance.onBossHealthChange -= UpdateHealth;
    }
}