using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public GameController game;
    private StateController stateController;

    private void Start()
    {
        stateController = game.stateController;
    }

    public void onClickMapOpen()
    {
        stateController.deactivateGameState();
        stateController.activateMapState();
    }

    public void onClickMapReturn()
    {
        stateController.deactivateMapState();
        stateController.activateGameState();
    }

    public void onClickInventoryOpen()
    {
        stateController.deactivateGameState();
        stateController.activateInventoryState();
    }

    public void onClickInventoryReturn()
    {
        stateController.deactivateInventoryState();
        stateController.activateGameState();
    }

    public void onClickMenue()
    {
        stateController.deactivateGameState();
        stateController.activateMenueState();
    }
    public void onClickMenueReturn()
    {
        stateController.deactivateMenueState();
        stateController.activateGameState();
    }

    public void onClickMenueEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
