using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionDetector : MonoBehaviour
{
    public CameraController cam;

    public RegionTile hittetRegionTileCurrent;

    private RaycastHit hitFromEntity;
    private Ray rayFromEntity;

    public Region hittetRegionCurrent;
    public Region hittetRegionLast;

    private void Start()
    {
        cam = FindObjectOfType<CameraController>().GetComponent<CameraController>();
    }

    private void Update()
    {
        HandleCurrRegionRaycast();

        //Regionzuweisung des Entities
        if (hittetRegionCurrent != hittetRegionLast)
        {
            gameObject.transform.SetParent(hittetRegionCurrent.gameObject.transform);
        }
    }

    private void HandleCurrRegionRaycast()
    {
        int layerMask = 1 << 9;

        rayFromEntity.origin = transform.position;
        rayFromEntity.direction = transform.TransformDirection(0, 0, 1);

        if (Physics.Raycast(rayFromEntity, out hitFromEntity, Mathf.Infinity, layerMask))
        {
            hittetRegionTileCurrent = hitFromEntity.collider.GetComponent<RegionTile>();

            hittetRegionLast = hittetRegionCurrent;

            hittetRegionCurrent = hittetRegionTileCurrent.GetComponentInParent<Region>();
        }
        //else
        //{
        //    hittetRegionLast = hittetRegionCurrent;
        //    hittetRegionTileCurrent = null;
        //    hittetRegionCurrent = null;
        //}
    }
}
