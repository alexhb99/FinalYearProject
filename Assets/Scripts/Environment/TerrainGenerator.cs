using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{
    private Tilemap tilemap;
    private GridController gridController;
    private TerrainDatabase terrainDatabase;

    public Vector2Int size;
    public TerrainArchetype[,] baseTerrain;

    public int seed;
    public bool randomSeed;
    [Min(0.00001f)]
    public float scale;
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    public Vector2 octaveOffset;
    public Vector2 offset;

    private float[,] heightmap;

    [HideInInspector]
    public Vector3 size3;
    [HideInInspector]
    public Bounds terrainBounds;

    private void Start()
    {
        tilemap = GameObject.FindWithTag("TerrainTilemap").GetComponent<Tilemap>();
        gridController = GameObject.FindWithTag("Pathfinding").GetComponent<GridController>();
        terrainDatabase = GetComponent<TerrainDatabase>();
    }

    public List<TerrainArchetype> GetTerrainInBounds(BoundsInt bounds)
    {
        TileBase[] tileArray = tilemap.GetTilesBlock(bounds);
        List<TileBase> accTiles = new List<TileBase>();
        List<TerrainArchetype> tts = new List<TerrainArchetype>();

        foreach (TileBase t in tileArray)
        {
            accTiles.Add(null);
            if (t != null)
            {
                tts.Add(terrainDatabase.tileBaseToTerrainArchetype[t]);
            }
            else
            {
                tts.Add(null);
            }
        }

        //tilemap.SetTilesBlock(bounds, accTiles.ToArray());
        return tts;
    }

    public TerrainArchetype GetTileAtPos(Vector3Int pos)
    {
        TileBase tileBase = tilemap.GetTile(pos);
        if(tileBase != null)
        {
            return terrainDatabase.tileBaseToTerrainArchetype[tileBase];
        }
        return null;
    }

    public void GenerateTerrain()
    {
        GenerateHeightmap();

        Vector3Int[] positions = new Vector3Int[size.x * size.y];
        TileBase[] tiles = new TileBase[size.x * size.y];
        baseTerrain = new TerrainArchetype[size.x, size.y];

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                positions[x * size.y + y] = new Vector3Int(x, y, 0);

                foreach (TerrainArchetype terrainArchetype in terrainDatabase.terrainArchetypes)
                {
                    if(terrainArchetype.height > heightmap[x, y])
                    {
                        break;
                    }
                    tiles[x * size.y + y] = terrainArchetype.tile;
                    baseTerrain[x, y] = terrainArchetype;
                }                
            }
        }
        tilemap.SetTiles(positions, tiles);

        gridController.CreateGrid(size);

        size3 = new Vector3(size.x, size.y, 0);
        terrainBounds = new Bounds(size3 / 2f + tilemap.transform.position, size3);
    }

    private void GenerateHeightmap()
    {
        heightmap = new float[size.x, size.y];

        if (randomSeed)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }

        System.Random prng = new System.Random(seed);
        Vector2[] octaveoffsets = new Vector2[octaves];

        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-65536, 65536) + octaveOffset.x;
            float offsetY = prng.Next(-65536, 65536) + octaveOffset.y;
            octaveoffsets[i] = new Vector2(offsetX, offsetY);

            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                amplitude = 1;
                frequency = 1;
                float height = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x + offset.x) / scale * frequency + octaveoffsets[i].x;
                    float sampleY = (y + offset.y) / scale * frequency + octaveoffsets[i].y;
                    float perlinVal = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                    height += perlinVal * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                heightmap[x, y] = height;
            }
        }
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                float normalizedHeight = (heightmap[x, y] + 1) / (2f * maxPossibleHeight / 2f);
                heightmap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
            }
        }
    }
}
