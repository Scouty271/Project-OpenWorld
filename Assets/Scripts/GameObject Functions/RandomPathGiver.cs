using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPathGiver : MonoBehaviour
{
    public Map map;
    public StateController stateController;
    public Pathfinding pathfinding;
    public RegionTileArrayInterface tileInterface;

    public DeactivatedInformationGameObjects deactivatedInformationGameObjects;

    private Node endNode;

    // Für RegionChange
    Vector2Int neighborPos;

    private int moveRangeForJob = 30;

    private void Start()
    {
        stateController = FindObjectOfType<StateController>();
        pathfinding = FindObjectOfType<Pathfinding>();
        tileInterface = FindObjectOfType<RegionTileArrayInterface>();
        deactivatedInformationGameObjects = FindObjectOfType<DeactivatedInformationGameObjects>();
        map = FindObjectOfType<Map>();
    }

    private void Update()
    {
        if (stateController.state == StateController.State.gameState && !GetComponent<JobHandling>().busy && gameObject.GetComponent<Entity>().state == Entity.States.Idle)
        {
            //var xRand = Random.Range(-moveRangeForJob, moveRangeForJob);
            //var yRand = Random.Range(-moveRangeForJob, moveRangeForJob);

            var xRand = Random.Range(0, 0);
            var yRand = Random.Range(0, -moveRangeForJob);

            var xPos = Mathf.RoundToInt(transform.position.x);
            var yPos = Mathf.RoundToInt(transform.position.y);

            var xValue = xPos + xRand;
            var yValue = yPos + yRand;


            // Sollte ein Node null sein (Nicht aktiv, weil außerhalb des Genererierten Bereiches)
            try
            {
                endNode = tileInterface.GetRegionTile(new Vector3Int(xValue, yValue, 0)).GetComponent<Node>();

                if (endNode.isWalkable && endNode.gameObject.GetComponentInParent<Region>().gameObject.activeSelf)
                    GetComponent<JobHandling>().addJob_MoveTo(endNode);
            }
            catch (System.NullReferenceException)
            {
                var i = 0;
                while (i < moveRangeForJob)
                {
                    // Wurde Negativ auf Positiv umgewandelt? Wenn ja am Ende der while wieder Umwandeln um richtige Position zu bekommen!
                    var xConvertet = false;
                    var yConvertet = false;
                    // Nur Positive Werte als Differenz!
                    if (xRand < 0)
                    {
                        xRand = -xRand;
                        xConvertet = true;
                    }
                    if (yRand < 0)
                    {
                        yRand = -yRand;
                        yConvertet = true;
                    }
                    // (-)2,(-)2
                    if (xRand == yRand)
                    {
                        xRand--;
                        yRand--;
                    }
                    // (-)2,0 / (-)2,(-)1
                    else if (xRand > yRand)
                        xRand--;
                    // 0,(-)2 / (-)1,(-)2
                    else if (yRand > xRand)
                        yRand--;
                    if (xConvertet)
                        xRand = -xRand;
                    if (yConvertet)
                        yRand = -yRand;
                    try
                    {
                        xValue = xPos + xRand;
                        yValue = yPos + yRand;
                        endNode = tileInterface.GetRegionTile(new Vector3Int(xValue, yValue, 0)).GetComponent<Node>();
                        if (endNode.isWalkable && endNode != null)
                        {
                            GetComponent<JobHandling>().addJob_MoveTo(endNode);
                            break;
                        }
                    }
                    catch (System.NullReferenceException)
                    {
                    }
                    i++;
                }
                Debug.Log(i);


                // Informieren der Region in dem das Reh gelaufen ist
                if (GetComponent<PositionDetector>().hittetRegionLast.informationInterfaceMapPositions.Count != 0)
                {
                    Information info = new Information(Information.Informationpackages.defectedEntity, GetComponent<Entity>().gameObject);

                    neighborPos = GetComponent<PositionDetector>().hittetRegionLast.informationInterfaceMapPositions[0];
                    map.mapTiles[neighborPos.x, neighborPos.y].informations.Add(info);
                }

                // Region Change bei Endnode außerhalb des Generierten Bereiches
                // Idle oder Busy funktioniert nicht da dies erst nach dieser methode gesetzt wird, jedoch der Job chon davor hinzugefügt wird!... Und es treten Bugs auf :O
                if (GetComponent<JobHandling>().jobQueue.Count == 0)
                {
                    gameObject.SetActive(false);
                    gameObject.transform.SetParent(map.mapTiles[neighborPos.x, neighborPos.y].gameObject.transform);
                }
            }
        }
    }
}
