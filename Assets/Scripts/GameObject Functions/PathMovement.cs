using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMovement : MonoBehaviour
{
    private Pathfinding pathfinding;

    private RegionTileArrayInterface regionTileArrayInterface;

    private Entity entity;

    private float backupSpeed;
    private float reducedbackupSpeed;

    private bool isPointEndReachedX = true;
    private bool isPointEndReachedY = true;

    public bool isPathGone = true;

    private Vector3 pathPointDirToNext;
    private Vector3 pathPointPos;
    private Vector3 pathPointPosNext;

    public int currPathIndex = 0;

    public List<Node> pathToGo;

    private void Start()
    {
        entity = GetComponent<Entity>();

        pathfinding = FindObjectOfType<Pathfinding>();
        regionTileArrayInterface = FindObjectOfType<RegionTileArrayInterface>();

        backupSpeed = entity.speed;
        reducedbackupSpeed = entity.speed / Mathf.Sqrt(2);
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (pathToGo.Count != 0)
        {
            isPathGone = false;

            drawLines();

            if (isNodeEndReached())
            {
                try
                {
                    if (currPathIndex == pathToGo.Count) // Ziel erreicht
                    {
                        currPathIndex = 0;
                        pathToGo.Clear();
                        isPathGone = true;

                        //Bestätigung des Abschlusses des Jobs in der Entity Klasse
                        GetComponent<JobHandling>().busy = false;

                        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), transform.position.z);

                        clearPath();
                    }
                    else // Ziel noch nicht erreicht
                    {
                        try
                        {
                            if (pathToGo[currPathIndex + 1] != null)
                            {
                                pathPointDirToNext = pathToGo[currPathIndex + 1].position - pathToGo[currPathIndex].position;
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                        }

                        pathPointPos = pathToGo[currPathIndex].position;

                        try
                        {
                            if (pathToGo[currPathIndex + 1] != null)
                            {
                                pathPointPosNext = pathToGo[currPathIndex + 1].position;
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                        }

                        isPointEndReachedX = false;
                        isPointEndReachedY = false;

                        currPathIndex++;

                        //if (!regionTileArrayInterface.GetRegionTile(pathToGo[currPathIndex].GetComponent<RegionTile>().worldPosition).GetComponentInParent<Region>().gameObject.activeSelf)
                        //{
                        //    Debug.Log("Ist Hineingesprungen!!!!");
                        //    currPathIndex = pathToGo.Count;
                        //}

                        try
                        {
                            if (regionTileArrayInterface.GetRegionTile(pathToGo[currPathIndex].GetComponent<RegionTile>().worldPosition).GetComponentInParent<Region>().gameObject == null)
                            {
                                Debug.Log("Try!");
                                currPathIndex = pathToGo.Count;
                            }
                        }
                        catch (System.NullReferenceException)
                        {
                            Debug.Log("Catch!");
                            currPathIndex = pathToGo.Count;
                        }


                    }
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    currPathIndex = 0;
                    pathToGo.Clear();
                    isPathGone = true;

                    GetComponent<JobHandling>().busy = false;

                    transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), transform.position.z);

                    clearPath();
                }
            }

            if (!isPathGone)
            {
                Move(pathPointDirToNext);

                if (pathPointDirToNext.y != 0 || pathPointDirToNext.x != 0)
                    entity.speed = backupSpeed;

                if (pathPointDirToNext.y != 0 && pathPointDirToNext.x != 0)
                    entity.speed = reducedbackupSpeed;
            }

            setEndReachedX();
            setEndReachedY();
        }
    }

    private bool isNodeEndReached()
    {
        if (isPointEndReachedX && isPointEndReachedY)
            return true;
        else
            return false;
    }

    private void setEndReachedX()
    {
        if (!isPointEndReachedX)
        {
            if (pathPointDirToNext.x > 0 && transform.position.x >= pathPointPosNext.x)
                isPointEndReachedX = true;

            if (pathPointDirToNext.x < 0 && transform.position.x <= pathPointPosNext.x)
                isPointEndReachedX = true;

            if (pathPointDirToNext.x == 0)
                isPointEndReachedX = true;
        }
    }

    private void setEndReachedY()
    {
        if (!isPointEndReachedY)
        {
            if (pathPointDirToNext.y > 0 && transform.position.y >= pathPointPosNext.y)
                isPointEndReachedY = true;

            if (pathPointDirToNext.y < 0 && transform.position.y <= pathPointPosNext.y)
                isPointEndReachedY = true;

            if (pathPointDirToNext.y == 0)
                isPointEndReachedY = true;
        }
    }

    private void Move(Vector2 direction)
    {
        transform.Translate(entity.speed * direction.x * GetComponent<Entity>().world.time, entity.speed * direction.y * GetComponent<Entity>().world.time, 0);
    }

    public Vector3Int getPosition()
    {
        return new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
    }

    public void drawLines()
    {
        for (int i = 0; i < pathToGo.Count - 1; i++)
        {
            Debug.DrawLine(new Vector3(pathToGo[i].position.x, pathToGo[i].position.y, -1), new Vector3(pathToGo[i + 1].position.x, pathToGo[i + 1].position.y, -1), Color.grey, 0, false);
        }
    }

    public void clearPath()
    {
        for (int i = 0; i < pathToGo.Count; i++)
        {
            clearNode(pathToGo[i]);
        }
    }

    private void clearNode(Node node)
    {
        node.GCost = 0;
        node.FCost = 0;
        node.HCost = 0;
        node.parent = null;
        node.isOpen = false;
        node.isClosed = false;
        node.children.Clear();
        node.parent = null;
        node = null;
    }

    // Geht bis zum Endnode
    public void setPath(Node _startNode, Node _endNode)
    {
        var path = new List<Node>();

        try
        {
            if (_endNode != _startNode && _endNode.isWalkable && _endNode != null)
            {
                path = pathfinding.FindWithAStarWithEndNode(_startNode, _endNode);

                // Kopieren nötig, da sonst Referenz von Pathfinding die Movement Path Liste löscht
                for (int i = 0; i < path.Count; i++)
                {
                    GetComponent<PathMovement>().pathToGo.Add(path[i]);
                }

                pathfinding.clearValues(_startNode);
                path.Clear();
            }
            else
            {
                //GetComponent<PathMovement>().clearPath();
                //GetComponent<PathMovement>().pathToGo.Clear();

                //pathfinding.clearValues(_startNode);
                //_startNode = null;
                //_endNode = null;

                //path.Clear();

                //Debug.Log("Unpassierbares Gelände!");
            }
        }
        catch (System.Exception)
        {
            //GetComponent<PathMovement>().clearPath();
            //GetComponent<PathMovement>().pathToGo.Clear();

            //pathfinding.clearValues(_startNode);
            //_startNode = null;
            //_endNode = null;

            //path.Clear();

            //Debug.Log("Unpassierbares Gelände!");
        }
    }

    //Bleibt ein Node vor dem Endnode stehen
    public void setPathForInteractionWithNonWalkable(Node _startNode, Node _endNode)
    {
        var path = new List<Node>();

        _endNode.isWalkable = true;

        try
        {
            if (_endNode != _startNode && _endNode.isWalkable)
            {
                path = pathfinding.FindWithAStarWithEndNode(_startNode, _endNode);

                // Kopieren nötig, da sonst Referenz von Pathfinding die Movement Path Liste löscht
                for (int i = 0; i < path.Count - 1; i++)
                {
                    GetComponent<PathMovement>().pathToGo.Add(path[i]);
                }

                pathfinding.clearValues(_startNode);
                path.Clear();
            }
        }
        catch (System.Exception)
        {
            GetComponent<PathMovement>().clearPath();
            GetComponent<PathMovement>().pathToGo.Clear();

            pathfinding.clearValues(_startNode);
            _startNode = null;
            _endNode = null;

            path.Clear();

            Debug.Log("Unpassierbares Gelände!");
        }

        _endNode.isWalkable = false;
    }
}
