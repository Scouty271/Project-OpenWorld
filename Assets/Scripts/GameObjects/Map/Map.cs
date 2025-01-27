using UnityEngine;

public class Map : MonoBehaviour
{
    public MapTile[,] mapTiles;

    public MapTile getMapTile(Vector2 index)
    {
        return mapTiles[(int)index.x, (int)index.y];
    }
    public MapTile getMapTile(float indexX, float indexY)
    {
        return mapTiles[(int)indexX, (int)indexY];
    }
    public MapTile getMapTile(int indexX, int indexY)
    {
        return mapTiles[indexX, indexY];
    }
    public void setMapTile(MapTile mapTile, Vector2 index)
    {
        mapTiles[(int)index.x, (int)index.y] = mapTile;
    }
    public void setMapTile(MapTile mapTile, int indexX, int indexY)
    {
        mapTiles[indexX, indexY] = mapTile;
    }
    public void setMapTile(MapTile mapTile, float indexX, float indexY)
    {
        mapTiles[(int)indexX, (int)indexY] = mapTile;
    }
}
