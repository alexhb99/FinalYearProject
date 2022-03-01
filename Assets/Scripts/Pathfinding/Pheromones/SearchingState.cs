using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingState : PheromoneState
{
    FoodUnit foodUnit;

    public SearchingState(PheromoneStateMachine psm) : base(psm)
    {
    }

    public override void Enter(UnitMovement movement)
    {
        base.Enter(movement);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void DropPheromone()
    {
        base.DropPheromone();
    }

    public override void MoveAnt()
    {
        base.MoveAnt();
        movement.MoveAnt();

        if(foodUnit != null)
        {
            if(movement.diff.sqrMagnitude < EAT_DISTANCE)
            {
                EatFood();
            }
        }
    }

    private void EatFood()
    {
        //Add nutrition
        foodUnit.Pickup();
        movement.target = null;
    }

    public override void DetectTriggerCollision(Collider2D other)
    {
        base.DetectTriggerCollision(other);
        if (foodUnit == null)
        {
            foodUnit = other.gameObject.GetComponent<FoodUnit>();
            movement.target = other.gameObject.transform.position;
        }
    }
}
