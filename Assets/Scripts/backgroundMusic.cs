using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip introMusic; // The intro music clip
    public AudioClip backgroundMusic; // The background music clip

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // Play the intro music once
        audioSource.clip = introMusic;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the intro music has finished playing
        if (!audioSource.isPlaying)
        {
            // Loop the background music
            audioSource.clip = backgroundMusic;
            audioSource.loop = true; // Set the audio source to loop the background music
            audioSource.Play();
        }
    }
}
