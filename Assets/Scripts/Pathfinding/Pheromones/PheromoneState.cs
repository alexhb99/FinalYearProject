using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PheromoneState
{
    PheromoneStateMachine psm;

    public PheromoneState(PheromoneStateMachine psm)
    {
        this.psm = psm;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }

    public virtual void DropPheromone() { }
}
