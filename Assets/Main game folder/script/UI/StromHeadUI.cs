using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StromHeadUI : MonoBehaviour
{
    [SerializeField] private GameObject bossHealthUIPanel;
    [SerializeField] private Slider _bossHealthSlider;
    
    private void Start(){
        StromHeadEvents.instance.activateUI += ToggleUIOn;
        StromHeadEvents.instance.onBossDead += ToggleUIOff;
        StromHeadEvents.instance.onStromHeadHealthChanged += UpdateHealth;
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

    private void OnDisable(){
        StromHeadEvents.instance.activateUI -= ToggleUIOn;
        StromHeadEvents.instance.onBossDead = ToggleUIOff;
        StromHeadEvents.instance.onStromHeadHealthChanged -= UpdateHealth;
    }
}
