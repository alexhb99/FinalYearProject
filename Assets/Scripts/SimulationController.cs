using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    private Transform creatureParent;
    public GameObject antColony;
    public GameObject antAStar;
    public GameObject antP;
    [Min(0)]
    public int antCount;

    public bool isPheromones;

    private TerrainGenerator terrainGenerator;
    private FloraGenerator floraGenerator;
    private GridController gridController;
    private PheromoneController pheromoneController;

    private void Start()
    {
        terrainGenerator = GameObject.FindWithTag("EnvironmentController").GetComponent<TerrainGenerator>();
        floraGenerator = terrainGenerator.GetComponent<FloraGenerator>();
        gridController = GameObject.FindWithTag("Pathfinding").GetComponent<GridController>();
        creatureParent = GameObject.FindWithTag("Creatures").transform;
        pheromoneController = GetComponent<PheromoneController>();
    }

    public void StartSimulation(Vector2Int size, int seed, float scale, int octaves, float persistance, float lacunarity)
    {
        terrainGenerator.size = size;
        terrainGenerator.seed = seed;
        terrainGenerator.scale = scale;
        terrainGenerator.octaves = octaves;
        terrainGenerator.persistance = persistance;
        terrainGenerator.lacunarity = lacunarity;

        StartSimulation();
    }

    private void StartSimulation()
    {
        RemoveOldSimulation();

        terrainGenerator.GenerateTerrain();
        floraGenerator.GenerateFlora();

        for (int i = 0; i < antCount; i++)
        {
            SpawnAnt();
        }
    }

    private void RemoveOldSimulation()
    {
        terrainGenerator.tilemap.ClearAllTiles();
        foreach(Transform child in creatureParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in floraGenerator.foodParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartSimulation();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnAntColony();
        }
    }

    private void SpawnAnt()
    {
        int randX, randY;
        while (true)
        {
            randX = Random.Range(0, gridController.size.x);
            randY = Random.Range(0, gridController.size.y);

            if (gridController.grid[randX, randY].walkable)
            {
                break;
            }
        }

        Instantiate(isPheromones ? antP : antAStar, new Vector3(gridController.gridStartPos.x + randX, gridController.gridStartPos.y + randY, 0), Quaternion.identity, creatureParent);
    }

    private void SpawnAntColony()
    {
        int randX, randY;
        while (true)
        {
            randX = Random.Range(0, gridController.size.x);
            randY = Random.Range(0, gridController.size.y);

            if (gridController.grid[randX, randY].walkable)
            {
                break;
            }
        }

        GameObject instance = Instantiate(antColony, new Vector3(gridController.gridStartPos.x + randX, gridController.gridStartPos.y + randY, 0), Quaternion.identity, creatureParent);
        instance.GetComponent<AntColony>().CreateAnts(50, 0.25f, 10f, 100f, 100f, 5f, 5f, 6f, 0.2f);
    }
}
