using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainHubUI : MonoBehaviour , IDataPersistance , IFade
{
    [Header("GAME UI")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private Button volume;
    [SerializeField] private GameObject volumePanel;
    private bool sliderPanelToggle = false;
    private bool isPaused;

    [Header("Player UI")]
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private TextMeshProUGUI soulCount;
    private int soulAmount = 0 ;


    void Start(){

        //UI 

        UpdateHealthSlider(GameObject.FindGameObjectWithTag("Player").GetComponent<playerDeath>().health);

            soulCount.text = $"{soulAmount}/10";
        continueButton.onClick.AddListener(() =>{
            DisableButtons();
            Continue();
        });

        mainMenuButton.onClick.AddListener(() =>{
            DisableButtons();
            StartCoroutine(MainMenu());
        });

        quitButton.onClick.AddListener(() =>{
            DisableButtons();
            QuitGame();
        });

        volume.onClick.AddListener(() =>
        {
            Volume();
        });

        // EVENTS

        Events.instance.onItemCollectedPlayer += UpdateSoulCount;
        Events.instance.onHealthChangePlayer += UpdateHealthSlider;


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
        EnableButtons();
        pauseMenuPanel.SetActive(false);
    }

    IEnumerator MainMenu() {
        EnableButtons();
        FadeOut();
        yield return new WaitForSeconds(.8f);
        SceneManager.LoadScene(0);
    }

    void QuitGame() {
        EnableButtons();
        Application.Quit();
    }

    void UpdateHealthSlider(float health) { 
        playerHealthSlider.value = health;
    }

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

    void Volume(){
        volumePanel.SetActive(!sliderPanelToggle);
        sliderPanelToggle = !sliderPanelToggle;
    }

    void DisableButtons(){
        quitButton.interactable = false;
        continueButton.interactable = false;
        mainMenuButton.interactable = false;
    }

    void EnableButtons() {
        quitButton.interactable = true;
        continueButton.interactable = true;
        mainMenuButton.interactable = true;
    }

    private void OnDisable()
    {
        Events.instance.onItemCollectedPlayer -= UpdateSoulCount;
        Events.instance.onHealthChangePlayer -= UpdateHealthSlider;

    }

    public void FadeOut() {
        levelLoader.LoadLevel();
    }
}
