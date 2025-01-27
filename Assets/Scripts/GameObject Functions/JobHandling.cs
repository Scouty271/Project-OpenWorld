using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Jobs
{
    idle,
    moveTo,
    moveInFront,
    wait,
    fellTree,
    cutStone
}

public class Job
{
    public string name;
    public Jobs job;
    public Node endNode;
}

public class JobHandling : MonoBehaviour
{
    private Inventory inventory;

    private RegionTileArrayInterface regionTileInterface;

    public Queue<Job> jobQueue = new Queue<Job>();

    public bool busy = false;


    public int debug_jobQueueCounter;

    public Job currentJob = null;

    private Node startNode;

    private Entity entity;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        regionTileInterface = FindObjectOfType<RegionTileArrayInterface>();
        inventory = GetComponent<Inventory>();
    }

    private void Update()
    {
        if (jobQueue.Count > 0/* && !busy*/)
        {
            //Job aus Queue holen

            currentJob = jobQueue.Dequeue();

            debug_jobQueueCounter++;

            busy = true;

            //Job Typifizieren und Methoden aufrufen
            switch (currentJob.job)
            {
                case Jobs.idle:
                    break;

                case Jobs.moveTo:
                    entity.state = Entity.States.Moving;
                    startNode = getStartNode();

                    GetComponent<PathMovement>().setPath(startNode, currentJob.endNode);
                    break;

                case Jobs.moveInFront:
                    entity.state = Entity.States.Moving;
                    startNode = getStartNode();
                    GetComponent<PathMovement>().setPathForInteractionWithNonWalkable(startNode, currentJob.endNode);
                    break;

                case Jobs.wait:
                    break;

                case Jobs.fellTree:
                    Tree tree;
                    if (currentJob.endNode.GetComponentInChildren<Tree>())
                    { tree = currentJob.endNode.GetComponentInChildren<Tree>(); }
                    else
                        break;
                    currentJob.endNode.isWalkable = true;
                    Destroy(tree.gameObject);
                    busy = false;

                    inventory.addItem(Item.ItemTypes.wood);
                    break;

                case Jobs.cutStone:
                    Block block;
                    if (currentJob.endNode.GetComponentInChildren<Block>())
                    { block = currentJob.endNode.GetComponentInChildren<Block>(); }
                    else
                        break;
                    currentJob.endNode.isWalkable = true;
                    Destroy(block.gameObject);
                    busy = false;

                    inventory.addItem(Item.ItemTypes.stone);
                    break;

                default:
                    break;
            }
        }

        //Abschluss des Jobs bestätigen
        if (!busy)
        {
            entity.state = Entity.States.Idle;
            currentJob = null;
            //busy = false;
            debug_jobQueueCounter--;
        }
    }

    private Node getStartNode()
    {
        return regionTileInterface.GetRegionTile(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0)).GetComponent<Node>();
    }

    public void addJob_MoveTo(Node _endNode)
    {
        var job_MoveTo = new Job();
        job_MoveTo.job = Jobs.moveTo;
        job_MoveTo.endNode = _endNode;

        if (currentJob != null && busy)
        {
            if (job_MoveTo.endNode != currentJob.endNode)            
                jobQueue.Enqueue(job_MoveTo);            
        }
        else if(getStartNode() != job_MoveTo.endNode)
            jobQueue.Enqueue(job_MoveTo);
    }
    public void addJob_MoveInFront(Node _endNode)
    {
        var job_MoveInFront = new Job();
        job_MoveInFront.job = Jobs.moveInFront;
        job_MoveInFront.endNode = _endNode;

        jobQueue.Enqueue(job_MoveInFront);
    }

    public void addJob_FellTree(Node _endNode)
    {
        var job_FellTree = new Job();
        job_FellTree.job = Jobs.fellTree;
        job_FellTree.endNode = _endNode;

        jobQueue.Enqueue(job_FellTree);
    }
    public void addJob_CutStone(Node _endNode)
    {
        var job_cutStone = new Job();
        job_cutStone.job = Jobs.cutStone;
        job_cutStone.endNode = _endNode;

        jobQueue.Enqueue(job_cutStone);
    }
}
