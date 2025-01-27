using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public WorldController world;

    public float speed;
    public enum States
    {
        Idle,
        Moving
    }
    public States state;

    public bool isControllable;

    private void Awake()
    {
        world = FindObjectOfType<WorldController>();
    }
}
