using UnityEngine;
using System.Collections.Generic;
using System;

public class Grid : MonoBehaviour
{
    //Variables
    public bool displayGridGizmos;

    //Layer Masks
    private LayerMask walkableMask;
    public LayerMask unwalkableMask; //Unwalkable Mask (used to determine which areas are walkable)

    private Node[,] grid;
    public TerrainType[] walkableTerrains;
    private Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();

    public Vector2 gridWorldSize;

    public float nodeRadius;
    private float nodeDiameter;

    public int obstacleProxPenalty = 0;
    private int gridSizeX, gridSizeY; //The dimensions of the grid
    private int penaltyMin = int.MaxValue;
    private int penaltyMax = int.MinValue;

    public int MaxSize => gridSizeX * gridSizeY;

    private void Awake()
    {
        //Get Diameter
        nodeDiameter = nodeRadius * 2;

        //Grid Size
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        foreach (TerrainType type in walkableTerrains)
        {
            walkableMask |= type.terrainMask.value;
            walkableRegionsDictionary.Add((int)Mathf.Log(type.terrainMask.value, 2), type.terrainPenalty);
        }

        //Create the grid
        CreateGrid();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));

        if (grid != null && displayGridGizmos)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = Color.Lerp(Color.white, Color.red, Mathf.InverseLerp(penaltyMin, penaltyMax, node.movementPenalty));
                Gizmos.color = (node.walkable) ? Gizmos.color : Color.black;


                Gizmos.DrawCube(node.worldPos, Vector2.one * (nodeDiameter - 0.1f));
            }
        }
    }


    public Node NodeFromWorldPoint(Vector2 worldPosition)
    {
        // Calculate the percentage of the world position relative to the grid
        float percentX = (worldPosition.x - transform.position.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y - transform.position.y + gridWorldSize.y / 2) / gridWorldSize.y;

        // Clamp the values before flooring them to avoid going out of bound
        int x = Mathf.FloorToInt(Mathf.Clamp(gridSizeX * percentX, 0, gridSizeX - 1));
        int y = Mathf.FloorToInt(Mathf.Clamp(gridSizeY * percentY, 0, gridSizeY - 1));

        // Return the appropriate node
        return grid[x, y];
    }

    private void BlurPenaltyMap(int blurSize)
    {
        int kernalSize = blurSize * 2 + 1;
        int kernelExtents = blurSize;

        int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
        int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = -kernelExtents; x <= kernelExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
            }

            for (int x = 1; x < gridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);

                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[removeIndex, y].movementPenalty
                    + grid[addIndex, y].movementPenalty;
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(x, 0, kernelExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];

            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernalSize * kernalSize));
            grid[x, 0].movementPenalty = blurredPenalty;

            for (int y = 1; y < gridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);

                penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex]
                    + penaltiesHorizontalPass[x, addIndex];

                blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernalSize * kernalSize));
                grid[x, y].movementPenalty = blurredPenalty;

                if (blurredPenalty > penaltyMax)
                    penaltyMax = blurredPenalty;

                if (blurredPenalty < penaltyMin)
                    penaltyMin = blurredPenalty;
            }
        }
    }

    //Returns a list of Neighbours of a node
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                int checkX = node.gridX + i;
                int checkY = node.gridY + j;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    neighbours.Add(grid[checkX, checkY]);
            }
        }

        return neighbours;
    }

    //Grid Creation
    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 worldBottomLeftCorner = (Vector2)transform.position - Vector2.right *
            gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;

        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                Vector2 worldPoint = worldBottomLeftCorner + Vector2.right * (i * nodeDiameter + nodeRadius)
                    + Vector2.up * (j * nodeDiameter + nodeRadius);

                bool walkable = (Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask) == null);
                int movementPenalty = walkable ? 0 : obstacleProxPenalty;

                RaycastHit2D hit = Physics2D.CircleCast(worldPoint, nodeRadius, Vector2.up, nodeRadius, walkableMask);

                if (hit)
                {
                    walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out int terrainPenalty);
                    movementPenalty += terrainPenalty;
                }

                grid[i, j] = new Node(walkable, worldPoint, i, j, movementPenalty);
            }
        }

        BlurPenaltyMap(3);
    }

    [Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
    
    public void RefreshGrid()
    {
        CreateGrid();  // Rebuilds grid and recalculates penalties
    }
}