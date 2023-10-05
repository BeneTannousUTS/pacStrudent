using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject outsideCornerPrefab;
    public GameObject outsideWallPrefab;
    public GameObject insideCornerPrefab;
    public GameObject insideWallPrefab;
    public GameObject standardPelletPrefab;
    public GameObject powerPelletPrefab;
    public GameObject tJunctionPrefab;

    public enum TileRotation
    {
        Default,
        Rotate90,
        Rotate180,
        Rotate270
    }


    public float tileSize = 1f;

    int[,] levelMap =
        {
            {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
            {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
            {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
            {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
            {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
            {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
            {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
            {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
            {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
            {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
            {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
            {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
            {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
            {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
            {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
        };

    void Start()
    {
        clearScene();
        GenerateMaze();
    }


    public void clearScene()
    {
        GameObject NW = GameObject.Find("NW");
        GameObject NE = GameObject.Find("NE");
        GameObject SW = GameObject.Find("SW");
        GameObject SE = GameObject.Find("SE");

        Destroy(NW);
        Destroy(NE);
        Destroy(SW);
        Destroy(SE);


    }
    void GenerateMaze()
    {
        int numRows = levelMap.GetLength(0);
        int numCols = levelMap.GetLength(1);

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                int tileType = levelMap[row, col];
                Vector3 tilePosition = new Vector3(col * tileSize, -row * tileSize, 0);

                GameObject prefabToInstantiate = null;

                // Determine which prefab to use based on the tileType.
                switch (tileType)
                {
                    case 1:
                        prefabToInstantiate = outsideCornerPrefab;
                        break;
                    case 2:
                        prefabToInstantiate = outsideWallPrefab;
                        break;
                    case 3:
                        prefabToInstantiate = insideCornerPrefab;
                        break;
                    case 4:
                        prefabToInstantiate = insideWallPrefab;
                        break;
                    case 5:
                        prefabToInstantiate = standardPelletPrefab;
                        break;
                    case 6:
                        prefabToInstantiate = powerPelletPrefab;
                        break;
                    case 7:
                        prefabToInstantiate = outsideCornerPrefab; //my outside corner sprite can work as a t junction
                        break;
                    default:
                        // Empty space (tileType 0), do nothing.
                        continue;
                }

                // Instantiate the prefab at the specified position.
                if (prefabToInstantiate != null)
                {
                    TileRotation rotation = CalculateTileRotation(row, col);

                    Quaternion tileRotation = Quaternion.Euler(0, 0, (int)rotation * 90f);
                    Instantiate(prefabToInstantiate, tilePosition, tileRotation);
                }
            }
        }
    }

    TileRotation CalculateTileRotation(int row, int col)
    {
        // Check neighboring tiles to determine rotation.
        int tileType = levelMap[row, col];
        int leftTile = (col > 0) ? levelMap[row, col - 1] : 0;
        int rightTile = (col < levelMap.GetLength(1) - 1) ? levelMap[row, col + 1] : 0;
        int aboveTile = (row > 0) ? levelMap[row - 1, col] : 0;
        int belowTile = (row < levelMap.GetLength(0) - 1) ? levelMap[row + 1, col] : 0;

        if (tileType == 1)
        {
            if (leftTile == 2 && rightTile == 2)
                return TileRotation.Rotate180;
            else if (aboveTile == 2 && belowTile == 2)
                return TileRotation.Default;
            else if (leftTile == 2 && aboveTile == 2)
                return TileRotation.Rotate270;
            else if (rightTile == 2 && aboveTile == 2)
                return TileRotation.Rotate90;
        }
        else if (tileType == 3)
        {
            // Implement logic for corner tiles.
        }

        // Default rotation.
        return TileRotation.Default;
    }

}