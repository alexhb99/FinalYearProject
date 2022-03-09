using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntColony : MonoBehaviour
{
    public GameObject antPrefab;

    private List<GameObject> antObjects;
    private PheromoneController pheromoneController;

    public void CreateAnts(int amount, float pheromoneDissipateSpeed, float pheromoneMaxIntensity, float searchPheromoneCapacity, float returnPheromoneCapacity)
    {
        pheromoneController = GetComponent<PheromoneController>();
        pheromoneController.Initialize(pheromoneDissipateSpeed, pheromoneMaxIntensity, searchPheromoneCapacity, returnPheromoneCapacity);

        antObjects = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            SpawnAnt();
        }
    }

    private void SpawnAnt()
    {
        GameObject newAnt = Instantiate(antPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))), transform.GetChild(0));
        newAnt.GetComponent<PheromoneUnit>().StartPheromone(this, pheromoneController);
        
        antObjects.Add(newAnt);
    }
}
