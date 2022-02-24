using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class TerrainDatabase : MonoBehaviour
{
    public TerrainType[] terrainTypes;

    public Dictionary<TileBase, TerrainType> tileBaseToTerrainType = new Dictionary<TileBase, TerrainType>();

    private void Start()
    {
        terrainTypes = terrainTypes.OrderBy(x => x.height).ToArray();

        foreach(TerrainType tt in terrainTypes)
        {
            if (!tileBaseToTerrainType.ContainsValue(tt))
            {
                tileBaseToTerrainType.Add(tt.tile, tt);
            }
        }
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
