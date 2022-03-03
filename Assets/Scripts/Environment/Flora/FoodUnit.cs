using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodUnit : MonoBehaviour
{
    public float nutrition;
    private List<MovementUnit> incomingAnts = new List<MovementUnit>();

    public void AssignIncomingAnt(MovementUnit newAnt)
    {
        incomingAnts.Add(newAnt);
    }

    public void Pickup()
    {
        foreach(MovementUnit movement in incomingAnts)
        {
            movement.target = null;
        }
        Destroy(gameObject);
    }
}
