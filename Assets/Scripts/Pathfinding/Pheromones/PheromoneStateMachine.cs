using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheromoneStateMachine
{
    public PheromoneState currentState;
    public MovementUnit movement;
    public PheromoneController pheromoneController;
    public AntColony antColony;
    public CreatureInstance creatureInstance;

    public float carriedNutrition;

    public void Initialize(PheromoneState startState, AntColony antColony, MovementUnit movement, PheromoneController pheromoneController, CreatureInstance creatureInstance)
    {
        currentState = startState;
        this.antColony = antColony;
        this.movement = movement;
        this.pheromoneController = antColony.GetComponent<PheromoneController>();
        this.creatureInstance = creatureInstance;

        startState.Enter(movement);
    }

    public void ChangeState(PheromoneState newState)
    {
        currentState.Exit();
        currentState = newState;
        newState.Enter(movement);
    }
}
