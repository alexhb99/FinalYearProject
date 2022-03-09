using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class TerrainDatabase : MonoBehaviour
{
    public TerrainArchetype[] terrainArchetypes;
    public TerrainArchetype unwalkableTerrain;

    public Dictionary<TileBase, TerrainArchetype> tileBaseToTerrainArchetype = new Dictionary<TileBase, TerrainArchetype>();

    private void Start()
    {
        unwalkableTerrain = new TerrainArchetype("Unwalkable", Resources.Load<TileBase>("Tiles/UnwalkableTile"), 0, -1);
        tileBaseToTerrainArchetype.Add(unwalkableTerrain.tile, unwalkableTerrain);

        terrainArchetypes = terrainArchetypes.OrderBy(x => x.height).ToArray();

        foreach(TerrainArchetype ta in terrainArchetypes)
        {
            if (!tileBaseToTerrainArchetype.ContainsValue(ta))
            {
                tileBaseToTerrainArchetype.Add(ta.tile, ta);
            }
        }
    }
}

[System.Serializable]
public class TerrainArchetype
{
    public string name;
    public TileBase tile;
    [Range(0, 1)]
    public float walkSpeed;

    [Range(0, 1)]
    public float height;

    public TerrainArchetype(string name, TileBase tile, float walkSpeed, float height)
    {
        this.name = name;
        this.tile = tile;
        this.walkSpeed = walkSpeed;
        this.height = height;
    }
}
