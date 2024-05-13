using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveUi : MonoBehaviour
{
    [SerializeField] GameSaveCheckPoint gameSaveCheckPoint;
    [SerializeField] GameObject gameSaveUI;

    // Update is called once per frame
    void Update()
    {
        if (gameSaveCheckPoint.playerInRange){
            Show();
        }
        else
            Hide();
    }

    void Show() { 
        gameSaveUI.SetActive(true);
    }
    
    void Hide() {
        gameSaveUI.SetActive(false);
    }
}
