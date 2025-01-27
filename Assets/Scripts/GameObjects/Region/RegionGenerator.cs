using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

public class RegionGenerator : MonoBehaviour
{
    public WorldController world;
    public Region region;
    public Map map;

    public TextureHolder textureHolder;

    public TerrainHolder terrainHolder;
    public MaterialHolder materialHolder;
    public EntityHolder entityHolder;

    private int seed;

    private RegionTile regionTile;

    private void Start()
    {
        regionTile = terrainHolder.regionTile;
    }

    public Region Create(Vector2Int worldPositionRegion, RegionTile tile, GameObject parent)
    {
        region.worldPositionRegion = worldPositionRegion;
        region.mapPosition = new Vector2Int(worldPositionRegion.x / region.size, worldPositionRegion.y / region.size);

        //Instanziere Region



        var instantiatedRegion = Instantiate(region, new Vector3(worldPositionRegion.x, worldPositionRegion.y), Quaternion.identity, parent.transform);

        instantiatedRegion.name = "Region: Map(" + instantiatedRegion.mapPosition.x + ", " + instantiatedRegion.mapPosition.y + ")";

        instantiatedRegion.tileType = map.getMapTile(instantiatedRegion.mapPosition).getMapTileType();

        //Instanziere Vegetation

        seed = Random.Range(0, 10000);

        region.regionTiles = new RegionTile[region.size, region.size];
        region.trees = new Tree[region.size, region.size];
        for (int ix = 0; ix < region.getSize(); ix++)
        {
            for (int iy = 0; iy < region.getSize(); iy++)
            {
                tile.withObject = false;

                setRegionTileStandart(tile);

                setTileCrossing(tile, ix, iy, MapTile.TileType.mountan, 2f, 0.1f, 1f, seed);
                setTileCrossing(tile, ix, iy, MapTile.TileType.water, 2f, 0.1f, 1f, seed);

                InstantiateRegionTile(ix, iy, worldPositionRegion, instantiatedRegion);

                //InstantiateTree(tile, ix, iy, seed, worldPositionRegion, instantiatedRegion);
                //InstantiateFlower(tile, ix, iy, seed, worldPositionRegion, instantiatedRegion);
            }
        }

        //Instanziere Entities

        for (int i = 0; i < 2; i++)
        {
            var valueX = Random.Range(0, instantiatedRegion.size);
            var valueY = Random.Range(0, instantiatedRegion.size);

            if (instantiatedRegion.regionTiles[valueX, valueY].tileType == RegionTile.regionTileType.grass && instantiatedRegion.regionTiles[valueX, valueY].withObject == false)
            {
                instantiatedRegion.entities.Add(Instantiate(entityHolder.deer, new Vector3(valueX + worldPositionRegion.x, valueY + worldPositionRegion.y, -0.1f), Quaternion.identity, instantiatedRegion.transform));
            }
        }

        return instantiatedRegion;
    }
    ///
    private void setRegionTileStandart(RegionTile tile)
    {
        if (map.getMapTile(region.mapPosition).getMapTileType() == MapTile.TileType.water)
        {
            setWaterTile(tile);
        }
        else if (map.getMapTile(region.mapPosition).getMapTileType() == MapTile.TileType.land)
        {
            setGrassTile(tile);
        }
        else if (map.getMapTile(region.mapPosition).getMapTileType() == MapTile.TileType.mountan)
        {
            setMountanTile(tile);
        }
    }
    private void setWaterTile(RegionTile tile)
    {
        tile.tileType = RegionTile.regionTileType.water;

        var rand = Random.Range(0, 5);

        if (rand == 0)
            tile.setMaterial(materialHolder.water1);
        if (rand == 1)
            tile.setMaterial(materialHolder.water2);
        if (rand == 2)
            tile.setMaterial(materialHolder.water3);
        if (rand == 3)
            tile.setMaterial(materialHolder.water4);
        if (rand == 4)
            tile.setMaterial(materialHolder.water5);
    }
    private void setGrassTile(RegionTile tile)
    {
        tile.tileType = RegionTile.regionTileType.grass;
        var rand = Random.Range(0, 3);
        if (rand == 0)
            tile.setMaterial(materialHolder.grass1);
        if (rand == 1)
            tile.setMaterial(materialHolder.grass2);
        if (rand == 2)
            tile.setMaterial(materialHolder.grass3);
    }
    private void setMountanTile(RegionTile tile)
    {
        tile.tileType = RegionTile.regionTileType.mountan;
        tile.setMaterial(materialHolder.mountan1);
    }

    private void setTileCrossing(RegionTile _tile, int _ix, int _iy, MapTile.TileType _type, float _frequenzy, float _amplitude, float _noiseRange, int _seed)
    {
        var noiseValueX = Mathf.PerlinNoise((float)_ix / ((float)region.getSize() * _frequenzy) + _seed, (float)_iy / ((float)region.getSize() * _frequenzy) + _seed) * _amplitude;
        var noiseValueY = Mathf.PerlinNoise((float)_ix / ((float)region.getSize() * _frequenzy) + _seed, (float)_iy / ((float)region.getSize() * _frequenzy) + _seed) * _amplitude;

        ///////

        //Kanten
        if (map.mapTiles[region.mapPosition.x + 1, region.mapPosition.y + 0].getMapTileType() == _type)
        {
            if (_ix > region.size / 2) // Start at 1/2 to set the Tiles
            {
                noiseValueX = _ix * noiseValueX; // The higher ix the lower is the noise Value

                if (noiseValueX > _noiseRange)
                {
                    if (_type == MapTile.TileType.water)
                        setWaterTile(_tile);
                    if (_type == MapTile.TileType.mountan)
                        setMountanTile(_tile);
                }
            }
        }

        if (map.getMapTile(region.mapPosition.x + 0, region.mapPosition.y + 1).getMapTileType() == _type)
        {
            if (_iy > region.size / 2) // Start at 1/2 to set the Tiles
            {
                noiseValueY = _iy * noiseValueY;

                if (noiseValueY > _noiseRange)
                {
                    if (_type == MapTile.TileType.water)
                        setWaterTile(_tile);
                    if (_type == MapTile.TileType.mountan)
                        setMountanTile(_tile);
                }
            }
        }

        if (map.getMapTile(region.mapPosition.x - 1, region.mapPosition.y + 0).getMapTileType() == _type)
        {
            if (_ix < region.size / 2) // Start at 1/2 to set the Tiles
            {
                noiseValueX = (region.size - _ix) * noiseValueX;

                if (noiseValueX > _noiseRange)
                {
                    if (_type == MapTile.TileType.water)
                        setWaterTile(_tile);
                    if (_type == MapTile.TileType.mountan)
                        setMountanTile(_tile);
                }
            }
        }

        if (map.getMapTile(region.mapPosition.x + 0, region.mapPosition.y - 1).getMapTileType() == _type)
        {
            if (_iy < region.size / 2) // Start at 1/2 to set the Tiles
            {
                noiseValueY = (region.size - _iy) * noiseValueY;

                if (noiseValueY > _noiseRange)
                {
                    if (_type == MapTile.TileType.water)
                        setWaterTile(_tile);
                    if (_type == MapTile.TileType.mountan)
                        setMountanTile(_tile);
                }
            }
        }

        ////////////
        ///
        var edgeFactor = 10f;

        //Ecken
        if (map.getMapTile(region.mapPosition.x + 1, region.mapPosition.y + 1).getMapTileType() == _type)
        {
            if (_ix > region.size / 1.5f && _iy > region.size / edgeFactor) // 1.5f because then it will start to set the Tiles at the 3/4 of the Region (Looks more Realistic on the Edges)
            {
                noiseValueX = _ix * noiseValueX;
                noiseValueY = _iy * noiseValueX;

                if (noiseValueX > _noiseRange && noiseValueY > _noiseRange)
                {
                    if (_type == MapTile.TileType.water)
                        setWaterTile(_tile);
                    if (_type == MapTile.TileType.mountan)
                        setMountanTile(_tile);
                }
            }
        }

        if (map.getMapTile(region.mapPosition.x - 1, region.mapPosition.y - 1).getMapTileType() == _type)
        {
            if (_ix < region.size / 1.5f && _iy < region.size / edgeFactor) // 1.5f because then it will start to set the Tiles at the 3/4 of the Region (Looks more Realistic on the Edges)
            {
                noiseValueX = (region.size - _ix) * noiseValueX;
                noiseValueY = (region.size - _iy) * noiseValueY;

                if (noiseValueX > _noiseRange && noiseValueY > _noiseRange)
                {
                    if (_type == MapTile.TileType.water)
                        setWaterTile(_tile);
                    if (_type == MapTile.TileType.mountan)
                        setMountanTile(_tile);
                }
            }
        }

        if (map.getMapTile(region.mapPosition.x - 1, region.mapPosition.y + 1).getMapTileType() == _type)
        {
            if (_ix < region.size / 1.5f && _iy > region.size / edgeFactor) // 1.5f because then it will start to set the Tiles at the 3/4 of the Region (Looks more Realistic on the Edges)
            {
                noiseValueX = (region.size - _ix) * noiseValueX;
                noiseValueY = _iy * noiseValueX;

                if (noiseValueX > _noiseRange && noiseValueY > _noiseRange)
                {
                    if (_type == MapTile.TileType.water)
                        setWaterTile(_tile);
                    if (_type == MapTile.TileType.mountan)
                        setMountanTile(_tile);
                }
            }
        }

        if (map.getMapTile(region.mapPosition.x + 1, region.mapPosition.y + 1).getMapTileType() == _type)
        {
            if (_ix > region.size / 1.5f && _iy < region.size / edgeFactor) // 1.5f because then it will start to set the Tiles at the 3/4 of the Region (Looks more Realistic on the Edges)
            {
                noiseValueX = _ix * noiseValueY;
                noiseValueY = (region.size - _iy) * noiseValueY;

                if (noiseValueX > _noiseRange && noiseValueY > _noiseRange)
                {
                    if (_type == MapTile.TileType.water)
                        setWaterTile(_tile);
                    if (_type == MapTile.TileType.mountan)
                        setMountanTile(_tile);
                }
            }
        }
        /////////
        ///
        /*region.*/
        regionTile.noiseValueCrossingX = noiseValueX;
        /*region.*/
        regionTile.noiseValueCrossingY = noiseValueY;
    }

    //private async void InstantiateAsync()
    //{
    //    await Task.Delay(1000);
    //    Instantiate(regionTile);
    //}

    //private async void InstantiateRegionTile(int _ix, int _iy, Vector2 _worldPositionRegion, Region _instantiatedRegion)
    //{
    //    //InstantiateAsync();

    //    await Task.Delay(1000);

    //    _instantiatedRegion.regionTiles[_ix, _iy] = Instantiate(regionTile, new Vector3(_ix + _worldPositionRegion.x, _iy + _worldPositionRegion.y, 0), Quaternion.identity, _instantiatedRegion.transform);

    //    InstantiateBlock(_ix, _iy, _worldPositionRegion, _instantiatedRegion);
    //}

    private void InstantiateRegionTile(int _ix, int _iy, Vector2 _worldPositionRegion, Region _instantiatedRegion)
    {
        _instantiatedRegion.regionTiles[_ix, _iy] = Instantiate(regionTile, new Vector3(_ix + _worldPositionRegion.x, _iy + _worldPositionRegion.y, 0), Quaternion.identity, _instantiatedRegion.transform);

        InstantiateBlock(_ix, _iy, _worldPositionRegion, _instantiatedRegion);
    }

    private void InstantiateBlock(int _ix, int _iy, Vector2 _worldPositionRegion, Region _instantiatedRegion)
    {
        if (regionTile.tileType == RegionTile.regionTileType.mountan)
        {
            terrainHolder.block.GetComponent<MeshRenderer>().material = materialHolder.mountan1;
            _instantiatedRegion.blocks[_ix, _iy] = Instantiate(terrainHolder.block, new Vector3(_ix + _worldPositionRegion.x, _iy + _worldPositionRegion.y, -0.5f), Quaternion.Euler(0, 0, 90), _instantiatedRegion.regionTiles[_ix, _iy].transform);
        }
    }
    private void InstantiateTree(RegionTile tile, int ix, int iy, int seed, Vector2 worldPositionRegion, Region instantiatedRegion)
    {
        var frequenzy = 5f;
        var amplitude = 1f;
        var noiseRange = 0.5f;

        var noiseValue = Mathf.PerlinNoise(((float)ix + (float)region.worldPositionRegion.x) / (float)world.getWorldSize() * frequenzy + seed, ((float)iy + (float)region.worldPositionRegion.y) / (float)world.getWorldSize() * frequenzy + seed) * amplitude;
        noiseValue *= (float)Random.Range(-1, 2);

        tile.GetComponent<Node>().isWalkable = true;

        if (tile.tileType == RegionTile.regionTileType.grass && noiseValue > noiseRange)
        {
            var rand = Random.Range(0, 2);

            if (rand == 0)
                instantiatedRegion.trees[ix, iy] = Instantiate(terrainHolder.tree1, new Vector3(ix + worldPositionRegion.x, iy + worldPositionRegion.y, -0.1f), Quaternion.Euler(-45, 0, 0), instantiatedRegion.regionTiles[ix, iy].transform);

            if (rand == 1)
                instantiatedRegion.trees[ix, iy] = Instantiate(terrainHolder.tree2, new Vector3(ix + worldPositionRegion.x, iy + worldPositionRegion.y, -0.1f), Quaternion.Euler(-45, 0, 0), instantiatedRegion.regionTiles[ix, iy].transform);

            tile.withObject = true;
        }
    }

    private void InstantiateFlower(RegionTile tile, int ix, int iy, int seed, Vector2 worldPositionRegion, Region instantiatedRegion)
    {
        var flower = terrainHolder.flower1;

        var rand1 = Random.Range(0, 4);

        if (rand1 == 0)
            flower = terrainHolder.flower1;
        if (rand1 == 1)
            flower = terrainHolder.flower2;
        if (rand1 == 2)
            flower = terrainHolder.flower3;
        if (rand1 == 3)
            flower = terrainHolder.flower4;

        var rand2 = Random.Range(0, 10);

        if (rand2 == 2 && !tile.withObject && tile.tileType == RegionTile.regionTileType.grass)
        {
            instantiatedRegion.flowers[ix, iy] = Instantiate(flower, new Vector3(ix + worldPositionRegion.x, iy + worldPositionRegion.y, -0.1f), Quaternion.Euler(0, 90, -45), instantiatedRegion.regionTiles[ix, iy].transform);
            tile.withObject = true;
        }
    }
}
