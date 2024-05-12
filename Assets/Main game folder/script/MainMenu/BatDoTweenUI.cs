using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System.Data.Common;

public class BatDoTweenUI : MonoBehaviour
{
    [SerializeField]private Transform[] targetPositions; // Array of target positions to follow
    [SerializeField] float moveSpeed = .2f;
    public float arrivalThreshold = 0.1f; // Distance threshold to consider reaching a point

    private RectTransform rectTransform;
    private int currentTargetIndex = 0; // Index of the current target point

    void Start()
    {
        // Get the RectTransform component of the UI image
        rectTransform = GetComponent<RectTransform>();

        // Set the initial position to the first point in the path
        SetPositionToTarget();
    }

    void Update()
    {
        // Move towards the current target point
        Vector3 targetPosition = targetPositions[currentTargetIndex].position;
        float step = moveSpeed * Time.deltaTime;
        rectTransform.position = Vector3.MoveTowards(rectTransform.position, targetPosition, step);

        // Check if the image has reached the current target point
        if (Vector3.Distance(rectTransform.position, targetPosition) <= arrivalThreshold)
        {
            // Move to the next target point in the path
            currentTargetIndex = (currentTargetIndex + 1) % targetPositions.Length;
        }
    }

    // Set the position of the UI image to the current target point
    void SetPositionToTarget()
    {
        rectTransform.position = targetPositions[currentTargetIndex].position;
    }

}
