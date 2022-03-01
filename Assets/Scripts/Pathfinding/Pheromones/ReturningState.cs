using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturningState : PheromoneState
{
    public ReturningState(PheromoneStateMachine psm) : base(psm)
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
}
