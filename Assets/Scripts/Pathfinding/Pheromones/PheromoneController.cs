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

    private void Start()
    {
        gridController = GameObject.FindWithTag("Pathfinding").GetComponent<GridController>();
    }

    private void Update()
    {
        foreach(Pheromone p in pheromones)
        {
            p.ReduceIntensities();
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
                pheromones.Add(new Pheromone(gridController.grid[x, y].worldPosition));
            }
        }
    }

    public Pheromone PheromoneFromPos(Vector3 pos3)
    {
        Vector2 pos = new Vector2(Mathf.Round(pos3.x), Mathf.Round(pos3.y));
        try
        {
            return posToPheromone[pos];
        }
        catch
        {
            return FindPheromoneInList(pos);
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
