using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : MonoBehaviour
{
    public Vector3 position;

    public int Cost;

    public int GCost;
    public int HCost;
    public int FCost;

    public bool isWalkable = true;
    public bool isOpen;
    public bool isClosed;

    public Node parent;

    public List<Node> children = new List<Node>();

    private void Awake()
    {
        isWalkable = true;
        Cost = 10;
    }

    private void Start()
    {
        position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
        name = "Tile: " + position.x + ", " + position.y;
    }

    public void InsertChild(Node _child)
    {
        if (children.Count > 0)
        {
            for (int i = 0; i < children.Count; i++)
            {
                try
                {
                    if (_child.FCost == children[i].FCost && _child.HCost <= children[i].HCost)
                    {
                        children.Insert(i, _child);
                        break;
                    }
                    else if (_child.FCost < children[i].FCost)
                    {
                        children.Insert(i, _child);
                        break;
                    }
                    else if (_child.FCost > children[i].FCost && _child.FCost <= children[i + 1].FCost)
                    {
                        children.Insert(i + 1, _child);
                        break;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    children.Add(_child);
                    break;
                }

            }
        }
        else
            children.Add(_child);
    }

    public void setClosedProps()
    {
        isOpen = false;
        isClosed = true;
    }
    public void setOpenProps()
    {
        isOpen = true;
        isClosed = false;
    }
}
