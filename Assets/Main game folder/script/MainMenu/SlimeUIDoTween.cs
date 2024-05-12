using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeUIDoTween : MonoBehaviour
{
    public float movementDistance = 20; 
    public float movementSpeed = .2f; 

    private RectTransform rectTransform;
    private Vector3 startPosition;

    void Start()
    {
        // Get the RectTransform component of the UI image
        rectTransform = GetComponent<RectTransform>();

        // Store the initial position as the starting position
        startPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        // Calculate the next position using Mathf.Sin to create back-and-forth movement
        float newX = startPosition.x + Mathf.Sin(Time.time * movementSpeed) * movementDistance;

        // Update the anchored position of the RectTransform
        rectTransform.anchoredPosition = new Vector2(newX, startPosition.y);
    }
}
