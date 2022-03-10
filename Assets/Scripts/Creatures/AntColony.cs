using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntColony : MonoBehaviour
{
    public GameObject antPrefab;

    private List<GameObject> antObjects;
    private List<CreatureInstance> ants;
    private List<PheromoneUnit> pheromoneUnits;

    private Creature antArchetype;

    private PheromoneController pheromoneController;

    //Params given to this ant colony
    private int amount;
    private float pheromoneDissipateSpeed;
    private float pheromoneMaxIntensity;
    private float searchPheromoneCapacity;
    private float returnPheromoneCapacity;
    private float antMaxSpeed;
    private float antAcceleration;
    private float antTurnSpeed;
    private float antRandomRotation;

    public float birthNutritionRequirement;
    private float storedNutrition;

    public void CreateAnts(int amount, float pheromoneDissipateSpeed, float pheromoneMaxIntensity, float searchPheromoneCapacity, float returnPheromoneCapacity,
        float antMaxSpeed, float antAcceleration, float antTurnSpeed, float antRandomRotation)
    {
        antArchetype = Resources.Load<CreatureDatabase>("Data/CreatureDatabase").CreatureFromName("Ant");
        storedNutrition = 0;
        //TODO - Add this to creator UI
        birthNutritionRequirement = 10;

        this.amount = amount;
        this.pheromoneDissipateSpeed = pheromoneDissipateSpeed;
        this.pheromoneMaxIntensity = pheromoneMaxIntensity;
        this.searchPheromoneCapacity = searchPheromoneCapacity;
        this.returnPheromoneCapacity = returnPheromoneCapacity;
        this.antMaxSpeed = antMaxSpeed;
        this.antAcceleration = antAcceleration;
        this.antTurnSpeed = antTurnSpeed;
        this.antRandomRotation = antRandomRotation;

        pheromoneController = GetComponent<PheromoneController>();
        pheromoneController.Initialize(pheromoneDissipateSpeed, pheromoneMaxIntensity, searchPheromoneCapacity, returnPheromoneCapacity);

        antObjects = new List<GameObject>();
        ants = new List<CreatureInstance>();
        pheromoneUnits = new List<PheromoneUnit>();
        for (int i = 0; i < amount; i++)
        {
            SpawnAnt();
        }
    }

    private void SpawnAnt()
    {
        GameObject newAnt = Instantiate(antPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))), transform.GetChild(0));
        antObjects.Add(newAnt);

        PheromoneUnit pheromoneUnit = newAnt.GetComponent<PheromoneUnit>();
        pheromoneUnit.StartPheromone(this, pheromoneController);
        pheromoneUnits.Add(pheromoneUnit);

        newAnt.GetComponent<MovementUnit>().Initialize(antMaxSpeed, antAcceleration, antTurnSpeed, antRandomRotation);

        CreatureInstance instance = newAnt.GetComponent<CreatureInstance>();
        instance.Initialize();
        //TODO - include mutation chance in UI rather than a 0.05f const here
        antArchetype.SetRandomInstance(0.05f, instance);
        ants.Add(instance);

    }

    private void Reproduce()
    {
        SpawnAnt();
        storedNutrition -= birthNutritionRequirement;
    }

    public void StoreNutrition(float amount)
    {
        storedNutrition += amount;
        if(storedNutrition > birthNutritionRequirement)
        {
            Reproduce();
        }
    }

    private void Update()
    {
        for(int i = ants.Count - 1; i >= 0; i--)
        {
            ants[i].HealTick();

            //Only start ticking hunger if not carrying nutrition
            bool dead = pheromoneUnits[i].stateMachine.carriedNutrition == 0 ? ants[i].HungerTick() : false;

            //Kill ant if dead
            if (dead)
            {
                ants.RemoveAt(i);
                pheromoneUnits.RemoveAt(i);
                GameObject antObject = antObjects[i];
                antObjects.RemoveAt(i);
                Destroy(antObject);

                //TODO - Turn into food
            }
        }
    }
}
