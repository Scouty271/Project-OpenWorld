using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDMovement : MonoBehaviour
{
    private Entity entity;
    private PositionDetector posDetector;
    public RegionTileArrayInterface arrayInterface;

    public bool isMoving = false;

    public float moveX;
    public float moveY;

    public KeyCode keyCode1 = new KeyCode();
    public KeyCode keyCode2 = new KeyCode();

    public float y;
    public float x;

    public RegionTile currTile;
    public RegionTile NextTile;

    private void Start()
    {
        entity = GetComponent<Entity>();
        posDetector = GetComponent<PositionDetector>();
        arrayInterface = FindObjectOfType<RegionTileArrayInterface>();

        y = 1;
        x = 1;

        currTile = posDetector.hittetRegionTileCurrent;
    }
    private void Update()
    {
        moveX = Input.GetAxis("HorizontalCharacter");
        moveY = Input.GetAxis("VerticalCharacter");

        handleMoving(KeyCode.W, new Vector2Int(0, 1), true, false, false, false);
        handleMoving(KeyCode.S, new Vector2Int(0, -1), false, true, false, false);
        handleMoving(KeyCode.D, new Vector2Int(1, 0), false, false, false, true);
        handleMoving(KeyCode.A, new Vector2Int(-1, 0), false, false, true, false);

        handleMoving(KeyCode.W, KeyCode.A, new Vector2Int(0, 1), new Vector2Int(-1, 1), true, false, false, false);
        handleMoving(KeyCode.S, KeyCode.D, new Vector2Int(0, -1), new Vector2Int(1, -1), false, true, false, false);
        handleMoving(KeyCode.D, KeyCode.W, new Vector2Int(1, 0), new Vector2Int(1, 1), false, false, false, true);
        handleMoving(KeyCode.A, KeyCode.S, new Vector2Int(-1, 0), new Vector2Int(-1, -1), false, false, true, false);
    }

    private void handleMoving(KeyCode _keyCode1, Vector2Int _direction1, bool up, bool down, bool left, bool right)
    {
        //Normale Richtung
        if (keyCode1 == KeyCode.None && Input.GetKey(_keyCode1))
        {
            keyCode1 = _keyCode1;

            currTile = posDetector.hittetRegionTileCurrent;
            NextTile = arrayInterface.GetRegionTile(new Vector3Int(currTile.worldPosition.x + _direction1.x, currTile.worldPosition.y + _direction1.y, 0));
        }

        //Normale Richtung
        if (keyCode1 == _keyCode1 && keyCode2 == KeyCode.None)
            if (NextTile.GetComponent<Node>().isWalkable)
            {
                entity.GetComponent<AnimationHandling>().setAnimationParameters(right, left, up, down, false);
                entity.state = Entity.States.Moving;
                isMoving = true;

                if (_direction1.x != 0)
                    transform.Translate(_direction1.x * entity.speed, 0, 0);

                if (_direction1.y != 0)
                    transform.Translate(0, _direction1.y * entity.speed, 0);


                if (_direction1.y == 1)
                    if (NextTile.worldPosition.y < transform.position.y)
                        setPropsForIdle();

                if (_direction1.y == -1)
                    if (NextTile.worldPosition.y > transform.position.y)
                        setPropsForIdle();

                if (_direction1.x == 1)
                    if (NextTile.worldPosition.x < transform.position.x)
                        setPropsForIdle();

                if (_direction1.x == -1)
                    if (NextTile.worldPosition.x > transform.position.x)
                        setPropsForIdle();
            }
            else
            {
                keyCode1 = KeyCode.None;
            }
    }

    private void handleMoving(KeyCode _keyCode1, KeyCode _keyCode2, Vector2Int _direction1, Vector2Int _direction2, bool up, bool down, bool left, bool right)
    {
        //Schräge Richtung
        if (keyCode1 == KeyCode.None && keyCode2 == KeyCode.None && Input.GetKey(_keyCode1) && Input.GetKey(_keyCode2))
        {
            keyCode1 = _keyCode1;
            keyCode2 = _keyCode2;

            currTile = posDetector.hittetRegionTileCurrent;
            NextTile = arrayInterface.GetRegionTile(new Vector3Int(currTile.worldPosition.x + _direction2.x, currTile.worldPosition.y + _direction2.y, 0));
        }

        //Schräge Richtung
        if (keyCode1 == _keyCode1 && keyCode2 == _keyCode2)
            if (NextTile.GetComponent<Node>().isWalkable)
            {
                //Baustelle !!! Schräge Richtung ohne Hindernis gehen!
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////
                var anim = entity.GetComponent<Animator>();

                if (anim.GetBool("isMovingUp"))
                {

                }
                entity.GetComponent<AnimationHandling>().setAnimationParameters(right, left, up, down, false);

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////

                entity.state = Entity.States.Moving;
                isMoving = true;

                transform.Translate(_direction2.x * entity.speed / Mathf.Sqrt(2), _direction2.y * entity.speed / Mathf.Sqrt(2), 0);

                if (_direction2.x == 1 && _direction2.y == 1)
                    if (NextTile.worldPosition.x < transform.position.x && NextTile.worldPosition.y < transform.position.y)
                        setPropsForIdle();

                if (_direction2.x == -1 && _direction2.y == 1)
                    if (NextTile.worldPosition.x > transform.position.x && NextTile.worldPosition.y < transform.position.y)
                        setPropsForIdle();

                if (_direction2.x == 1 && _direction2.y == -1)
                    if (NextTile.worldPosition.x < transform.position.x && NextTile.worldPosition.y > transform.position.y)
                        setPropsForIdle();

                if (_direction2.x == -1 && _direction2.y == -1)
                    if (NextTile.worldPosition.x > transform.position.x && NextTile.worldPosition.y > transform.position.y)
                        setPropsForIdle();
            }
            else
            {
                keyCode1 = KeyCode.None;
                keyCode2 = KeyCode.None;
            }
    }

    private void setPropsForIdle()
    {
        if (!Input.GetKey(KeyCode.W) || !Input.GetKey(KeyCode.S) || !Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.D))
        {
            keyCode1 = KeyCode.None;
            keyCode2 = KeyCode.None;
            entity.GetComponent<AnimationHandling>().setAnimationParameters(false, false, false, false, true);
            entity.state = Entity.States.Idle;
            isMoving = false;
            transform.position = new Vector3(NextTile.worldPosition.x, NextTile.worldPosition.y, this.transform.position.z);
        }
    }

}
