using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CreatureDatabase", order = 1)]
public class CreatureDatabase : ScriptableObject
{
    public List<Creature> creatures = new List<Creature>();

    public Creature CreatureFromName(string name)
    {
        foreach(Creature c in creatures)
        {
            if (c.name == name)
            {
                return c;
            }
        }
        Debug.LogError("Can't find creature " + name);
        return new Creature();
    }
}
