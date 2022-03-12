using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodUnit : MonoBehaviour
{
    public float nutrition;
    [HideInInspector]
    public float originalNutrition;
    [HideInInspector]
    public float maxNutrition;
    private float growthRate;

    private List<MovementUnit> incomingAnts = new List<MovementUnit>();

    private TimeController timeController;


    public void Initialize(float nutrition, float maxNutrition, float growthRate)
    {
        timeController = GameObject.FindWithTag("GlobalLight").GetComponent<TimeController>();

        this.nutrition = nutrition;
        this.maxNutrition = maxNutrition;
        this.growthRate = growthRate;
        originalNutrition = nutrition;
        SetScaleFromNutrition();
    }

    private void Update()
    {
        Grow();
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

            float val = Mathf.Log(nutrition * 0.5f) * 2f;
            transform.localScale = Vector3.one + new Vector3(val, val, 0);
        }
    }

    private void Grow()
    {
        if (timeController != null && timeController.lightLevel > 0.01f && nutrition < maxNutrition)
        {
            float growth = Mathf.Min(maxNutrition - nutrition, Time.deltaTime * TimeControls.timeScale * timeController.lightLevel * growthRate);
            nutrition += growth;
            SetScaleFromNutrition();
        }
    }
}
