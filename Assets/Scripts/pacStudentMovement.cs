using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pacStudentMovement : MonoBehaviour
{
    public Vector2[] cornerCoordinates = new Vector2[]
    {
        new Vector2(1f, -1f),
        new Vector2(6f, -1f),
        new Vector2(6f, -5f),
        new Vector2(1f, -5f)
    };

    public float moveSpeed = 2.0f; // Speed of movement

    private int currentCornerIndex = 0; // Index of the current corner
    private Vector2 currentTarget; // Target position for movement

    void Start()
    {
        // Set the initial target position
        currentTarget = cornerCoordinates[currentCornerIndex];

        // Set the sprite's position to match the first corner
        transform.position = new Vector2(cornerCoordinates[currentCornerIndex].x, cornerCoordinates[currentCornerIndex].y);
    }

    void Update()
    {
        // Calculate the direction towards the current target
        Vector2 direction = (currentTarget - (Vector2)transform.position).normalized;

        // Move the character in the calculated direction
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // Check if the character is close enough to the current target
        if (Vector2.Distance(transform.position, currentTarget) < 0.1f)
        {
            // Move to the next corner in a clockwise fashion
            currentCornerIndex = (currentCornerIndex + 1) % cornerCoordinates.Length;
            currentTarget = cornerCoordinates[currentCornerIndex];
        }
    }
}
