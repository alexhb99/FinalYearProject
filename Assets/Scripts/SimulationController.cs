using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    private Transform creatureParent;
    public GameObject antAStar;
    public GameObject antP;
    [Min(0)]
    public int antCount;

    public bool isPheromones;

    private TerrainGenerator terrainGenerator;
    private FloraGenerator floraGenerator;
    private GridController gridController;

    private void Start()
    {
        terrainGenerator = GameObject.FindWithTag("EnvironmentController").GetComponent<TerrainGenerator>();
        floraGenerator = terrainGenerator.GetComponent<FloraGenerator>();
        gridController = GameObject.FindWithTag("Pathfinding").GetComponent<GridController>();
        creatureParent = GameObject.FindWithTag("Creatures").transform;        
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

        if (Input.GetKey(KeyCode.X))
        {
            SpawnAnt();
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
}
