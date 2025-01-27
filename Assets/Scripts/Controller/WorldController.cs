using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using System.Threading;
using System.Threading.Tasks;

public class WorldController : MonoBehaviour
{
    public Human mainCharacter;
    public Human worker;
    public Deer deer;

    public RegionTileArrayInterface regionTileArrayInterface;

    public RegionController regionController;
    public MapController mapController;

    public Region region;
    public RegionGenerator regionGenerator;
    public ChunkController chunkController;
    public Map map;
    public MapGenerator mapGenerator;
    public CameraController cam;
    public TerrainHolder prefabHolder;

    public bool timeStopped;
    public float time;

    private Region[,] regions;

    public int seed;

    public int worldSize;

    public bool fixedSeed;

    public bool startAtMiddleOfMap;

    public Vector2Int startMapPos;

    private void Awake()
    {
        time = 1;
        regions = new Region[worldSize, worldSize];
    }

    private void Start()
    {
        if (!fixedSeed)
            seed = Random.Range(0, 10000);

        if (startAtMiddleOfMap)
        {
            startMapPos = new Vector2Int(worldSize / 2, worldSize / 2);
        }

        mapGenerator.createMapTiles();

        var vRegion = regionGenerator.Create(new Vector2Int(startMapPos.x * region.getSize(), startMapPos.y * region.getSize()), prefabHolder.regionTile, regionController.gameObject);

        setRegion(vRegion, startMapPos.x, startMapPos.y);

        chunkController.activeRegions.Add(vRegion);

        mainCharacter = Instantiate(mainCharacter, new Vector3(startMapPos.x * region.getSize() + 2, startMapPos.y * region.getSize() + 2, -0.1f), Quaternion.identity, this.transform);

        cam.transform.position = new Vector3(mainCharacter.transform.position.x, mainCharacter.transform.position.y, cam.transform.position.z);

        worker = Instantiate(worker, new Vector3(startMapPos.x * region.getSize(), startMapPos.y * region.getSize(), -0.1f), Quaternion.identity, this.transform);
    }

    private void Update()
    {
        chunkController.HandleChunkGeneration5x5();
    }

    public Region getRegion(Vector2Int index)
    {
        return regions[index.x, index.y];
    }

    public Region getRegion(int x, int y)
    {
        return regions[x, y];
    }

    public int getWorldSize()
    {
        return worldSize;
    }

    public MapTile getCurrMapTileFromRegion()
    {
        return map.getMapTile((int)mainCharacter.GetComponent<PositionDetector>().hittetRegionCurrent.mapPosition.x, (int)mainCharacter.GetComponent<PositionDetector>().hittetRegionCurrent.mapPosition.y);
    }

    public void setRegion(Region region, Vector2Int index)
    {
        regions[index.x, index.y] = region;
    }

    public void setRegion(Region region, int x, int y)
    {
        regions[x, y] = region;
    }
}
