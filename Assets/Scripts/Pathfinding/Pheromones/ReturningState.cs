using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturningState : PheromoneState
{
    public float returnPheromoneAmount;

    public ReturningState(PheromoneStateMachine psm) : base(psm)
    {
    }

    public override void Enter(MovementUnit movement)
    {
        base.Enter(movement);
        returnPheromoneAmount = psm.pheromoneController.returnPheromoneCapacity;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void DropPheromone()
    {
        base.DropPheromone();

        Vector2 newRoundedPosition = new Vector2(Mathf.Round(movement.transform.position.x), Mathf.Round(movement.transform.position.y));

        if (newRoundedPosition != roundedPosition && returnPheromoneAmount > 0)
        {
            Pheromone currentPheromone = psm.pheromoneController.PheromoneFromPos(newRoundedPosition);
            if(currentPheromone != null)
            {
                returnPheromoneAmount -= currentPheromone.IncrementToFoodIntensity(psm.pheromoneController.maxIntensity);
                roundedPosition = newRoundedPosition;
            }
        }
    }

    public override void MoveAnt()
    {
        base.MoveAnt();

        Vector3 sense = SensePheromones() * 10f;
        //Debug.Log(movement.transform.position + " , " + sense + " , target: " + movement.target);
        movement.MoveAnt(sense);

        //If arrived at home...
        if (((Vector2)movement.transform.position - (Vector2)psm.antColony.transform.position).sqrMagnitude < INTERACT_DISTANCE)
        {
            movement.transform.localRotation *= Quaternion.Euler(0, 0, 180);
            movement.target = null;
            psm.ChangeState(new SearchingState(psm));
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

            for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    Pheromone pheromone = psm.pheromoneController.PheromoneFromPos(sampledPosition + new Vector3(x, y, 0));
                    if (pheromone != null)
                    {
                        sensorIntensities[i] += pheromone.toHomeIntensity;
                    }
                }
            }
        }


        //Left
        if (sensorIntensities[0] > Mathf.Max(sensorIntensities[1], sensorIntensities[2]))
        {
            //Debug.Log("LEFT : " + sensorIntensities[0] + " , " + sensorIntensities[1] + " , " + sensorIntensities[2]);
            return -movement.transform.right * halfSampleSize.x;
        }
        //Right
        else if (sensorIntensities[2] > sensorIntensities[1])
        {
            //Debug.Log("RIGHT : " + sensorIntensities[0] + " , " + sensorIntensities[1] + " , " + sensorIntensities[2]);
            return movement.transform.right * halfSampleSize.x;
        }
        //Centre
        else if (sensorIntensities[1] > sensorIntensities[2])
        {
            //Debug.Log("CENTRE : " + sensorIntensities[0] + " , " + sensorIntensities[1] + " , " + sensorIntensities[2]);
            return movement.transform.up;
        }
        else
        {
            //Debug.Log("NONE : " + sensorIntensities[0] + " , " + sensorIntensities[1] + " , " + sensorIntensities[2]);
            return Vector3.zero;
        }
    }

    public override void DetectTriggerCollision(Collider2D other)
    {
        base.DetectTriggerCollision(other);

        AntColony antColony = other.GetComponent<AntColony>();
        if(antColony != null && antColony == psm.antColony)
        {
            movement.target = other.gameObject.transform.position;
        }
    }
}
