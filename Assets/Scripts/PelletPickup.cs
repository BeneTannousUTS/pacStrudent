using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudent : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip pelletEatenSound;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 1.0f; // Adjust the volume as needed.
     
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
        } else 
        if (collision.gameObject.CompareTag("PowerPellet"))
        { 
            // Destroy the power pellet GameObject
            Destroy(collision.gameObject);
        }
    }
}
