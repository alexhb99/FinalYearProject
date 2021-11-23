using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2Int pos;
    public Vector3 worldPosition;
    public List<Node> neighbours;
    public bool walkable;

    public Node parent;
    public int gCost;
    public int hCost;

    public Node(Vector2Int pos, Vector3 worldPosition, bool walkable)
    {
        this.pos = pos;
        this.worldPosition = worldPosition;
        this.parent = null;
        this.walkable = walkable;
    }

    public Node()
    {
        pos = Vector2Int.zero;
        walkable = false;
    }
}
