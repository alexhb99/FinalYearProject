using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodUnit : MonoBehaviour
{
    public float nutrition;
    private List<MovementUnit> incomingAnts = new List<MovementUnit>();

    public void Initialize(float nutrition)
    {
        this.nutrition = nutrition;
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
            transform.localScale = Vector3.one + new Vector3(nutrition / 10f - 1, nutrition / 10f - 1, 0);
        }
    }
}
