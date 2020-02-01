using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // IMPORTANT  - face0 = faces[0]
    public Tile[] tiles;
    public float rotationSpeed = 200f;


    private Tile _currentTile;
    private CubeController _cubeController;

    private bool Rotating { get; set; } = false;

    public Vector3 _transformForward;
    public Vector3 _transformUp;
    public Vector3 _transformRight;
    public Quaternion TransformRotation;
    public Vector3 _transformRotationEulerAngles;

    private TileState _previousTileState;
    private Vector3 PLAYER_TILE_POSITION = new Vector3(0, 0, 0);
    private Vector3 LEFT_TILE_POSITION = new Vector3(0, 0, 1.5f);
    private Vector3 RIGHT_TILE_POSITION = new Vector3(1.5f, 0, 0);

    private Vector3 DISAPPEARING_RIGHT_TILE_POSITION = new Vector3(0, 0, -10f);
    private Vector3 DISAPPEARING_LEFT_TILE_POSITION = new Vector3(-10, 0, 0f);
    private Vector3 APPEARING_RIGHT_TILE_POSITION = new Vector3(10, 0, 0);
    private Vector3 APPEARING_LEFT_TILE_POSITION = new Vector3(0, 0, 10f);

    private Vector3 OUT_OF_SIGHT = new Vector3(0, 0, -100F);

    public class TileState
    {
        public Tile Player { get; }
        public Tile Right { get; }
        public Tile Left { get; }

        public TileState(Tile player, Tile right, Tile left)
        {
            this.Player = player;
            this.Right = right;
            this.Left = left;
        }
    }

    void Start()
    {
        _cubeController = GetComponentInChildren<CubeController>();
        foreach (var tile in tiles)
        {
            tile.gameObject.transform.position = OUT_OF_SIGHT;
        }
        var tileState = MapCubeRotationToTilesVisible();
        var playerGameObject = tileState.Player.gameObject;
        playerGameObject.SetActive(true);
        var leftGameObject = tileState.Left.gameObject;
        leftGameObject.SetActive(true);
        var rightGameObject = tileState.Right.gameObject;
        rightGameObject.SetActive(true);

        playerGameObject.transform.position = PLAYER_TILE_POSITION;
        leftGameObject.transform.position = LEFT_TILE_POSITION;
        rightGameObject.transform.position = RIGHT_TILE_POSITION;

        _previousTileState = tileState;
    }


    // Update is called once per frame
    void Update()
    {
        if (!Rotating)
        {
            ControlRotateCube();
        }
    }

    private void TransitionAllTiles()
    {
        var tileState = MapCubeRotationToTilesVisible();
        TransitionToNewTileState(_previousTileState, tileState);
        _previousTileState = tileState;
    }

    private void TransitionToNewTileState(TileState previousTileState, TileState newTileState)
    {
        if (previousTileState.Left == newTileState.Left)
        {
            TransitionRightMovement(previousTileState, newTileState);
        }
        else if (previousTileState.Right == newTileState.Right)
        {
            TransitionLeftMovement(previousTileState, newTileState);
        }
    }

    private void TransitionLeftMovement(TileState previousTileState, TileState newTileState)
    {
        var previousPlayerPosition = previousTileState.Player.transform.position;
        var playerPosition = newTileState.Player.transform.position;
        StartCoroutine(
            TransitionTileThatDisappears(
                previousTileState.Player.transform,
                previousPlayerPosition,
                DISAPPEARING_RIGHT_TILE_POSITION,
                OUT_OF_SIGHT));
        StartCoroutine(
            TransitionTile(
                newTileState.Player.transform,
                playerPosition,
                PLAYER_TILE_POSITION));
        StartCoroutine(
            TransitionTile(
                newTileState.Left.transform,
                APPEARING_LEFT_TILE_POSITION,
                LEFT_TILE_POSITION));
    }

    private void TransitionRightMovement(TileState previousTileState, TileState newTileState)
    {
        var previousPlayerPosition = previousTileState.Player.transform.position;
        var playerPosition = newTileState.Player.transform.position;
        StartCoroutine(
            TransitionTileThatDisappears(
                previousTileState.Player.transform,
                previousPlayerPosition,
                DISAPPEARING_LEFT_TILE_POSITION,
                OUT_OF_SIGHT));
        StartCoroutine(
            TransitionTile(
                newTileState.Player.transform,
                playerPosition,
                PLAYER_TILE_POSITION));
        StartCoroutine(
            TransitionTile(
                newTileState.Right.transform,
                APPEARING_RIGHT_TILE_POSITION,
                RIGHT_TILE_POSITION));
    }

    IEnumerator TransitionTileThatDisappears(Transform transform, Vector3 original, Vector3 newPosition, Vector3 disappearingPosition)
    {
        yield return TransitionTile(transform, original, newPosition);
        transform.position = disappearingPosition;
    }

    IEnumerator TransitionTile(Transform transform, Vector3 original, Vector3 newPosition)
    {
        var totalTime = 0.5f;
        var timeLeft = totalTime;

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            transform.position = Vector3.Lerp(original, newPosition, 1 - (timeLeft / totalTime));
            yield return new WaitForEndOfFrame();
        }
        Rotating = false;
    }

    // If you want the right tiles, talk to ME
    public TileState MapCubeRotationToTilesVisible()
    {
        var vector3s = new List<Vector3>();
        _transformForward = _cubeController.transform.forward;
        var _transformBack = -_transformForward;
        _transformUp = _cubeController.transform.up;
        var _transformDown = -_transformUp;
        _transformRight = _cubeController.transform.right;
        var _transformLeft = -_transformRight;

        vector3s.Add(_transformRight);
        vector3s.Add(_transformUp);
        vector3s.Add(_transformLeft);
        vector3s.Add(_transformDown);
        vector3s.Add(_transformBack);
        vector3s.Add(_transformForward);

        Tile playerTile = null;
        Tile leftTile = null;
        Tile rightTile = null;

        var playerSquare = new Vector3(0, 1, 0);
        var playerLeft = new Vector3(0, 0, 1);
        var playerRight = new Vector3(1, 0, 0);
        for (int i = 0; i < 6; i++)
        {
            if (Vector3.Distance(playerSquare, vector3s[i]) < 0.01f)
            {
                playerTile = tiles[i];
            }

            if (Vector3.Distance(playerLeft, vector3s[i]) < 0.01f)
            {
                leftTile = tiles[i];
            }

            if (Vector3.Distance(playerRight, vector3s[i]) < 0.01f)
            {
                rightTile = tiles[i];
            }
        }

        Debug.DrawRay(_cubeController.transform.position, _transformUp * 5, Color.blue);
        Debug.DrawRay(_cubeController.transform.position, _transformForward * 5, Color.red);
        Debug.DrawRay(_cubeController.transform.position, _transformRight * 5, Color.green);
        TransformRotation = _cubeController.transform.rotation;
        _transformRotationEulerAngles = TransformRotation.eulerAngles;

        if (playerTile == null || rightTile == null || leftTile == null)
        {
            throw new Exception("WTF!?!?!?!?! yOuR tILE iS nUlL?!?!?!?!");
        }

        return new TileState(playerTile, rightTile, leftTile);
    }

    private void ControlRotateCube()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(RotateCube(new Vector3(-1, 0, 0)));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(RotateCube(new Vector3(0, 0, 1)));
        }
    }

    private IEnumerator RotateCube(Vector3 rotation)
    {
        Rotating = true;
        var angleLeft = 90f;
        while (angleLeft > 0)
        {
            var angleThisFrame = Time.deltaTime * rotationSpeed;
            angleLeft -= angleThisFrame;
            if (angleLeft < 0)
            {
                angleThisFrame += angleLeft;
            }

            _cubeController.transform.Rotate(angleThisFrame * rotation.x, angleThisFrame * rotation.y,
                angleThisFrame * rotation.z, Space.World);

            yield return new WaitForEndOfFrame();
        }

        TransitionAllTiles();
    }
}