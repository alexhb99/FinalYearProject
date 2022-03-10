using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureInstance : MonoBehaviour
{
    public Creature archetype;

    //Nutrition (hunger)
    public float maxHunger;
    public float currentHunger;
    public float hungerRate;

    //Sleep
    public float maxSleep;
    public float currentSleep;
    public float fatigueRate;

    //Health
    public float maxHealth;
    public float currentHealth;
    public float healRate;
    public float healAtNutritionPercentage;     //If lower than this %, don't heal on update

    public MovementUnit movement;

    public void Initialize()
    {
        movement = GetComponent<MovementUnit>();
    }

    public float Eat(float amount)
    {
        currentHunger += amount;
        float remainder = Mathf.Max(0, currentHunger - maxHunger);
        currentHunger = Mathf.Min(maxHunger, currentHunger);
        return remainder;
    }

    public bool TakeDamage(float amount)
    {
        currentHealth -= amount;
        return currentHealth < 0;   //Return true if should be dead
    }

    public void HealTick()
    {
        if(currentHealth < maxHealth && currentHunger / maxHunger > healAtNutritionPercentage)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + healRate * Time.deltaTime * TimeControls.timeScale);
        }
    }

    public bool HungerTick()
    {
        float energyUse = movement.currentSpeed;
        currentHunger -= energyUse * hungerRate * Time.deltaTime * TimeControls.timeScale;

        float damage = 0 - currentHunger;
        currentHunger = Mathf.Max(0, currentHunger);
        return damage > 0 ? TakeDamage(damage) : false;
    }
}
