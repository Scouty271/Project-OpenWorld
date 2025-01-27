using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public WorldController world;
    public CameraController camController;

    public enum State
    {
        gameState,
        mapState,
        inventoryState,
        menueState
    }
    public State state;

    public Canvas gameCanvas;
    public Canvas mapCanvas;
    public Canvas inventoryCanvas;
    public Canvas menueCanvas;

    private float camSizeBackup;
    private Vector3 camPositionBackup;

    public void activateMapState()
    {
        state = State.mapState;

        world.map.gameObject.SetActive(true);
        mapCanvas.gameObject.SetActive(true);
    }
    public void deactivateMapState()
    {
        world.map.gameObject.SetActive(false);
        mapCanvas.gameObject.SetActive(false);
    }
    public void activateGameState()
    {
        state = State.gameState;
        world.regionController.gameObject.SetActive(true);
        camController.GetComponent<Camera>().orthographicSize = camSizeBackup;
        camController.gameObject.transform.position = camPositionBackup;
        gameCanvas.gameObject.SetActive(true);
    }
    public void deactivateGameState()
    {
        world.regionController.gameObject.SetActive(false);
        camSizeBackup = camController.GetComponent<Camera>().orthographicSize;
        camPositionBackup = camController.gameObject.transform.position;
        gameCanvas.gameObject.SetActive(false);
    }
    public void activateInventoryState()
    {
        state = State.inventoryState;
        inventoryCanvas.gameObject.SetActive(true);
        inventoryCanvas.GetComponent<InventoryCanvas>().refreshInventory();
    }
    public void deactivateInventoryState()
    {
        inventoryCanvas.gameObject.SetActive(false);
    }
    public void activateMenueState()
    {
        state = State.menueState;
        menueCanvas.gameObject.SetActive(true);
    }
    public void deactivateMenueState()
    {
        menueCanvas.gameObject.SetActive(false);
    }
}
