using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemsBehaviour : MonoBehaviour, IDataPersistance
{
    [SerializeField]private string ID;
    private bool collected;

    [ContextMenu("New GUID ID")]
    private void NewGuidId(){
        ID = System.Guid.NewGuid().ToString();
    }
    
    public void SaveData(ref GameData gameData){
        if(gameData.itemCollected.ContainsKey(ID))
        {
            gameData.itemCollected.Remove(ID);
        }
        gameData.itemCollected.Add(ID, collected);
    }

    public void LoadData(GameData gameData){
        gameData.itemCollected.TryGetValue(ID,out collected);
        if(collected)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("Player"))
        {
            collected = true;
            Events.instance.onItemCollectedPlayer();
            //Particle Effects Maybe??
            gameObject.SetActive(false);
        }
    }
    
}
