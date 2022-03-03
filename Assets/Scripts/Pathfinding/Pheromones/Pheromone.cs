using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromone
{
    private const float DissipateSpeed = 0.25f;
    private const float MinIntensity = 0.01f;
    private const float MaxIntensity = 10f;

    public Vector3 pos;
    public float toFoodIntensity;
    public float toHomeIntensity;

    public Pheromone(Vector3 pos)
    {
        this.pos = pos;
        toFoodIntensity = 0;
        toHomeIntensity = 0;
    }

    public void IncrementToFoodIntensity()
    {
        toFoodIntensity++;
        if (toFoodIntensity > MaxIntensity) toFoodIntensity = MaxIntensity;
    }
    public void IncrementToHomeIntensity()
    {
        toHomeIntensity++;
        if (toHomeIntensity > MaxIntensity) toHomeIntensity = MaxIntensity;
    }

    public void ReduceIntensities()
    {
        toFoodIntensity = Mathf.Lerp(toFoodIntensity, 0, Time.deltaTime * DissipateSpeed);
        toHomeIntensity = Mathf.Lerp(toHomeIntensity, 0, Time.deltaTime * DissipateSpeed);

        if (toFoodIntensity < MinIntensity) toFoodIntensity = 0;
        if (toHomeIntensity < MinIntensity) toHomeIntensity = 0;
    }
}
