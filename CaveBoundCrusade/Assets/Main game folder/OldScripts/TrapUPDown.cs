using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapUPDown : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField] private float frequency = 1.0f;
    [SerializeField] private float amplitude = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = startPos.y + amplitude * Mathf.Sin(frequency * Time.time);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
