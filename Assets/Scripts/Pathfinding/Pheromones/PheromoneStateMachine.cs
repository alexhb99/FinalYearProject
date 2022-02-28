using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheromoneStateMachine
{
    public PheromoneState currentState;

    public void Initialize(PheromoneState startState)
    {
        currentState = startState;
        startState.Enter();
    }

    public void ChangeState(PheromoneState newState)
    {
        currentState.Exit();
        currentState = newState;
        newState.Enter();
    }
}
