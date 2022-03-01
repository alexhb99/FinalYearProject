using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PheromoneState
{
    public const float EAT_DISTANCE = 0.4f;

    public PheromoneStateMachine psm;
    public UnitMovement movement;

    public PheromoneState(PheromoneStateMachine psm)
    {
        this.psm = psm;
    }

    public virtual void Enter(UnitMovement movement)
    {
        this.movement = movement;
    }
    public virtual void Exit() { }
    public virtual void MoveAnt() { }

    public virtual void DropPheromone() { }
    public virtual void DetectTriggerCollision(Collider2D other) { }
}
