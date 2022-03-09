using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntColony : MonoBehaviour
{
    public GameObject antPrefab;

    private List<GameObject> antObjects;
    private PheromoneController pheromoneController;

    public void CreateAnts(int amount, float pheromoneDissipateSpeed, float pheromoneMaxIntensity, float searchPheromoneCapacity, float returnPheromoneCapacity,
        float antMaxSpeed, float antAcceleration, float antTurnSpeed, float antRandomRotation)
    {
        pheromoneController = GetComponent<PheromoneController>();
        pheromoneController.Initialize(pheromoneDissipateSpeed, pheromoneMaxIntensity, searchPheromoneCapacity, returnPheromoneCapacity);

        antObjects = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            SpawnAnt(antMaxSpeed, antAcceleration, antTurnSpeed, antRandomRotation);
        }
    }

    private void SpawnAnt(float antMaxSpeed, float antAcceleration, float antTurnSpeed, float antRandomRotation)
    {
        GameObject newAnt = Instantiate(antPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))), transform.GetChild(0));
        newAnt.GetComponent<PheromoneUnit>().StartPheromone(this, pheromoneController);
        newAnt.GetComponent<MovementUnit>().Initialize(antMaxSpeed, antAcceleration, antTurnSpeed, antRandomRotation);
        antObjects.Add(newAnt);
    }
}
