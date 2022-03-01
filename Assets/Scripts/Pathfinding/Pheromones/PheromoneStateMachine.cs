using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheromoneStateMachine
{
    public PheromoneState currentState;
    public UnitMovement movement;

    public void Initialize(PheromoneState startState, UnitMovement movement)
    {
        currentState = startState;
        this.movement = movement;
        startState.Enter(movement);
    }

    public void ChangeState(PheromoneState newState)
    {
        currentState.Exit();
        currentState = newState;
        newState.Enter(movement);
    }
}
