using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SerializeDictionaryForJson<Tkey,Tvalue> : Dictionary<Tkey,Tvalue>, ISerializationCallbackReceiver
{
    [SerializeField] List<Tkey> key;
    [SerializeField] List<Tvalue> value;


    // Save The Dicitonary to list
    public void OnBeforeSerialize()
    {
        key.Clear(); value.Clear();
        foreach(KeyValuePair<Tkey,Tvalue> pair in this)
        {
            key.Add(pair.Key);
            value.Add(pair.Value);
        }
    }


    // Load the dictionary from list 
    public void OnAfterDeserialize()
    {
        this.Clear();

        if(key.Count !=  value.Count)
        {
            Debug.LogError("Key count : " + key.Count + " Doesn't Match value count : " + value.Count);
        }
        for (int i = 0; i<key.Count; i++)
        {
            this.Add(key[i], value[i]);
        }
    }
}
