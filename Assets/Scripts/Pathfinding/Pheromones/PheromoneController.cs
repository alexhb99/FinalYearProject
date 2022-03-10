using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheromoneController : MonoBehaviour
{
    public bool drawGizmos;

    private GridController gridController;
    [HideInInspector]
    public List<Pheromone> pheromones = new List<Pheromone>();
    [HideInInspector]
    public Dictionary<Vector2, Pheromone> posToPheromone = new Dictionary<Vector2, Pheromone>();

    public float dissipateSpeed;
    public float maxIntensity;
    public float searchPheromoneCapacity;
    public float returnPheromoneCapacity;

    public void Initialize(float pheromoneDissipateSpeed, float pheromoneMaxIntensity, float searchPheromoneCapacity, float returnPheromoneCapacity)
    {
        gridController = GameObject.FindWithTag("Pathfinding").GetComponent<GridController>();
        dissipateSpeed = pheromoneDissipateSpeed;
        maxIntensity = pheromoneMaxIntensity;
        this.searchPheromoneCapacity = searchPheromoneCapacity;
        this.returnPheromoneCapacity = returnPheromoneCapacity;

        GeneratePheromoneGrid();
    }

    private void Update()
    {
        foreach(Pheromone p in pheromones)
        {
            p.ReduceIntensities(dissipateSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            foreach(Pheromone p in pheromones)
            {
                if(p.toFoodIntensity > p.toHomeIntensity)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(p.pos, Vector3.one * p.toFoodIntensity / 10f);

                    Gizmos.color = Color.cyan;
                    Gizmos.DrawCube(p.pos, Vector3.one * p.toHomeIntensity / 10f);
                }
                else
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawCube(p.pos, Vector3.one * p.toHomeIntensity / 10f);

                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(p.pos, Vector3.one * p.toFoodIntensity / 10f);
                }
            }
        }
    }

    public void GeneratePheromoneGrid()
    {
        pheromones = new List<Pheromone>();
        posToPheromone = new Dictionary<Vector2, Pheromone>();

        for (int x = 0; x < gridController.grid.GetLength(0); x++)
        {
            for (int y = 0; y < gridController.grid.GetLength(1); y++)
            {
                Pheromone newPheromone = new Pheromone(gridController.grid[x, y].worldPosition);
                pheromones.Add(newPheromone);
                posToPheromone.Add(new Vector2(x, y), newPheromone);
            }
        }
    }

    public Pheromone PheromoneFromPos(Vector3 pos3)
    {
        Vector2 pos = new Vector2(Mathf.Round(pos3.x), Mathf.Round(pos3.y));
        if(pos.x > 0 && pos.y > 0 && pos.x < gridController.size.x - 1 && pos.y < gridController.size.y - 1)
        {
            return posToPheromone[pos];
        }
        else
        {
            return null;
        }
    }

    private Pheromone FindPheromoneInList(Vector3 pos3)
    {
        Vector2 pos = pos3;
        foreach (Pheromone p in pheromones)
        {
            if (p.pos.x == pos.x && p.pos.y == pos.y)
            {
                posToPheromone.Add(pos, p);
                return p;
            }
        }
        //Debug.LogWarning("Can't find pheromone at " + pos3);
        Pheromone newPheromone = new Pheromone(pos);
        pheromones.Add(newPheromone);
        posToPheromone.Add(pos, newPheromone);
        return newPheromone;
    }
}
