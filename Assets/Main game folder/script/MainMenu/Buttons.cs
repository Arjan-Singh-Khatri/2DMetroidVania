using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour, IFade
{
    [SerializeField] Button _quit;
    [SerializeField] Button _newGame;
    [SerializeField] Button _load;
    [SerializeField] LevelLoader levelLoader;
    

    void Start(){

        if(DataPersistanceManager.Instance.gameData == null) {
            _load.interactable = false;
        }

        _quit.onClick.AddListener(() =>
        {
            DisableButtons();
            QuitGame();
        });

        _newGame.onClick.AddListener(() =>
        {
            DisableButtons();
            NewGame();
        });

        _load.onClick.AddListener(() =>
        {
            DisableButtons();
            LoadGame();
        });
    }

    void NewGame() {

        // SO MAKES A NEW GAME DATA 
        DataPersistanceManager.Instance.NewGame();

        // Then the SceneUnloaded Saves the game data to the file
        // -- FADE OUT
        SceneManager.LoadScene(1);
        // Then the SceneLoaded just Loads the game data that was just saved (New game)
    }

    void QuitGame() { 
        Application.Quit();
    }

    void LoadGame() {
        // The SceneLoaded just loads the game data last saved
        // -- FADE OUT
        SceneManager.LoadScene(1);
    }

    void DisableButtons() { 
        _quit.interactable = false;
        _newGame.interactable = false;  
        _load.interactable = false; 
    }


    public void FadeOut(){
        levelLoader.LoadLevel();
    }
}
