using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import the UnityEngine.UI namespace for Text

public class PacStudent : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip pelletEatenSound;
    public Text scoreText; // Reference to your Text component
    private int score = 0; // Initialize the score

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 1.0f;

        // Find the Text component with the "ScoreCounter" tag
        scoreText = GameObject.FindGameObjectWithTag("ScoreCounter").GetComponent<Text>();
        UpdateScoreText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pellet"))
        {
            // Play the audio effect
            if (audioSource != null)
            {
                audioSource.PlayOneShot(pelletEatenSound);
            }

            // Destroy the pellet GameObject
            Destroy(collision.gameObject);

            // Add 10 to the score and update the Text component
            score += 10;
            UpdateScoreText();
        }
        else if (collision.gameObject.CompareTag("PowerPellet"))
        {
            // Destroy the power pellet GameObject
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Cherry"))
        {
            Destroy(collision.gameObject);
            score += 100;
            UpdateScoreText();
        }
    }

    // Update the score displayed in the Text component
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "" + score;
        }
    }
}
