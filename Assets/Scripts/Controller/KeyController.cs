using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public GameController game;
    private StateController stateController;

    private void Start()
    {
        stateController = game.stateController;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.M) && stateController.state == StateController.State.gameState)
        {
            stateController.deactivateGameState();
            stateController.activateMapState();
        }

        if (Input.GetKey(KeyCode.Escape) && stateController.state == StateController.State.mapState)
        {
            stateController.deactivateMapState();
            stateController.activateGameState();
        }

        if (Input.GetKeyDown(KeyCode.X) && stateController.state == StateController.State.gameState && stateController.camController.fixedOnPlayer)
            stateController.camController.fixedOnPlayer = false;
        else if (Input.GetKeyDown(KeyCode.X) && stateController.state == StateController.State.gameState && !stateController.camController.fixedOnPlayer)
            stateController.camController.fixedOnPlayer = true;


        if (Input.GetKeyDown(KeyCode.Space) && !game.world.timeStopped)
        {
            game.world.timeStopped = true;
            game.world.time = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && game.world.timeStopped)
        {
            game.world.timeStopped = false;
            game.world.time = 1;
        }
    }
}
