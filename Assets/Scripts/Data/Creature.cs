using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Creature
{
    public string name;

    public float maxHunger;
    public float hungerRate;
    public float maxThirst;
    public float thirstRate;
    public bool needThirst;

    public float maxSleep;
    public float fatigueRate;
    public float sleepSpeed;

    //Movement details
        //Algorithm type, tile weights (do they swim/walk? mud/forest?),
        //default: speed, turn, acceleration...

    //default sensor bounds (i.e. sight)

    public Creature()
    {

    }
}
