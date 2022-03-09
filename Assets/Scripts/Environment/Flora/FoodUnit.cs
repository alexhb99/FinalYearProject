using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodUnit : MonoBehaviour
{
    private float originalNutrition;
    Vector3 originalScale;
    public float nutrition;
    private List<MovementUnit> incomingAnts = new List<MovementUnit>();

    public void Initialize(float nutrition)
    {
        this.nutrition = nutrition;
        originalNutrition = nutrition;
        originalScale = Vector3.one + new Vector3(Mathf.Min(10f, originalNutrition / 10f - 1), Mathf.Min(10f, originalNutrition / 10f - 1), 0);
        SetScaleFromNutrition();
    }

    public void AssignIncomingAnt(MovementUnit newAnt)
    {
        incomingAnts.Add(newAnt);
    }

    public float Pickup(float amount)
    {
        nutrition -= amount;

        if(nutrition < 0)
        {
            foreach(MovementUnit movement in incomingAnts)
            {
                movement.target = null;
            }

            Destroy(gameObject);
            amount += nutrition;
        }
        SetScaleFromNutrition();

        return amount;
    }

    private void SetScaleFromNutrition()
    {
        if (nutrition < 1)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = Vector3.one + new Vector3(originalScale.x * nutrition / originalNutrition, originalScale.y * nutrition / originalNutrition, 1);
        }
    }
}
