using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheromoneUnit : MonoBehaviour
{
    public PheromoneStateMachine stateMachine;
    public SearchingState searching;
    public ReturningState returning;

    private MovementUnit movement;

    public bool drawGizmos;

    public void StartPheromone(AntColony antColony, PheromoneController pheromoneController)
    {
        movement = GetComponent<MovementUnit>();

        stateMachine = new PheromoneStateMachine();

        searching = new SearchingState(stateMachine);
        returning = new ReturningState(stateMachine);

        stateMachine.Initialize(searching, antColony, movement, pheromoneController);
    }

    private void Update()
    {
        stateMachine.currentState.MoveAnt();
        stateMachine.currentState.DropPheromone();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        stateMachine.currentState.DetectTriggerCollision(collision);
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, -transform.right * (stateMachine.currentState.halfSampleSize.x / 2f) + transform.up);
            Gizmos.DrawRay(transform.position, transform.right * (stateMachine.currentState.halfSampleSize.x / 2f) + transform.up);

            Vector3 pos = transform.position + transform.up - transform.right * stateMachine.currentState.halfSampleSize.x;
            Gizmos.color = Color.white;
            Gizmos.DrawLine(pos, pos + transform.right * 2);

            for(int i = 0; i < 3; i++)
            {
                Vector3 sampledPosition = pos + transform.right * i;
                sampledPosition = new Vector3(Mathf.Round(sampledPosition.x), Mathf.Round(sampledPosition.y), 0);
                Gizmos.DrawCube(sampledPosition, Vector3.one * 0.5f);
            }
        }
    }
}
