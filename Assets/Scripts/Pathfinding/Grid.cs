using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{
    private Tilemap tilemap;
    private Node[,] grid;

    public Vector2Int size;

    private void Start()
    {
        tilemap = GameObject.FindWithTag("TerrainTilemap").GetComponent<Tilemap>();
    }

    public void CreateGrid(Vector2Int size)
    {
        this.size = size;
        grid = new Node[size.x, size.y];

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                grid[x, y] = new Node(new Vector2(x, y), true);
            }
        }
    }
}
