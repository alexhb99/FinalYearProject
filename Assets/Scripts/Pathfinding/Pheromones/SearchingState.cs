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
            searchPheromoneAmount -= currentPheromone.IncrementToHomeIntensity(psm.pheromoneController.maxIntensity);
            roundedPosition = newRoundedPosition;
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

        Vector3 pos = movement.transform.position + movement.transform.up - movement.transform.right * halfSampleSize.x;

        float[] sensorIntensities = new float[3];

        for (int i = 0; i < 3; i++)
        {
            Vector3 sampledPosition = pos + movement.transform.right * i;
            sampledPosition = new Vector3(Mathf.Round(sampledPosition.x), Mathf.Round(sampledPosition.y), 0);

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    sensorIntensities[i] += psm.pheromoneController.PheromoneFromPos(sampledPosition + new Vector3(x, y, 0)).toFoodIntensity;
                }
            }
        }

        //Left
        if (sensorIntensities[0] > Mathf.Max(sensorIntensities[1], sensorIntensities[2]))
        {
            return movement.transform.up - movement.transform.right * halfSampleSize.x;
        }
        //Right
        else if (sensorIntensities[2] > sensorIntensities[1])
        {
            return movement.transform.up + movement.transform.right * halfSampleSize.x;
        }
        //Centre
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
        float pickedUpAmount = foodUnit.Pickup(1f);
        movement.target = null;
    }

    public override void DetectTriggerCollision(Collider2D other)
    {
        base.DetectTriggerCollision(other);
        if (foodUnit == null)
        {
            foodUnit = other.gameObject.GetComponent<FoodUnit>();
            if(foodUnit != null)
            {
                movement.target = other.gameObject.transform.position;
                foodUnit.AssignIncomingAnt(movement);
            }
        }
    }
}
