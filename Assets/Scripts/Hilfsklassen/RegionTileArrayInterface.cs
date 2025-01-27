using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RegionTileArrayInterface : MonoBehaviour
{
    public WorldController world;
    public RegionTile GetRegionTile(Vector3Int worldPosition)
    {
        try
        {
            // Baustelle! Finden keine Region bei RandomPathGiver nach while addJob
            var region = world.getRegion(worldPosition.x / world.region.size, worldPosition.y / world.region.size);

            var regionTile = region.getRegionTile(worldPosition.x % world.region.size, worldPosition.y % world.region.size);

            return regionTile;
        }
        catch (NullReferenceException)
        {
            //Debug.Log("Interface findet kei RegionTile!");
        }

        return null;
    }
}