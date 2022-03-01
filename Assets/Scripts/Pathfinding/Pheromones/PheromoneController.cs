using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheromoneController : MonoBehaviour
{
    public PheromoneStateMachine stateMachine;
    public SearchingState searching;
    public ReturningState returning;

    private UnitMovement movement;

    private void Start()
    {
        movement = GetComponent<UnitMovement>();

        stateMachine = new PheromoneStateMachine();

        searching = new SearchingState(stateMachine);
        returning = new ReturningState(stateMachine);

        stateMachine.Initialize(searching, movement);
    }

    private void Update()
    {
        stateMachine.currentState.MoveAnt();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        stateMachine.currentState.DetectTriggerCollision(collision);
    }
}
