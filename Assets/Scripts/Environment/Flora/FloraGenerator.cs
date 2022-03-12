using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloraGenerator : MonoBehaviour
{
    private const float MaxNutrition = 200f;
    private const float MinNutrition = 20f;

    public GameObject foodPrefab;
    public int foodCount;
    public int availableTilesPerFood;
    public bool useRatio;

    private GridController gridController;
    [HideInInspector]
    public Transform foodParent;
    private List<Node> availableSpaces;

    private float timeToSpawn;
    public float roughTimeToSpawn;

    private void Start()
    {
        gridController = GameObject.FindWithTag("Pathfinding").GetComponent<GridController>();
        foodParent = GameObject.FindWithTag("FoodParent").transform;

        ResetSpawnClock();
    }

    private void Update()
    {
        if(gridController.grid != null)
        {
            timeToSpawn -= Time.deltaTime * TimeControls.timeScale;
            if(timeToSpawn < 0)
            {
                GetApplicableSpaces();
                SpawnFlora();
                ResetSpawnClock();
            }
        }
    }

    private void ResetSpawnClock()
    {
        timeToSpawn = Tools.DeviateByPercent(roughTimeToSpawn, 0.2f, 0.1f);
    }

    private void SpawnFlora()
    {
        if (availableSpaces.Count == 0)
        {
            return;
        }

        Node space = availableSpaces[Random.Range(0, availableSpaces.Count)];
        Vector3 pos = space.worldPosition + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0);

        GameObject foodObject = Instantiate(foodPrefab, pos, Quaternion.identity, foodParent);

        foodObject.GetComponent<FoodUnit>().Initialize(Random.Range(MinNutrition / 2f, MinNutrition), Random.Range(MinNutrition, MaxNutrition), Random.Range(0.5f, 3f));
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
            SpawnFlora();
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
