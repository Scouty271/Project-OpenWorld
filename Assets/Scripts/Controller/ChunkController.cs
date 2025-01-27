using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

public class ChunkController : MonoBehaviour
{
    public enum ChunkGenerationTypes
    {
        threeTimesThree,
        fiveTimesFive
    }
    public ChunkGenerationTypes types;

    public WorldController world;
    public Region region;
    public TerrainHolder prefabHolder;
    public PositionDetector positionDetector;
    public RegionGenerator regionGenerator;
    public MapGenerator mapGenerator;

    public List<Region> activeRegions = new List<Region>();

    ////public void HandleChunkGeneration3x3()
    ////{
    ////    positionDetector = world.mainCharacter.GetComponent<PositionDetector>();
    ////    HandleRegionCreation3x3();
    ////    HandleRegionDeactivation3x3();
    ////    HandleRegionActivation3x3();
    ////    positionDetector.hittetRegionLast = positionDetector.hittetRegionCurrent;
    ////}

    public void HandleChunkGeneration5x5()
    {
        positionDetector = world.mainCharacter.GetComponent<PositionDetector>();
        if (positionDetector.hittetRegionCurrent != positionDetector.hittetRegionLast && positionDetector.hittetRegionCurrent != null)
        {
            HandleRegionCreation5x5();

            foreach (var item in activeRegions)
                item.fillNeighborList();
        }
        if (positionDetector.hittetRegionCurrent != positionDetector.hittetRegionLast && positionDetector.hittetRegionLast != null)
        {
            HandleRegionDeactivation5x5();
            HandleRegionActivation5x5();

            foreach (var item in activeRegions)
                item.fillNeighborList();
        }

        positionDetector.hittetRegionLast = positionDetector.hittetRegionCurrent;
    }

    //////////////////////////////

    //private void HandleRegionCreation3x3()
    //{
    //    positionDetector = world.mainCharacter.GetComponent<PositionDetector>();

    //    if (positionDetector.hittetRegionCurrent != positionDetector.hittetRegionLast && positionDetector.hittetRegionCurrent != null)
    //    {
    //        Vector2Int currMapPos = new Vector2Int((int)positionDetector.hittetRegionCurrent.mapPosition.x, (int)positionDetector.hittetRegionCurrent.mapPosition.y);
    //        Vector2Int currWorldPos = new Vector2Int((int)positionDetector.hittetRegionCurrent.worldPositionRegion.x, (int)positionDetector.hittetRegionCurrent.worldPositionRegion.y);

    //        //Innerer Ring

    //        for (int ix = -1; ix <= 1; ix++)
    //            for (int iy = -1; iy <= 1; iy++)
    //                CreateRegion(currMapPos, currWorldPos, ix, iy);
    //    }
    //}

    private void HandleRegionCreation5x5()
    {
        positionDetector = world.mainCharacter.GetComponent<PositionDetector>();
        Vector2Int currMapPos = new Vector2Int((int)positionDetector.hittetRegionCurrent.mapPosition.x, (int)positionDetector.hittetRegionCurrent.mapPosition.y);
        Vector2Int currWorldPos = new Vector2Int((int)positionDetector.hittetRegionCurrent.worldPositionRegion.x, (int)positionDetector.hittetRegionCurrent.worldPositionRegion.y);

        for (int ix = -2; ix <= 2; ix++)
            for (int iy = -2; iy <= 2; iy++)
                CreateRegion(currMapPos, currWorldPos, ix, iy);

        setActiveRegions(currMapPos, currWorldPos);
    }

    private void CreateRegion(Vector2Int currMapPos, Vector2Int currWorldPos, int x, int y)
    {
        if (world.getRegion(new Vector2Int(currMapPos.x + x, currMapPos.y + y)) == null)
        {
            var vRegion = regionGenerator.Create(new Vector2Int(currWorldPos.x + x * region.getSize(), currWorldPos.y + y * region.getSize()), prefabHolder.regionTile, world.regionController.gameObject);

            world.setRegion(vRegion, currMapPos.x + x, currMapPos.y + y);


            world.getRegion(currMapPos.x + x, currMapPos.y + y).relativePositionToMiddleChunk.x = x;
            world.getRegion(currMapPos.x + x, currMapPos.y + y).relativePositionToMiddleChunk.y = y;
        }
    }

    //////////////////////////////

    //private void HandleRegionDeactivation3x3()
    //{
    //    if (positionDetector.hittetRegionCurrent != positionDetector.hittetRegionLast && positionDetector.hittetRegionLast != null)
    //    {
    //        Vector2Int currMapPos = new Vector2Int((int)positionDetector.hittetRegionCurrent.mapPosition.x, (int)positionDetector.hittetRegionCurrent.mapPosition.y);
    //        Vector2Int lastMapPos = new Vector2Int((int)positionDetector.hittetRegionLast.mapPosition.x, (int)positionDetector.hittetRegionLast.mapPosition.y);

    //        if (lastMapPos.x < currMapPos.x)
    //            for (int i = -1; i <= 1; i++)
    //                DeactivateRegion(lastMapPos, -1, i);

    //        if (lastMapPos.x > currMapPos.x)
    //            for (int i = -1; i <= 1; i++)
    //                DeactivateRegion(lastMapPos, 1, i);

    //        if (lastMapPos.y > currMapPos.y)
    //            for (int i = -1; i <= 1; i++)
    //                DeactivateRegion(lastMapPos, i, 1);

    //        if (lastMapPos.y < currMapPos.y)
    //            for (int i = -1; i <= 1; i++)
    //                DeactivateRegion(lastMapPos, i, -1);
    //    }
    //}
    private void HandleRegionDeactivation5x5()
    {
        Vector2Int currMapPos = new Vector2Int((int)positionDetector.hittetRegionCurrent.mapPosition.x, (int)positionDetector.hittetRegionCurrent.mapPosition.y);
        Vector2Int lastMapPos = new Vector2Int((int)positionDetector.hittetRegionLast.mapPosition.x, (int)positionDetector.hittetRegionLast.mapPosition.y);

        if (lastMapPos.x < currMapPos.x)
            for (int i = -2; i <= 2; i++)
                DeactivateRegion(lastMapPos, -2, i);

        if (lastMapPos.x > currMapPos.x)
            for (int i = -2; i <= 2; i++)
                DeactivateRegion(lastMapPos, 2, i);

        if (lastMapPos.y > currMapPos.y)
            for (int i = -2; i <= 2; i++)
                DeactivateRegion(lastMapPos, i, 2);

        if (lastMapPos.y < currMapPos.y)
            for (int i = -2; i <= 2; i++)
                DeactivateRegion(lastMapPos, i, -2);
    }

    private void DeactivateRegion(Vector2Int lastMapPos, int x, int y)
    {
        var vRegion = world.getRegion(lastMapPos.x + x, lastMapPos.y + y);

        vRegion.relativePositionToMiddleChunk.x = 0;
        vRegion.relativePositionToMiddleChunk.y = 0;
        vRegion.informationInterfaceMapPositions.Clear();

        vRegion.Deactivate();
    }

    //////////////////////////////

    //private void HandleRegionActivation3x3()
    //{
    //    if (positionDetector.hittetRegionCurrent != positionDetector.hittetRegionLast && positionDetector.hittetRegionLast != null)
    //    {
    //        Vector2Int currMapPos = new Vector2Int((int)positionDetector.hittetRegionCurrent.mapPosition.x, (int)positionDetector.hittetRegionCurrent.mapPosition.y);

    //        for (int ix = -1; ix <= 1; ix++)
    //            for (int iy = -1; iy <= 1; iy++)
    //                ActivateRegion(currMapPos, ix, iy);
    //    }

    //    positionDetector.hittetRegionLast = positionDetector.hittetRegionCurrent;
    //}
    private void HandleRegionActivation5x5()
    {
        Vector2Int currMapPos = new Vector2Int((int)positionDetector.hittetRegionCurrent.mapPosition.x, (int)positionDetector.hittetRegionCurrent.mapPosition.y);

        for (int ix = -2; ix <= 2; ix++)
            for (int iy = -2; iy <= 2; iy++)
                ActivateRegion(currMapPos, ix, iy);

        positionDetector.hittetRegionLast = positionDetector.hittetRegionCurrent;
    }
    private void ActivateRegion(Vector2Int currMapPos, int x, int y)
    {
        var vRegion = world.getRegion(currMapPos.x + x, currMapPos.y + y);

        if (vRegion != null)
        {
            vRegion.relativePositionToMiddleChunk.x = x;
            vRegion.relativePositionToMiddleChunk.y = y;

            vRegion.Activate();
        }
    }

    private void setActiveRegions(Vector2Int currMapPos, Vector2Int currWorldPos)
    {
        activeRegions.Clear();

        for (int ix = -2; ix <= 2; ix++)
            for (int iy = -2; iy <= 2; iy++)
                addActiveRegion(currMapPos, currWorldPos, ix, iy);
    }

    private void addActiveRegion(Vector2Int currMapPos, Vector2Int currWorldPos, int ix, int iy)
    {
        activeRegions.Add(world.getRegion(currMapPos.x + ix, currMapPos.y + iy));
    }
}