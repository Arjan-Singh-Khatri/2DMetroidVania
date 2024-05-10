using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainHubUI : MonoBehaviour , IDataPersistance
{
    [Header("GAME UI")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject pauseMenuPanel;
    private bool isPaused;

    [Header("Player UI")]
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private TextMeshProUGUI soulCount;
    [SerializeField] private GameObject soulParentGameobject;
    private int soulAmount = 0 ;

    void Start(){

        // EVENTS
        Events.instance.onItemCollectedPlayer += UpdateSoulCount;
        Events.instance.onHealthChangePlayer += UpdateHealthSlider;

        //UI 
        UpdateHealthSlider(GameObject.FindGameObjectWithTag("Player").GetComponent<playerDeath>().health);

        soulCount.text = $"{soulAmount}/10";

        continueButton.onClick.AddListener(() =>{
            DisableButtons();
            Continue();
        });

        mainMenuButton.onClick.AddListener(() =>{
            DisableButtons();
            MainMenu();
        });

        quitButton.onClick.AddListener(() =>{
            DisableButtons();
            QuitGame();
        });

    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) { 
                isPaused = false;
                pauseMenuPanel.SetActive(false);
            }
            else { 
                isPaused = true;
                pauseMenuPanel.SetActive(true);
            }
        }
    }

    void Continue() { 
        pauseMenuPanel.SetActive(false);
    }

    void MainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    void QuitGame() { 
        Application.Quit();
    }

    void UpdateHealthSlider(float health) { 
        playerHealthSlider.value = health;
    }

    // MILAUNA PARYO
    void UpdateSoulCount() {
        soulAmount++;
        soulCount.text = $"{soulAmount}/10";
    }

    public void SaveData(ref GameData gameData) { 
        // NOTHING
    }

    public void LoadData(GameData gameData) { 
        foreach(KeyValuePair<string, bool> pair in gameData.itemCollected) { 
            if(pair.Value == true) {
                soulAmount++;
            }
        }
    }

    void DisableButtons(){
        quitButton.interactable = false;
        continueButton.interactable = false;
        mainMenuButton.interactable = false;
    }

    private void OnDisable()
    {
        Events.instance.onItemCollectedPlayer -= UpdateSoulCount;
        Events.instance.onHealthChangePlayer -= UpdateHealthSlider;

    }

}
