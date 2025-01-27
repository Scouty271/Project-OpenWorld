using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Flora
{
    private void Start()
    {
        GetComponentInParent<RegionTile>().GetComponent<Node>().isWalkable = false;

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (transform.position.y / 10000));
    }
}
