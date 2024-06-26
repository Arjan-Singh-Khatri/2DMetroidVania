using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData1 : MonoBehaviour
{
    public int soul_numbers =0;
    public int health =100;
    public float attack = 10;

    private void Update()
    {
        Debug.Log(soul_numbers);
    }

}
