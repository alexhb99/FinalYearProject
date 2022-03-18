using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingState : PheromoneState
{
    FoodUnit foodUnit;

    public float searchPheromoneAmount;

    public SearchingState(PheromoneStateMachine psm) : base(psm)
    {
    }

    public override void Enter(MovementUnit movement)
    {
        base.Enter(movement);
        searchPheromoneAmount = psm.pheromoneController.searchPheromoneCapacity;
        psm.carriedNutrition = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void DropPheromone()
    {
        base.DropPheromone();

        Vector2 newRoundedPosition = new Vector2(Mathf.Round(movement.transform.position.x), Mathf.Round(movement.transform.position.y));

        if(newRoundedPosition != roundedPosition && searchPheromoneAmount > 0)
        {
            Pheromone currentPheromone = psm.pheromoneController.PheromoneFromPos(newRoundedPosition);
            if (currentPheromone != null)
            {
                searchPheromoneAmount -= currentPheromone.IncrementToHomeIntensity(psm.pheromoneController.maxIntensity);
                roundedPosition = newRoundedPosition;
            }
        }
    }

    public override void MoveAnt()
    {
        base.MoveAnt();

        movement.MoveAnt(SensePheromones());

        if(foodUnit != null)
        {
            if(movement.diff.sqrMagnitude < INTERACT_DISTANCE)
            {
                PickupFood();
                movement.transform.localRotation *= Quaternion.Euler(0, 0, 180);
                psm.ChangeState(new ReturningState(psm));
            }
        }
    }

    public override Vector3 SensePheromones()
    {
        base.SensePheromones();

        //Get the position of the leftmost sensor
        Vector3 pos = movement.transform.position + movement.transform.up - movement.transform.right * halfSampleSize.x;

        //Stores intensity measured in each of the 3 sensors
        float[] sensorIntensities = new float[3];

        for (int i = 0; i < 3; i++)
        {
            //Place this sensor on the appropriate tile
            Vector3 sampledPosition = pos + movement.transform.right * i;
            sampledPosition = new Vector3(Mathf.Round(sampledPosition.x), Mathf.Round(sampledPosition.y), 0);

            //In a 3x3 area around this sensor, add up the intensities of pheromones
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Pheromone pheromone = psm.pheromoneController.PheromoneFromPos(sampledPosition + new Vector3(x, y, 0));
                    if(pheromone != null)
                    {
                        sensorIntensities[i] += pheromone.toFoodIntensity;
                    }
                }
            }
        }

        //Left sensor
        if (sensorIntensities[0] > Mathf.Max(sensorIntensities[1], sensorIntensities[2]))
        {
            return movement.transform.up - movement.transform.right * halfSampleSize.x;
        }
        //Right sensor
        else if (sensorIntensities[2] > sensorIntensities[1])
        {
            return movement.transform.up + movement.transform.right * halfSampleSize.x;
        }
        //Centre sensor
        else if (sensorIntensities[1] > sensorIntensities[2])
        {
            return movement.transform.up;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private void PickupFood()
    {
        //Add nutrition
        psm.carriedNutrition = foodUnit.Pickup(1f);
        movement.target = null;
    }

    public override void DetectTriggerCollision(Collider2D other)
    {
        base.DetectTriggerCollision(other);

        //If currently not chasing food
        if (foodUnit == null)
        {
            foodUnit = other.gameObject.GetComponent<FoodUnit>();
            //Check the other object has a FoodUnit component attached to it
            if(foodUnit != null)
            {
                //Set creature's target as this object
                movement.target = other.gameObject.transform.position;

                //Assign this creature to the food
                foodUnit.AssignIncomingAnt(movement);
            }
        }
    }
}
