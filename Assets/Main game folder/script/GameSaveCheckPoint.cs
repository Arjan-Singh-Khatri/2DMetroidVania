using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveCheckPoint : MonoBehaviour
{
    private Collider2D[] _overlapColliders;
    [SerializeField]private float circleCastRadius;
    public bool playerInRange = false;


    void Update()
    {
        _overlapColliders = Physics2D.OverlapCircleAll(transform.position, circleCastRadius);
        foreach (var col in _overlapColliders)
        {
            if (col.name.CompareTo("SavePoint") == 0)
            {
                Debug.Log("IN RANGE");
                playerInRange = true;
                if (Input.GetKeyDown(KeyCode.E))
                    DataPersistanceManager.Instance.SaveGame();
            }else 
                playerInRange = false;
        }
    }
}
