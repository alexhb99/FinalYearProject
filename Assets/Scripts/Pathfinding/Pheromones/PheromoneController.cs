using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheromoneController : MonoBehaviour
{
    public PheromoneStateMachine stateMachine;
    public SearchingState searching;
    public ReturningState returning;

    private void Start()
    {
        stateMachine = new PheromoneStateMachine();

        searching = new SearchingState(stateMachine);
        returning = new ReturningState(stateMachine);

        stateMachine.Initialize(searching);
    }

    
}
