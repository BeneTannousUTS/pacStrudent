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

    int[,] expandedLevelMap;

    void Start()
    {
        clearScene();
        expandLevelMap();
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
        int numRows = expandedLevelMap.GetLength(0);
        int numCols = expandedLevelMap.GetLength(1);

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                int tileType = expandedLevelMap[row, col];
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
                        prefabToInstantiate = tJunctionPrefab; //my outside corner sprite can work as a t junction
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
        int tileType = expandedLevelMap[row, col];
        int leftTile = (col > 0) ? expandedLevelMap[row, col - 1] : 0;
        int rightTile = (col < expandedLevelMap.GetLength(1) - 1) ? expandedLevelMap[row, col + 1] : 0;
        int aboveTile = (row > 0) ? expandedLevelMap[row - 1, col] : 0;
        int belowTile = (row < expandedLevelMap.GetLength(0) - 1) ? expandedLevelMap[row + 1, col] : 0;

        bool hasPelletLeft = (col > 0) && (expandedLevelMap[row, col - 1] == 5 || expandedLevelMap[row, col - 1] == 6);
        bool hasPelletRight = (col < expandedLevelMap.GetLength(1) - 1) && (expandedLevelMap[row, col + 1] == 5 || expandedLevelMap[row, col + 1] == 6);
        bool hasPelletAbove = (row > 0) && (expandedLevelMap[row - 1, col] == 5 || expandedLevelMap[row - 1, col] == 6);
        bool hasPelletBelow = (row < expandedLevelMap.GetLength(0) - 1) && (expandedLevelMap[row + 1, col] == 5 || expandedLevelMap[row + 1, col] == 6);

        if (tileType == 1) // Outer Corner
        {
            if (leftTile == 2 && aboveTile == 2)
                return TileRotation.Rotate180; // Top-left corner, rotated 90 degrees
            else if (rightTile == 2 && aboveTile == 2)
                return TileRotation.Rotate90; // Top-right corner, rotated 180 degrees
            else if (leftTile == 2 && belowTile == 2)
                return TileRotation.Rotate270; // Bottom-left corner, default rotation
            else if (rightTile == 2 && belowTile == 2)
                return TileRotation.Default; // Bottom-right corner, rotated 270 degrees
        }
        else if (tileType == 2) // Outer Wall
        {
            if ((leftTile == 2 || rightTile == 2) && !(hasPelletLeft || hasPelletRight))
                return TileRotation.Rotate90; // Vertical rotation
            else if ((aboveTile == 2 || belowTile == 2) && !(hasPelletAbove || hasPelletBelow))
                return TileRotation.Default; // Horizontal rotation
        }
        else if (tileType == 3) // Inner Corner
        {
            bool isBottomRight = (leftTile == 4 && aboveTile == 4);
            bool isBottomLeft = (rightTile == 4 && aboveTile == 4);
            bool isTopRight = (leftTile == 4 && belowTile == 4);
            bool isTopLeft = (rightTile == 4 && belowTile == 4);

            if (isBottomRight)
            {
                if (isBottomLeft)
                    return TileRotation.Default; // U-shaped connection
                else if (isTopRight)
                    return TileRotation.Rotate270;
            }
            else if (isBottomLeft)
            {
                if (isTopRight)
                    return TileRotation.Rotate180; // U-shaped connection
            }
            else if (isTopRight)
            {
                if (isTopLeft)
                    return TileRotation.Rotate90; // U-shaped connection
            }
        }
        else if (tileType == 4) // Inner Wall
        {
            if (hasEmptySpace(leftTile) && hasEmptySpace(rightTile))
                return TileRotation.Default; // Horizontal rotation
            else if (hasEmptySpace(aboveTile) && hasEmptySpace(belowTile))
                return TileRotation.Rotate90; // Vertical rotation
        }
        else if (tileType == 7) // T-Junction
        {
            if (leftTile == 2 && rightTile == 2 && belowTile == 2)
                return TileRotation.Default;
            else if (leftTile == 2 && rightTile == 2 && aboveTile == 2)
                return TileRotation.Rotate180;
            else if (leftTile == 2 && belowTile == 2 && aboveTile == 2)
                return TileRotation.Rotate270;
            else if (rightTile == 2 && belowTile == 2 && aboveTile == 2)
                return TileRotation.Rotate90;
        }

        // Default rotation for pellets and power pellets.
        return TileRotation.Default;
    }

    bool hasEmptySpace(int tile)
    {
        return tile == 0;
    }

    void expandLevelMap()
    {
        int numRows = levelMap.GetLength(0);
        int numCols = levelMap.GetLength(1);

        expandedLevelMap = new int[numRows * 2, numCols * 2];

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                // Copy the original value to the corresponding position in the expanded map.
                expandedLevelMap[row, col] = levelMap[row, col];

                // Mirror through the x-axis.
                expandedLevelMap[row, numCols * 2 - col - 1] = levelMap[row, col];

                // Mirror through the y-axis.
                expandedLevelMap[numRows * 2 - row - 1, col] = levelMap[row, col];

                // Mirror through both x-axis and y-axis.
                expandedLevelMap[numRows * 2 - row - 1, numCols * 2 - col - 1] = levelMap[row, col];
            }
        }
    }
}