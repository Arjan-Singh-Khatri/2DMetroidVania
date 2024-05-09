using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainHubUI : MonoBehaviour
{
    [Header("GAME UI")]
    [SerializeField] Button continueButton;
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button quitButton;
    [SerializeField] GameObject pauseMenuPanel;

    [Header("Player UI")]
    [SerializeField] Slider playerHealthSlider;
    [SerializeField] TextMeshProUGUI playerCoinCount;

    void Start(){
    }

    void Update(){
        
    }
}
