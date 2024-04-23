using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerPref :MonoBehaviour 
{
    public PlayerData1 PlayerData1;
    public itemsBehaviour items;
    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown("s"))
            save();
        else if (Input.GetKeyDown("t"))
            Load();
    }


    void save()
    {
        PlayerPrefs.SetInt("souls",PlayerData1.soul_numbers);

    }
    void Load()
    {
        PlayerData1.soul_numbers = PlayerPrefs.GetInt("souls");
        Debug.Log("hguw");
        Debug.Log(PlayerData1.soul_numbers);

    }
}
