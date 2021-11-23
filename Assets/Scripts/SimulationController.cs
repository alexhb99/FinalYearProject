using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    private Transform creatureParent;
    public GameObject ant;
    [Min(0)]
    public int antCount;

    private TerrainGenerator terrainGenerator;
    private GridController gridController;

    private void Start()
    {
        terrainGenerator = GameObject.FindWithTag("EnvironmentController").GetComponent<TerrainGenerator>();
        gridController = GameObject.FindWithTag("Pathfinding").GetComponent<GridController>();
        creatureParent = GameObject.FindWithTag("Creatures").transform;        
    }

    private void StartSimulation()
    {
        terrainGenerator.GenerateTerrain();
        for (int i = 0; i < antCount; i++)
        {
            SpawnAnt();
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

        Instantiate(ant, new Vector3(gridController.gridStartPos.x + randX, gridController.gridStartPos.y + randY, 0), Quaternion.identity, creatureParent);
    }
}
