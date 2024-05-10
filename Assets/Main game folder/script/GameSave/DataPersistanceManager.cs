using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistanceManager : MonoBehaviour
{
    public static DataPersistanceManager Instance { get; private set; }

    [SerializeField] private string fileName;
    public GameData gameData;

    private List<IDataPersistance> dataPersistanceObjects;
    private DataHandlerFile dataHandlerFile;

    private void Awake(){
        if(Instance != null){
            Destroy(this.gameObject);
            Debug.LogError("More than One instance");
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandlerFile = new DataHandlerFile(Application.persistentDataPath, fileName);
    }
    private void OnEnable(){
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable(){
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        this.dataPersistanceObjects = GetAllDataPersistanceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene){
        SaveGame();
    }

    private List<IDataPersistance> GetAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();
        return new List<IDataPersistance>(dataPersistanceObjects);
    }

    public void NewGame(){
        this.gameData = new GameData();
    }

    // LOAD GAME AT MAIN MENU -- TO DO LATER
    public void LoadGame(){
        
        this.gameData = this.dataHandlerFile.Load();
        if(this.gameData == null)
        {
            Debug.LogError("No GameDataFound.");
            return;
        }
        foreach(IDataPersistance dataPersistance in dataPersistanceObjects)
        {
            dataPersistance.LoadData(gameData);
        }
    }

    // SAVE GAME AT ONLY CHECKPOINT -- TO DO LATER
    public void SaveGame(){

        if(gameData == null) {
            Debug.Log("No Game Data Was Found. Start a new Game");
        }

        foreach(IDataPersistance dataPersistance in dataPersistanceObjects)
        {
            dataPersistance.SaveData(ref gameData);
        }
        this.dataHandlerFile.Save(gameData);
    }

    // REMOVE THIS -- TO DO LATER
    private void OnApplicationQuit(){
        SaveGame();
    }
}
