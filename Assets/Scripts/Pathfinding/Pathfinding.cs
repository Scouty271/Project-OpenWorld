using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinding : MonoBehaviour
{
    public RegionTileArrayInterface regionTileInterface;

    public List<Node> openList = new List<Node>();
    public List<Node> closedList = new List<Node>();

    public List<Node> path;

    public bool pathFound;

    public List<Node> FindWithAStarWithEndNode(Node _startNode, Node _endNode)
    {
        path = new List<Node>();

        var currNode = _startNode;

        currNode.setOpenProps();

        addNeighborsToOpenWithEnd(currNode, _startNode, _endNode);

        while (true)
        {
            addNeighborsToOpenWithEnd(currNode, _startNode, _endNode);

            if (currNode.children.Count > 0)
            {
                foreach (var child in currNode.children)
                {
                    if (child.FCost <= openList[0].FCost)
                    {
                        if (child.isOpen)
                        {
                            closedList.Add(currNode);
                            currNode.setClosedProps();
                        }

                        closedList.Add(currNode);
                        child.setClosedProps();

                        openList.Remove(child);
                        currNode = child;
                        updateNeightborsWithEnd(currNode, _startNode, _endNode);
                        break;
                    }
                    else
                    {
                        currNode = openList[0];
                        closedList.Add(currNode);
                        currNode.setClosedProps();

                        openList.Remove(currNode);
                        updateNeightborsWithEnd(currNode, _startNode, _endNode);
                        break;
                    }
                }
            }
            else
            {
                currNode = openList[0];

                closedList.Add(currNode);
                currNode.setClosedProps();

                openList.Remove(currNode);
            }

            if (currNode == _endNode)
            {
                while (true)
                {
                    if (currNode.parent != null)
                    {
                        path.Add(currNode);
                        currNode = currNode.parent;
                    }
                    else
                        break;

                }
                break;
            }

            if (closedList.Count > 2000)
            {
                clearValues(_startNode);
                Debug.Log("Zu langer Weg!");
                break;
            }
        }
        path.Add(_startNode);
        path.Reverse();

        for (int i = 0; i < closedList.Count - 1; i++)
        {
            clearNode(closedList[i]);
        }

        return path;
    }

    private void updateNeightborsWithEnd(Node _currNode, Node _startNode, Node _endNode)
    {
        if (_currNode != null)
        {
            updateNeightborWithEnd(_currNode, 1, 0, _startNode, _endNode);
            updateNeightborWithEnd(_currNode, -1, 0, _startNode, _endNode);
            updateNeightborWithEnd(_currNode, 0, 1, _startNode, _endNode);
            updateNeightborWithEnd(_currNode, 0, -1, _startNode, _endNode);

            updateNeightborWithEnd(_currNode, 1, 1, _startNode, _endNode);
            updateNeightborWithEnd(_currNode, -1, 1, _startNode, _endNode);
            updateNeightborWithEnd(_currNode, -1, -1, _startNode, _endNode);
            updateNeightborWithEnd(_currNode, 1, -1, _startNode, _endNode);
        }
    }

    private void updateNeightborWithEnd(Node _currNode, int _x, int _y, Node _startNode, Node _endNode)
    {
            if (regionTileInterface.GetRegionTile(new Vector3Int(_currNode.GetComponent<RegionTile>().getPositionInWorld().x + _x, _currNode.GetComponent<RegionTile>().getPositionInWorld().y + _y, 0)) != null)
            {
                    var currNode = regionTileInterface.GetRegionTile(new Vector3Int(_currNode.GetComponent<RegionTile>().getPositionInWorld().x + _x, _currNode.GetComponent<RegionTile>().getPositionInWorld().y + _y, 0)).GetComponent<Node>();

                    if (currNode.isWalkable && currNode != null && currNode.isOpen && currNode.parent != _currNode && !currNode.isClosed)
                    {
                        var backupGCost = currNode.GCost;

                        setTilePathfindingValues(currNode, _currNode, _startNode, _endNode);

                        if (currNode.GCost <= backupGCost)
                        {
                            currNode.parent.children.Remove(currNode);
                            currNode.parent = _currNode;
                            _currNode.InsertChild(currNode);
                        }
                        else
                        {
                            setTilePathfindingValues(currNode, currNode.parent, _startNode, _endNode);
                        }
                    }
            }
    }

    private void addNeighborsToOpenWithEnd(Node _currNode, Node _startNode, Node _endNode)
    {
        addNeighborToOpenWithEnd(_currNode, 1, 0, _startNode, _endNode);
        addNeighborToOpenWithEnd(_currNode, -1, 0, _startNode, _endNode);
        addNeighborToOpenWithEnd(_currNode, 0, 1, _startNode, _endNode);
        addNeighborToOpenWithEnd(_currNode, 0, -1, _startNode, _endNode);

        addNeighborToOpenWithEnd(_currNode, 1, 1, _startNode, _endNode);
        addNeighborToOpenWithEnd(_currNode, -1, -1, _startNode, _endNode);
        addNeighborToOpenWithEnd(_currNode, -1, 1, _startNode, _endNode);
        addNeighborToOpenWithEnd(_currNode, 1, -1, _startNode, _endNode);
    }

    private void addNeighborToOpenWithEnd(Node _currNode, int _x, int _y, Node _startNode, Node _endNode)
    {
        try
        {
            if (regionTileInterface.GetRegionTile(new Vector3Int(_currNode.GetComponent<RegionTile>().getPositionInWorld().x + _x, _currNode.GetComponent<RegionTile>().getPositionInWorld().y + _y, 0))/*.GetComponent<Node>()*/ != null)
            {
                var currNode = regionTileInterface.GetRegionTile(new Vector3Int(_currNode.GetComponent<RegionTile>().getPositionInWorld().x + _x, _currNode.GetComponent<RegionTile>().getPositionInWorld().y + _y, 0)).GetComponent<Node>();

                if (currNode != null && !currNode.isOpen && currNode.isWalkable && !currNode.isClosed)
                {
                    setTilePathfindingValues(currNode, _currNode, _startNode, _endNode);
                    currNode.parent = _currNode;
                    _currNode.InsertChild(currNode);
                    InsertToOpen(currNode);

                    currNode.isOpen = true;
                    currNode.setOpenProps();
                }
            }
        }
        catch (IndexOutOfRangeException)
        {
        }
    }

    public void InsertToOpen(Node _node)
    {
        if (openList.Count > 0)
        {
            for (int i = 0; i < openList.Count; i++)
            {
                try
                {
                    if (_node.FCost == openList[i].FCost && _node.HCost <= openList[i].HCost)
                    {
                        openList.Insert(i, _node);
                        break;
                    }
                    else if (_node.FCost < openList[i].FCost)
                    {
                        openList.Insert(i, _node);
                        break;
                    }
                    else if (_node.FCost > openList[i].FCost && _node.FCost <= openList[i + 1].FCost)
                    {
                        openList.Insert(i + 1, _node);
                        break;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    openList.Add(_node);
                    break;
                }
            }
        }
        else
            openList.Add(_node);
    }

    private void setTilePathfindingValues(Node _neighborNode, Node currNode, Node _startNode, Node _endNode)
    {
        var neighborXPos = _neighborNode.position.x;
        var neighborYPos = _neighborNode.position.y;

        var currXPos = currNode.position.x;
        var currYPos = currNode.position.y;

        var diffX = neighborXPos - currXPos;
        var diffY = neighborYPos - currYPos;

        //////////////////////////////////////////////////////////

        _neighborNode.GCost = 0;

        if (diffX < 0)
            diffX = -diffX;

        if (diffY < 0)
            diffY = -diffY;

        if (diffX > 0 && diffY > 0)
        {
            _neighborNode.GCost = currNode.GCost + _neighborNode.Cost + 4;
        }
        else if (diffX > 0)
        {
            _neighborNode.GCost = currNode.GCost + _neighborNode.Cost;
        }
        else if (diffY > 0)
        {
            _neighborNode.GCost = currNode.GCost + _neighborNode.Cost;
        }

        //////////////////////////////////////////////////////////

        _neighborNode.HCost = 0;

        var endXPos = _endNode.position.x;
        var endYPos = _endNode.position.y;

        var xDiffEnd = neighborXPos - endXPos;
        var yDiffEnd = neighborYPos - endYPos;

        if (xDiffEnd < 0)
            xDiffEnd = -xDiffEnd;

        if (yDiffEnd < 0)
            yDiffEnd = -yDiffEnd;

        while (xDiffEnd != 0 || yDiffEnd != 0)
        {
            if (xDiffEnd > 0 && yDiffEnd > 0)
            {
                _neighborNode.HCost = _neighborNode.HCost + _neighborNode.Cost + 4;

                xDiffEnd--;
                yDiffEnd--;
            }
            else if (xDiffEnd > 0)
            {
                _neighborNode.HCost = _neighborNode.HCost + _neighborNode.Cost;
                xDiffEnd--;
            }
            else if (yDiffEnd > 0)
            {
                _neighborNode.HCost = _neighborNode.HCost + _neighborNode.Cost;
                yDiffEnd--;
            }
        }

        _neighborNode.FCost = _neighborNode.HCost + _neighborNode.GCost;
    }

    public void clearValues(Node _startNode)
    {
        pathFound = false;

        var opencleared = false;
        var closedcleared = false;
        var pathcleared = false;

        clearNode(_startNode);

        for (int i = 0; i < closedList.Count; i++)
        {
            clearNode(closedList[i]);

            for (int ix = 0; ix < closedList[i].children.Count; ix++)
            {
                clearNode(closedList[i].children[ix]);
            }

            closedcleared = true;
        }

        for (int i = 0; i < path.Count; i++)
        {
            clearNode(path[i]);

            for (int ix = 0; ix < path[i].children.Count; ix++)
            {
                clearNode(path[i].children[ix]);
            }

            pathcleared = true;
        }

        for (int i = 0; i < openList.Count; i++)
        {
            clearNode(openList[i]);

            for (int ix = 0; ix < openList[i].children.Count; ix++)
            {
                clearNode(openList[i].children[ix]);
            }

            opencleared = true;
        }

        if (closedcleared)
            closedList.Clear();
        if (opencleared)
            openList.Clear();
        if (pathcleared)
            path.Clear();
    }

    private void clearNode(Node node)
    {
        node.FCost = 0;
        node.GCost = 0;
        node.HCost = 0;

        node.isOpen = false;
        node.isClosed = false;

        if (node.parent != null)
        {
            node.parent.GCost = 0;
            node.parent.FCost = 0;
            node.parent.HCost = 0;
            node.parent.isClosed = false;
            node.parent.isOpen = false;
        }

        if (node.children.Count > 0)
        {
            for (int i = 0; i < node.children.Count; i++)
            {
                node.children[i].FCost = 0;
                node.children[i].HCost = 0;
                node.children[i].GCost = 0;
                node.children[i].isOpen = false;
                node.children[i].parent = null;
                node.children[i].isClosed = false;
            }
        }

        node.children.Clear();
    }
}
