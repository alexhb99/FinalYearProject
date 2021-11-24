using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class TerrainDatabase : MonoBehaviour
{
    public TerrainType[] terrainTypes;

    private void Start()
    {
        terrainTypes = terrainTypes.OrderBy(x => x.height).ToArray();
    }
}

[System.Serializable]
public class TerrainType
{
    public string name;
    public TileBase tile;
    [Range(0, 1)]
    public float walkSpeed;

    [Range(0, 1)]
    public float height;
}
