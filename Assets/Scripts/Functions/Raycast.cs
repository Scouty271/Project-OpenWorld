using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Raycast : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;

    public Camera cam;
    public Pathfinding pathfinding;
    public RegionTileArrayInterface regionTileInterface;

    public Selection selectedObject;

    public Node startNode;
    public Node endNode;

    public Region hittetRegion;

    public ButtonOnMouse buttonOnMouse;
    public GameObject buttonTree;
    public GameObject buttonsBlock;

    public GameObject hittetMouseObjectDebug;

    public GameObject hittetObjectClickedRight;

    private void Start()
    {
        ray = new Ray();
    }
    private void Update()
    {
        ray.origin = cam.ScreenToWorldPoint(Input.mousePosition);
        ray.direction = this.transform.TransformDirection(new Vector3(0, 0, 1));

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            handlePassiveSelection();

            handleSelectObject();

            //Sofortige Jobzuweisung
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                if (areMovePropertiesFullfilled())
                {
                    selectedObject.GetComponent<JobHandling>().jobQueue.Clear();

                    endNode = hit.collider.GetComponent<Node>();

                    selectedObject.GetComponent<JobHandling>().addJob_MoveTo(endNode);
                }

                if (areTreeButtonPropertiesFullfilled())
                {
                    handleButtonCutTreeActivation();
                }

                if (areBlockButtonPropertiesFullfilled())
                {
                    handleButtonCutBlockActivation();
                }
            }

            //Jobkette
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (areMovePropertiesFullfilled() && Input.GetKey(KeyCode.LeftShift))
                {
                    endNode = hit.collider.GetComponent<Node>();

                    selectedObject.GetComponent<JobHandling>().addJob_MoveTo(endNode);
                }

                if (areTreeButtonPropertiesFullfilled() && Input.GetKey(KeyCode.LeftShift))
                    handleButtonCutTreeActivation();

                if (areBlockButtonPropertiesFullfilled() && Input.GetKey(KeyCode.LeftShift))
                    handleButtonCutBlockActivation();
            }
            handleUnselectButton();
        }
        handleUnselectObject();
    }

    /////////////////////////////////

    private void handlePassiveSelection()
    {
        hittetRegion = hit.collider.GetComponentInParent<Region>();
        hittetMouseObjectDebug = hit.collider.gameObject;
    }

    private void handleSelectObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (hit.collider.GetComponent<Selection>() != null)
            {
                selectedObject = hit.collider.GetComponent<Selection>();
                selectedObject.isSelected = true;
            }
        }
    }

    private bool areMovePropertiesFullfilled()
    {
        if (Input.GetMouseButtonDown(1) &&
            selectedObject != null &&
            selectedObject.isControllable &&
            hit.collider.GetComponent<Node>() &&
            hit.collider.GetComponent<Node>().isWalkable)
            return true;
        else
            return false;
    }

    private bool areTreeButtonPropertiesFullfilled()
    {
        if (Input.GetMouseButtonDown(1) && selectedObject != null && selectedObject.isControllable && hit.collider.GetComponent<Tree>())
            return true;
        else
            return false;
    }

    private bool areBlockButtonPropertiesFullfilled()
    {
        if (Input.GetMouseButtonDown(1) && selectedObject != null && selectedObject.isControllable && hit.collider.GetComponent<Block>())
            return true;
        else
            return false;
    }

    private void handleButtonCutTreeActivation()
    {
        hittetObjectClickedRight = hit.collider.GetComponent<Tree>().gameObject;
        buttonTree.transform.position = Input.mousePosition;
        buttonOnMouse.gameObject.SetActive(true);
        buttonTree.SetActive(true);
    }

    private void handleButtonCutBlockActivation()
    {
        hittetObjectClickedRight = hit.collider.GetComponent<Block>().gameObject;
        buttonsBlock.transform.position = Input.mousePosition;
        buttonOnMouse.gameObject.SetActive(true);
        buttonsBlock.SetActive(true);
    }

    private void handleUnselectButton()
    {
        if (Input.GetMouseButtonUp(0))
        {
            buttonOnMouse.gameObject.SetActive(false);
            buttonsBlock.SetActive(false);
            buttonTree.SetActive(false);
        }
    }

    private void handleUnselectObject()
    {
        if (Input.GetMouseButtonDown(0) && FindObjectOfType<StateController>().state == StateController.State.gameState)
        {
            if (hit.collider != null && hit.collider.GetComponent<Selection>() == null)
            {
                if (selectedObject && !buttonOnMouse.gameObject.activeSelf)
                {
                    selectedObject.isSelected = false;
                    selectedObject = null;
                }
            }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            selectedObject = null;
        }
    }
}
