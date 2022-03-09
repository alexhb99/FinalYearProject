using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromone
{
    private const float MinIntensity = 0.01f;

    public Vector3 pos;
    public float toFoodIntensity;
    public float toHomeIntensity;

    public Pheromone(Vector3 pos)
    {
        this.pos = pos;
        toFoodIntensity = 0;
        toHomeIntensity = 0;
    }

    public float IncrementToFoodIntensity(float maxIntensity)
    {
        float originalIntensity = toFoodIntensity;
        toFoodIntensity++;
        if (toFoodIntensity > maxIntensity) toFoodIntensity = maxIntensity;
        return toFoodIntensity - originalIntensity;
    }
    public float IncrementToHomeIntensity(float maxIntensity)
    {
        float originalIntensity = toHomeIntensity;
        toHomeIntensity++;
        if (toHomeIntensity > maxIntensity) toHomeIntensity = maxIntensity;
        return toHomeIntensity - originalIntensity;
    }

    public void ReduceIntensities(float dissipateSpeed)
    {
        toFoodIntensity -= Time.deltaTime * dissipateSpeed; //Mathf.Lerp(toFoodIntensity, 0, Time.deltaTime * DissipateSpeed);
        toHomeIntensity -= Time.deltaTime * dissipateSpeed; //Mathf.Lerp(toHomeIntensity, 0, Time.deltaTime * DissipateSpeed);

        if (toFoodIntensity < MinIntensity) toFoodIntensity = 0;
        if (toHomeIntensity < MinIntensity) toHomeIntensity = 0;
    }
}
