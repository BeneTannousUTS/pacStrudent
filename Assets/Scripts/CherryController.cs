using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CherryController : MonoBehaviour
{
    public GameObject cherryPrefab; // Reference to the bonus cherry prefab
    public float spawnInterval = 10f; // Time interval between cherry spawns
    public float cherrySpeed = 2.0f; // Speed of cherry movement
    private float timeSinceLastSpawn = 0f;
    private Camera mainCamera;
    private Vector2 spawnPosition;

    private float halfHeight;
    private float halfWidth;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera = Camera.main;
        if (mainCamera != null && (SceneManager.GetActiveScene().name == "LevelOne"))
        {
            float nearClip = mainCamera.nearClipPlane;
            float farClip = mainCamera.farClipPlane;

            // Get the width and height of the near clipping plane
            halfHeight = nearClip * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            halfWidth = halfHeight * mainCamera.aspect;

            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= spawnInterval)
            {
                SpawnCherry();
                timeSinceLastSpawn = 0f;
            }
        }
    }

    private void SpawnCherry()
    {
    int side = Random.Range(0, 4); // 0: Top, 1: Bottom, 2: Left, 3: Right

    switch (side)
    {
        case 0: // Top
            spawnPosition = new Vector2(Random.Range(-halfWidth, halfWidth), halfHeight + 1.0f);
            break;

        case 1: // Bottom
            spawnPosition = new Vector2(Random.Range(-halfWidth, halfWidth), -halfHeight - 1.0f);
            break;

        case 2: // Left
            spawnPosition = new Vector2(-halfWidth - 1.0f, Random.Range(-halfHeight, halfHeight));
            break;

        case 3: // Right
            spawnPosition = new Vector2(halfWidth + 1.0f, Random.Range(-halfHeight, halfHeight));
            break;

        default:
            break;
    }

    GameObject cherry = Instantiate(cherryPrefab, spawnPosition, Quaternion.identity);

    Vector2 cameraPosition = mainCamera.transform.position;
    Vector2 moveDirection = cameraPosition - spawnPosition;
    moveDirection.Normalize();

    StartCoroutine(MoveCherry(cherry, moveDirection));
}



    private IEnumerator MoveCherry(GameObject cherry, Vector2 moveDirection)
    {
        while (true)
        {
            cherry.transform.Translate(moveDirection * cherrySpeed * Time.deltaTime);
            yield return null;
        }
    }
}
