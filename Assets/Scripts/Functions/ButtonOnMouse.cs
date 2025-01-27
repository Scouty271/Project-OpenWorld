using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOnMouse : MonoBehaviour
{
    public Raycast raycast;

    public bool isClickedDebug = false;

    public void OnClickButtonCutTree()
    {
        isClickedDebug = true;

        var endNode = raycast.hittetObjectClickedRight.GetComponentInParent<RegionTile>().GetComponent<Node>();

        if (!Input.GetKey(KeyCode.LeftShift))
            raycast.selectedObject.GetComponent<JobHandling>().jobQueue.Clear();

        raycast.selectedObject.GetComponent<JobHandling>().addJob_MoveInFront(endNode);
        raycast.selectedObject.GetComponent<JobHandling>().addJob_FellTree(endNode);
    }

    public void OnClickButtonCutStone()
    {
        isClickedDebug = true;

        var endNode = raycast.hittetObjectClickedRight.GetComponentInParent<RegionTile>().GetComponent<Node>();

        if (!Input.GetKey(KeyCode.LeftShift))
            raycast.selectedObject.GetComponent<JobHandling>().jobQueue.Clear();

        raycast.selectedObject.GetComponent<JobHandling>().addJob_MoveInFront(endNode);
        raycast.selectedObject.GetComponent<JobHandling>().addJob_CutStone(endNode);
    }
}
