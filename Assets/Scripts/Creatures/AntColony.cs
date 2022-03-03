using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntColony : MonoBehaviour
{
    private const int MinAnts = 50;
    private const int MaxAnts = 100;

    public GameObject antPrefab;

    private List<GameObject> antObjects;

    private void Start()
    {
        antObjects = new List<GameObject>();

        int numAnts = Random.Range(MinAnts, MaxAnts);
        for(int i = 0; i < numAnts; i++)
        {
            SpawnAnt();
        }
    }

    private void SpawnAnt()
    {
        GameObject newAnt = Instantiate(antPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))), transform.GetChild(0));
        newAnt.GetComponent<PheromoneUnit>().StartPheromone(this);
        antObjects.Add(newAnt);
    }
}
