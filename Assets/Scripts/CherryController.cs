using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherry;
    public float spawnInterval = 10f;
    private float lastSpawnTime = 0f;
    private Camera mainCamera;
    private float leftX, topY, rightX, bottomY, middleX, middleY;
    private float cherryX;
    private float cherryY;


    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        // Calculate the camera's dimensions
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Calculate the camera's position
        Vector3 cameraPosition = mainCamera.transform.position;

        // Calculate the x and y values of the four corners with padding
        leftX = (cameraPosition.x - cameraWidth / 2f) - 1f;
        rightX = (cameraPosition.x + cameraWidth / 2f) + 1f;
        topY = (cameraPosition.y + cameraHeight / 2f) + 1f;
        bottomY = (cameraPosition.y - cameraHeight / 2f) - 1f;
        middleX = (rightX - leftX) / 2f;
        middleY = (topY - bottomY) / 2f;
        
        lastSpawnTime += Time.deltaTime;

        if(lastSpawnTime > spawnInterval)
        {
            SpawnCherry(CherrySpawnLocation());
            MoveCherry();
            lastSpawnTime = 0f;
        }
    }

    private Vector2 CherrySpawnLocation()
    {
        float side = Random.Range(1.0f, 4.0f);

        switch (side)
        {
            case 1: // top
                cherryX = Random.Range(leftX, rightX);
                cherryY = topY;
                break;
            case 2: // bottom
                cherryX = Random.Range(leftX, rightX);
                cherryY = bottomY;
                break;
            case 3: // left
                cherryX = leftX;
                cherryY = Random.Range(bottomY, topY);
                break;
            case 4: // right
                cherryX = rightX;
                cherryY = Random.Range(bottomY, topY); ;
                break;
        }

        return new Vector2(cherryX, cherryY);
    }

    private void SpawnCherry(Vector2 spawnPos)
    {
        Instantiate(cherry, spawnPos, Quaternion.identity);
    }

    private void MoveCherry()
    {

    }
}
