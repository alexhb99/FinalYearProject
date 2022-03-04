using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2Int pos;
    public Vector3 worldPosition;
    public List<Node> neighbours;
    public TerrainArchetype terrainArchetype;
    public float walkSpeed;
    public bool walkable => walkSpeed > 0;

    public Node parent;
    public int gCost;
    public int hCost;

    public Node(Vector2Int pos, Vector3 worldPosition, TerrainArchetype terrainArchetype)
    {
        this.pos = pos;
        this.worldPosition = worldPosition;
        this.parent = null;
        this.terrainArchetype = terrainArchetype;
    }

    public Node()
    {
        pos = Vector2Int.zero;
        walkSpeed = 0;
    }
}
