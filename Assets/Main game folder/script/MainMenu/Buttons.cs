using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    [SerializeField] Button _quit;
    [SerializeField] Button _newGame;
    [SerializeField] Button _load;
    

    void Start(){
        _quit.onClick.AddListener(() =>
        {
            QuitGame();
        });

        _newGame.onClick.AddListener(() =>
        {
            NewGame();
        });

        _load.onClick.AddListener(() =>
        {
            LoadGame();
        });
    }

    void NewGame() { 
    
    }

    void QuitGame() { 
        Application.Quit();
    }

    void LoadGame() { 
    
    }

}
