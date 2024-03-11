using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DataHandlerFile
{
    private string dirPath = "";
    private string fileName = "";

    public DataHandlerFile(string dirPath, string fileName)
    {
        this.dirPath = dirPath;
        this.fileName = fileName;
    }

    public GameData Load()
    {
        // More like reading data which is to be loaded
        string fullPath = Path.Combine(dirPath, fileName);
        GameData loadData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                // De serialize datatoload to c# data 
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);  
            }
            catch(Exception e )
            {
                Debug.Log("Error : " + e);
            }
        }
        return loadData;
    }

    public void Save(GameData gameData)
    {
        string fullPath = Path.Combine(dirPath, fileName);
        try 
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            // Serialized the C# game data in Json
            string dataToStore = JsonUtility.ToJson(gameData, true);
            // Write the file to file system
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch
        {
            Debug.LogError("Problem in Saving data in : " + fullPath);
        }
    }
}
