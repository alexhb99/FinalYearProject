using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Creature
{
    public string name;

    //Nutrition (hunger)
    public float maxHunger;
    public float hungerRate;

    //Sleep
    public float maxSleep;
    public float fatigueRate;

    //Health
    public float maxHealth;
    public float healRate;
    public float healAtNutritionPercentage;     //If lower than this %, don't heal on update

    //Movement details
    //Algorithm type, tile weights (do they swim/walk? mud/forest?),
    //default: speed, turn, acceleration...

    //default sensor bounds (i.e. sight)

    public Creature()
    {

    }

    //Mutation power is % change:       0.1 means 10% deviation from archetype
    public void SetRandomInstance(float mutationPower, CreatureInstance instance)
    {
        instance.maxHunger = Tools.DeviateByPercent(maxHunger, mutationPower);
        instance.currentHunger = instance.maxHunger;
        instance.hungerRate = Tools.DeviateByPercent(hungerRate, mutationPower);

        instance.maxSleep = Tools.DeviateByPercent(maxSleep, mutationPower);
        instance.currentSleep = instance.maxSleep;
        instance.fatigueRate = Tools.DeviateByPercent(fatigueRate, mutationPower);

        instance.maxHealth = Tools.DeviateByPercent(maxHealth, mutationPower);
        instance.currentHealth = instance.maxHealth;
        instance.healRate = Tools.DeviateByPercent(healRate, mutationPower);
        instance.healAtNutritionPercentage = Tools.DeviateByPercent(healAtNutritionPercentage, mutationPower);

        instance.movement.moveSpeed = Tools.DeviateByPercent(instance.movement.moveSpeed, mutationPower, 0.01f);
        instance.movement.acceleration = Tools.DeviateByPercent(instance.movement.acceleration, mutationPower, 0.01f);
        instance.movement.randomTurnSpeed = Tools.DeviateByPercent(instance.movement.randomTurnSpeed, mutationPower, 1f);
        instance.movement.turnDeviation = Tools.DeviateByPercent(instance.movement.turnDeviation, mutationPower, 0f);
    }
}
