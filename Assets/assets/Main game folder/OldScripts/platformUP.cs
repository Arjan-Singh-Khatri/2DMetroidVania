using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class platformUP : MonoBehaviour
{

    public GameObject prefab;
    public float moveSpeed = 2f;
    public float spawnRate = 0.5f;

    private float nextSpawnTime = 0f;

    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + 1f / spawnRate;
            GameObject newObject = Instantiate(prefab, transform.position, Quaternion.identity);
            Destroy(newObject, 10f);
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Platform"))
        {
            obj.transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }
    }


}
