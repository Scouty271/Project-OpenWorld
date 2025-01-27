using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Map map;
    public MapTile mapTile;

    public TextureHolder textureHolder;

    public WorldController world;

    private int worldSize;

    private Map instantiatedMap;

    private float mapShapeNoiseRange = 0.8f;
    private float mapMountanNoiseRange = 0.7f;

    private void Awake()
    {
        worldSize = world.worldSize;

        instantiatedMap = FindObjectOfType<Map>();

        map.mapTiles = new MapTile[world.worldSize, world.worldSize];

    }

    private void Start()
    {
        //instantiatedMap = Instantiate(map, GetComponentInParent<MapController>().transform);
        //instantiatedMap.gameObject.SetActive(true);
        instantiatedMap.name = "Map";
        world.map = instantiatedMap;
    }

    public void createMapTiles()
    {
        //instantiatedMap.mapTiles = new MapTile[world.worldSize, world.worldSize];

        for (int ix = 0; ix < worldSize; ix++)
        {
            for (int iy = 0; iy < worldSize; iy++)
            {
                setMapShapeNoiseValue(worldSize, ix, iy);
                setMountanNoiseValue(worldSize, ix, iy);

                if (mapTile.mapShapeNoiseValue > mapShapeNoiseRange)
                    if (mapTile.mountanNoiseValue > mapMountanNoiseRange)
                        setMountanTile();
                    else
                        setLandTile();
                else
                    setWaterTile();

                map.mapTiles[ix, iy] = Instantiate(mapTile, new Vector3(ix, iy, 0), Quaternion.identity, instantiatedMap.transform);

                map.mapTiles[ix, iy].name = "MapTile: (" + ix + ", " + iy + ")";
            }
        }
    }

    private void setMapShapeNoiseValue(int worldSize, int ix, int iy)
    {
        var mapShapeFrequenzy = 13f;
        var mapShapeAmplitude = 0.7f;

        var kontinentFactor = (1f / worldSize) * 5f;

        mapTile.mapShapeNoiseValue = Mathf.PerlinNoise((float)ix / (float)worldSize * mapShapeFrequenzy + world.seed, (float)iy / (float)worldSize * mapShapeFrequenzy + world.seed) * mapShapeAmplitude;

        if (ix <= worldSize / 2)
            mapTile.mapShapeNoiseValue = (ix * kontinentFactor) * mapTile.mapShapeNoiseValue;

        if (ix > worldSize / 2)
            mapTile.mapShapeNoiseValue = kontinentFactor * (-ix + worldSize) * mapTile.mapShapeNoiseValue;

        if (iy <= worldSize / 2)
            mapTile.mapShapeNoiseValue = (iy * kontinentFactor) * mapTile.mapShapeNoiseValue;

        if (iy > worldSize / 2)
            mapTile.mapShapeNoiseValue = kontinentFactor * (-iy + worldSize) * mapTile.mapShapeNoiseValue;

    }
    private void setMountanNoiseValue(int worldSize, int ix, int iy)
    {
        var mountanFrequenzy = 20f;
        var mountanAmplitude = 1f;

        var mountanFactor = (1f / worldSize) * 5f;

        mapTile.mountanNoiseValue = Mathf.PerlinNoise((float)ix / (float)worldSize * mountanFrequenzy + world.seed, (float)iy / (float)worldSize * mountanFrequenzy + world.seed) * mountanAmplitude;
    }
    private void setLandTile()
    {
        mapTile.setSprite(textureHolder.mapLand1);
        mapTile.setMapTileType(MapTile.TileType.land);
    }
    private void setWaterTile()
    {
        mapTile.setSprite(textureHolder.mapWater1);
        mapTile.setMapTileType(MapTile.TileType.water);
    }
    private void setMountanTile()
    {
        mapTile.setSprite(textureHolder.mapMountan1);
        mapTile.setMapTileType(MapTile.TileType.mountan);
    }
}
