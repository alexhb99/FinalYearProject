using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PheromoneState
{
    public const float INTERACT_DISTANCE = 0.4f;
    public const int SampleSizeX = 1;
    public const int SampleSizeY = 1;

    public Vector3Int sampleSize;
    public Vector3Int halfSampleSize;
    public float offsetDistance;

    public PheromoneStateMachine psm;
    public MovementUnit movement;

    public Vector2 roundedPosition;

    public PheromoneState(PheromoneStateMachine psm)
    {
        this.psm = psm;
        sampleSize = new Vector3Int(SampleSizeX * 3, SampleSizeY, 1); // multiply x by 3, as there are 3 samples placed next to each other
        halfSampleSize = new Vector3Int(sampleSize.x / 2, sampleSize.y / 2, 0);
        offsetDistance = SampleSizeY;
    }

    public virtual void Enter(MovementUnit movement)
    {
        this.movement = movement;
        movement.target = null;
        roundedPosition = new Vector2(Mathf.Round(movement.transform.position.x), Mathf.Round(movement.transform.position.y));
    }
    public virtual void Exit() { }
    public virtual void MoveAnt() { }

    public virtual void DropPheromone() { }
    public virtual Vector3 SensePheromones() { return Vector3.zero; }
    public virtual void DetectTriggerCollision(Collider2D other) { }
}
