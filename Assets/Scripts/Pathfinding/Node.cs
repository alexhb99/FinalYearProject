using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2 pos;

    public bool walkable;

    public Node(Vector2 pos, bool walkable)
    {
        this.pos = pos;
        this.walkable = walkable;
    }

    public Node()
    {
        pos = Vector2.zero;
        walkable = false;
    }
}
