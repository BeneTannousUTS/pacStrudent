using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{

    public Sprite[] sprites;
    public int spritePerSecond = 6;
    public bool loop = true;
    public bool destroyOnEnd = false;

    private int index = 0;
    private Image image;
    private float timeSinceLastFrame = 0;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (!loop && index == sprites.Length) return;

        // Calculate the time required to show one frame
        float frameTime = 1.0f / spritePerSecond;

        // Accumulate time
        timeSinceLastFrame += Time.deltaTime;

        // Check if it's time to switch frames
        if (timeSinceLastFrame >= frameTime)
        {
            timeSinceLastFrame -= frameTime; // Subtract the frame time from the accumulator

            image.sprite = sprites[index];
            index++;

            if (index >= sprites.Length)
            {
                if (loop) index = 0;
                if (destroyOnEnd) Destroy(gameObject);
            }
        }
    }
}