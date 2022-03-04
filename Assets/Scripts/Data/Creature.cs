using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Creature
{
    public string name;

    public float maxHunger;
    public float maxSleep;

    //Movement details
        //Algorithm type, tile weights (do they swim/walk? mud/forest?),
        //default: speed, turn, acceleration...

    //default sensor bounds (i.e. sight)

    public Creature()
    {

    }
}
