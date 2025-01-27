using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public enum Entities
    {
        Human,
        Deer
    }
    public Entities entity;

    public WorldController world;
    public EntityHolder entityHolder;

    private float entitySpawnHeight = 0.1f;

    public Vector3Int spawnPos;

    public void spawnEntity(Entities entity)
    {
        if (Entities.Human == entity)
        {
            while (true)
            {
                spawnPos = new Vector3Int(Random.Range(1, world.worldSize - 1), Random.Range(1, world.worldSize - 1), 0);
                if (world.regionTileArrayInterface.GetRegionTile(spawnPos).GetComponent<Node>().isWalkable)
                {
                    break;
                }
            }
            //entityHolder.humans.Add(Instantiate(entityHolder.mainCharacter, new Vector3(spawnPos.x, spawnPos.y, entitySpawnHeight), Quaternion.identity, entityHolder.transform));

            Instantiate(entityHolder.mainCharacter, new Vector3(spawnPos.x, spawnPos.y, entitySpawnHeight), Quaternion.identity, entityHolder.transform);
        }
    }
}
