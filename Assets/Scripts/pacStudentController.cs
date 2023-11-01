using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public float speed = 5.0f; // Adjust the speed as needed.

    private Vector2 targetPosition;
    private Vector2 startPosition;
    private bool isLerping = false;
    private Vector2 currentInput = Vector2.zero;
    private Vector2 lastInput = Vector2.zero;
    private Animator animator;

    void Start()
    {
        // Initialize PacStudent's position at the start of the game.
        startPosition = transform.position;
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isLerping)
        {
            CheckInput();
        }
        else
        {
            MoveTowardsPosition();
        }
    }

    void CheckInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        currentInput = Vector2.zero;

        if (horizontalInput > 0)
        {
            currentInput =  Vector2.right;
        }
        else if (horizontalInput < 0)
        {
            currentInput = Vector2.left;
        }
        else if (verticalInput > 0)
        {
            currentInput = Vector2.up;
        }
        else if (verticalInput < 0)
        {
            currentInput = Vector2.down;
        }

        if (currentInput != Vector2.zero)
        {
            lastInput = currentInput;
            Vector2 nextPosition = targetPosition + currentInput;

            // Check if the next position is walkable (e.g., not a wall).
            if (IsWalkable(nextPosition))
            {
                targetPosition = nextPosition;
                isLerping = true;
            }
        }
    }

    void MoveTowardsPosition()
    {
        float step = speed * Time.deltaTime;

        // Calculate the direction to move.
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        UpdateAnimator(direction);

        // Calculate the new position based on the fixed speed.
        Vector2 newPosition = (Vector2)transform.position + direction * step;

        // Check if we have reached the target position.
        if (Vector2.Distance(newPosition, targetPosition) < step)
        {
            transform.position = targetPosition;
            isLerping = false;
        }
        else
        {
            transform.position = newPosition;
        }
    }

    private void UpdateAnimator(Vector2 direction)
    {
        // Determine the animation state based on the movement direction
        string animationState = "Right"; // Default to Idle

        if (direction.y > 0.1f)
            animationState = "Up";
        else if (direction.y < -0.1f)
            animationState = "Down";
        else if (direction.x > 0.1f)
            animationState = "Right";
        else if (direction.x < -0.1f)
            animationState = "Left";


        // Set the animation state in the Animator
        animator.Play(animationState);
    }

    bool IsWalkable(Vector2 position)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, 0.1f); // Adjust the radius as needed.

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Unwalkable"))
            {
                return false; // There is an unwalkable object at the next position.
            }
        }

        return true; // The next position is walkable.
    }
}
