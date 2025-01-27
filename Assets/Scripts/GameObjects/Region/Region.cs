using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    public int size;
    public Vector2Int worldPositionRegion;
    public Vector2Int mapPosition;

    public MapTile.TileType tileType;

    public EntityHolder entityHolder;

    public RegionTile[,] regionTiles;

    public Block[,] blocks;

    public Tree[,] trees;

    public Flower[,] flowers;

    public List<Entity> entities = new List<Entity>();

    public Vector2Int relativePositionToMiddleChunk;

    public List<Vector2Int> informationInterfaceMapPositions = new List<Vector2Int>();

    private void Awake()
    {
        regionTiles = new RegionTile[size, size];
        blocks = new Block[size, size];
        trees = new Tree[size, size];
        flowers = new Flower[size, size];
    }
    private void Start()
    {
        GetComponent<BoxCollider>().size = new Vector3(size, size);
        GetComponent<BoxCollider>().center = new Vector3(size / 2 - 0.5f, size / 2 - 0.5f, 0.01f);
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
    public void Activate()
    {
        gameObject.SetActive(true);
    }
    public int getSize()
    {
        return size;
    }
    public RegionTile getRegionTile(int x, int y)
    {
        return regionTiles[x, y];
    }
    public RegionTile getRegionTile(Vector2 index)
    {
        return regionTiles[(int)index.x, (int)index.y];
    }
    public void setRegionTile(RegionTile tile, int x, int y)
    {
        regionTiles[x, y] = tile;
    }
    public void setRegionTile(RegionTile tile, Vector2 index)
    {
        regionTiles[(int)index.x, (int)index.y] = tile;
    }
    public void fillNeighborList()
    {
        if (relativePositionToMiddleChunk.x == 2)
            informationInterfaceMapPositions.Add(new Vector2Int(mapPosition.x + 1, mapPosition.y));

        if (relativePositionToMiddleChunk.x == -2)
            informationInterfaceMapPositions.Add(new Vector2Int(mapPosition.x - 1, mapPosition.y));

        if (relativePositionToMiddleChunk.y == 2)
            informationInterfaceMapPositions.Add(new Vector2Int(mapPosition.x, mapPosition.y + 1));

        if (relativePositionToMiddleChunk.y == -2)
            informationInterfaceMapPositions.Add(new Vector2Int(mapPosition.x, mapPosition.y - 1));
    }
}