using UnityEngine;
using System;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public WorldController world;
    public CameraController camController;
    public StateController stateController;

    private void Start()
    {
        Application.targetFrameRate = 300;
    }
}
