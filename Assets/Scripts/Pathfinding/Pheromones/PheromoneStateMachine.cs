using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheromoneStateMachine
{
    public PheromoneState currentState;
    public MovementUnit movement;
    public PheromoneController pheromoneController;
    public AntColony antColony;

    public void Initialize(PheromoneState startState, AntColony antColony, MovementUnit movement, PheromoneController pheromoneController)
    {
        currentState = startState;
        this.antColony = antColony;
        this.movement = movement;
        this.pheromoneController = antColony.GetComponent<PheromoneController>();

        startState.Enter(movement);
    }

    public void ChangeState(PheromoneState newState)
    {
        currentState.Exit();
        currentState = newState;
        newState.Enter(movement);
    }
}
