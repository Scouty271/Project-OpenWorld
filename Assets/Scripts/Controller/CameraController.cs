using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameController game;

    public float moveSpeed;
    public float dragSpeed;

    private int currentZoomlevel = 2;
    private float[] zoomlevels = new float[] { 2f, 4f, 8f, 16f, 32f, 64f };

    // for map drag
    private Vector3 oldMousePosition;
    bool doDrag = false;

    public bool fixedOnPlayer;

    public float x;
    public float y;

    private void Start()
    {
        oldMousePosition = Input.mousePosition;
    }
    private void Update()
    {
        HandleZoom();
        HandleDrag();

        if (game.stateController.state == StateController.State.gameState)
        {
            if (fixedOnPlayer)
                HandleCamFixedOnPlayer();

            if (!fixedOnPlayer)
                HandleMove();
        }
        if (game.stateController.state == StateController.State.mapState)
            setCamOnMap();
    }
    private void HandleMove()
    {
        x = Input.GetAxis("HorizontalCam");
        y = Input.GetAxis("VerticalCam");
        transform.Translate(new Vector3(x * moveSpeed, y * moveSpeed, 0));
    }
    private void HandleZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            currentZoomlevel--;
            if (currentZoomlevel < 0) currentZoomlevel = 0;
            Camera.main.orthographicSize = zoomlevels[currentZoomlevel];
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            currentZoomlevel++;
            if (currentZoomlevel >= zoomlevels.Length) currentZoomlevel = zoomlevels.Length - 1;
            Camera.main.orthographicSize = zoomlevels[currentZoomlevel];
        }
    }
    private void HandleDrag()
    {
        if (Input.GetMouseButtonDown(2))
        {
            doDrag = true;
            oldMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(2))
        {
            doDrag = false;
        }
        if (doDrag)
        {
            gameObject.transform.position += (oldMousePosition - Input.mousePosition) * dragSpeed;
            oldMousePosition = Input.mousePosition;
        }
    }
    private void HandleCamFixedOnObject(GameObject gameObject)
    {
        var posGameObject = gameObject.transform.position;
        this.transform.position = new Vector3(posGameObject.x, posGameObject.y, this.transform.position.z);
    }
    public void HandleCamFixedOnPlayer()
    {
        HandleCamFixedOnObject(game.world.mainCharacter.gameObject);
    }

    public void setCamOnMap()
    {
        this.transform.position = new Vector3(game.world.map.transform.position.x + game.world.getWorldSize() / 2, game.world.map.transform.position.y + game.world.getWorldSize() / 2, -10);
        this.GetComponent<Camera>().orthographicSize = 64;
    }
}