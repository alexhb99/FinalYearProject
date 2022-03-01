using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloraGenerator : MonoBehaviour
{
    public GameObject foodPrefab;
    public int foodCount;
    public int availableTilesPerFood;
    public bool useRatio;

    private GridController gridController;
    [HideInInspector]
    public Transform foodParent;
    private List<Node> availableSpaces;

    private void Start()
    {
        gridController = GameObject.FindWithTag("Pathfinding").GetComponent<GridController>();
        foodParent = GameObject.FindWithTag("FoodParent").transform;
    }

    public void GenerateFlora()
    {
        GetApplicableSpaces();

        if (useRatio)
        {
            foodCount = availableSpaces.Count / availableTilesPerFood;
        }

        for(int i = 0; i < foodCount; i++)
        {
            if(availableSpaces.Count == 0)
            {
                break;
            }

            Node space = availableSpaces[Random.Range(0, availableSpaces.Count)];
            Vector3 pos = space.worldPosition + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0);

            Instantiate(foodPrefab, pos, Quaternion.identity, foodParent);
        }
    }

    private void GetApplicableSpaces()
    {
        availableSpaces = new List<Node>();
        for(int x = 0; x < gridController.grid.GetLength(0); x++)
        {
            for (int y = 0; y < gridController.grid.GetLength(1); y++)
            {
                if (gridController.grid[x, y].walkable)
                {
                    availableSpaces.Add(gridController.grid[x, y]);
                }
            }
        }
    }
}
