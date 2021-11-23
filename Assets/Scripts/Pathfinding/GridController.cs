using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    private TerrainGenerator terrainGenerator;
    private Tilemap tilemap;
    public Vector3 gridStartPos;
    public Node[,] grid;

    public Vector2Int size;

    private void Start()
    {
        terrainGenerator = GameObject.FindWithTag("EnvironmentController").GetComponent<TerrainGenerator>();
        tilemap = GameObject.FindWithTag("TerrainTilemap").GetComponent<Tilemap>();
    }

    public void CreateGrid(Vector2Int size)
    {
        this.size = size;
        grid = new Node[size.x, size.y];
        gridStartPos = tilemap.transform.position;

        TerrainType terrainType;

        //Set nodes
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                terrainType = terrainGenerator.terrain[x, y];
                grid[x, y] = new Node(new Vector2Int(x, y), new Vector3(x + gridStartPos.x, y + gridStartPos.y), terrainType.walkable);
            }
        }

        //Set neighbours
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                grid[x, y].neighbours = new List<Node>();

                for(int xx = -1; xx <= 1; xx++)
                {
                    for (int yy = -1; yy <= 1; yy++)
                    {
                        if(xx == 0 && yy == 0) continue;

                        int gridX = x + xx;
                        int gridY = y + yy;
                        if(gridX < 0 || gridX >= size.x || gridY < 0 || gridY >= size.y) continue;

                        grid[x, y].neighbours.Add(grid[gridX, gridY]);
                    }
                }
            }
        }
    }

    public Node GetNodeFromPoint(Vector3 point)
    {
        float percentX = Mathf.Clamp01((point.x - gridStartPos.x) / size.x);
        float percentY = Mathf.Clamp01((point.y - gridStartPos.y) / size.y);

        int x = Mathf.FloorToInt(size.x * percentX);
        int y = Mathf.FloorToInt(size.y * percentY);

        return grid[x, y];
    }
}
