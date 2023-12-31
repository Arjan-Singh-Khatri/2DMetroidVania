using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
    public static DataPersistanceManager Instance { get; private set; }

    [SerializeField] private string fileName;
    private GameData gameData;

    private List<IDataPersistance> dataPersistanceObjects;
    private DataHandlerFile dataHandlerFile;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("More than One instance");
        }
        Instance = this;
        
    }

    private void Start()
    {
        this.dataHandlerFile = new DataHandlerFile(Application.persistentDataPath, fileName);
        this.dataPersistanceObjects = GetAllDataPersistanceObjects();
        LoadGame();
        
    }

    private List<IDataPersistance> GetAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();
        return new List<IDataPersistance>(dataPersistanceObjects);
    }

    private void NewGame()
    {
        this.gameData = new GameData();
    }

    // LOAD GAME AT MAIN MENU -- TO DO LATER
    private void LoadGame()
    {
        this.gameData = this.dataHandlerFile.Load();
        if(this.gameData == null)
        {
            Debug.LogError("No GameDataFound initializing new Game");
            NewGame();
        }
        foreach(IDataPersistance dataPersistance in dataPersistanceObjects)
        {
            dataPersistance.LoadData(gameData);
        }
    }

    // SAVE GAME AT ONLY CHECKPOINT -- TO DO LATER
    private void SaveGame()
    {
        foreach(IDataPersistance dataPersistance in dataPersistanceObjects)
        {
            dataPersistance.SaveData(ref gameData);
        }

        this.dataHandlerFile.Save(gameData);
    }

    // REMOVE THIS -- TO DO LATER
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
