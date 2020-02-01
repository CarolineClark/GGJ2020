using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.WSA;
using Debug = UnityEngine.Debug;

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
    private Vector3 LEFT_TILE_POSITION = new Vector3(1.5f, 0, 0);
    private Vector3 RIGHT_TILE_POSITION = new Vector3(0, 0, 1.5f);

    private Vector3 DISAPPEARING_RIGHT_TILE_POSITION = new Vector3(-10, 0, 0);
    private Vector3 DISAPPEARING_LEFT_TILE_POSITION = new Vector3(0, 0, -10f);
    private Vector3 APPEARING_RIGHT_TILE_POSITION = new Vector3(0, 0, 10f);
    private Vector3 APPEARING_LEFT_TILE_POSITION = new Vector3(10, 0, 0);

    private Vector3 OUT_OF_SIGHT = new Vector3(0, 0, -100F);
    private float EPSILON = 1f;

    public class TileState
    {
        public Tile Player { get; }
        public Tile Right { get; }
        public Tile Left { get; }

        public float RightRotation { get; }
        public float LeftRotation { get; }

        public TileState(Tile player, Tile right, Tile left, float rightRotation,
            float leftRotation)
        {
            this.Player = player;
            this.Right = right;
            this.Left = left;
            this.RightRotation = rightRotation;
            this.LeftRotation = leftRotation;
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

    private bool FenceInTheWay(PlayerInput playerInput)
    {
        var playerInputLeft = playerInput.Left;
        var fenceLeft = _previousTileState.Left.Fence;
        var playerInputRight = playerInput.Right;
        var fenceRight = _previousTileState.Right.Fence;
        return CheckIfInputForcesPlayerIntoFence(playerInputLeft, fenceLeft) ||
               CheckIfInputForcesPlayerIntoFence(playerInputRight, fenceRight);
    }

    private bool CheckIfInputForcesPlayerIntoFence(bool playerInputDir, Transform fence)
    {
        if (playerInputDir && fence != null)
        {
            // check the fence is close. Within an expected value?
            var distance = Vector3.Distance(
                fence.transform.position,
                _previousTileState.Player.transform.position);
            Debug.Log(distance);
            if (distance < EPSILON)
            {
                return true;
            }
        }

        return false;
    }


    // Update is called once per frame
    void Update()
    {
        if (!Rotating)
        {
            PlayerInput playerInput = new PlayerInput(Input.GetKeyDown(KeyCode.LeftArrow), Input.GetKeyDown(KeyCode.RightArrow));
            if (!FenceInTheWay(playerInput))
            {
                ControlRotateCube(playerInput);
            }
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

        rotateY(newTileState.Right.gameObject, newTileState.RightRotation);
        rotateY(newTileState.Left.gameObject, newTileState.LeftRotation);
    }

    public void rotateY(GameObject gameObject, float angle)
    {
        gameObject.transform.Rotate(0, gameObject.transform.rotation.eulerAngles.y - angle, 0);
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
        var normalVectors = new List<Vector3>();
        _transformForward = _cubeController.transform.forward;
        var _transformBack = -_transformForward;
        _transformUp = _cubeController.transform.up;
        var _transformDown = -_transformUp;
        _transformRight = _cubeController.transform.right;
        var _transformLeft = -_transformRight;

        normalVectors.Add(_transformRight);
        normalVectors.Add(_transformUp);
        normalVectors.Add(_transformLeft);
        normalVectors.Add(_transformDown);
        normalVectors.Add(_transformBack);
        normalVectors.Add(_transformForward);

        var upVectors = new List<Vector3>();
        upVectors.Add(_transformBack);
        upVectors.Add(_transformBack);
        upVectors.Add(_transformBack);
        upVectors.Add(_transformBack);
        upVectors.Add(_transformDown);
        upVectors.Add(_transformUp);

        Tile playerTile = null;
        Tile leftTile = null;
        Tile rightTile = null;

        var playerSquare = new Vector3(0, 1, 0);
        var playerLeft = new Vector3(1, 0, 0);
        var playerRight = new Vector3(0, 0, 1);

        var leftRotation = 0f;
        var rightRotation = 0f;
        for (int i = 0; i < 6; i++)
        {
            if (Vector3.Distance(playerSquare, normalVectors[i]) < 0.01f)
            {
                // No Needs For The Changaroo
                playerTile = tiles[i];
            }

            if (Vector3.Distance(playerLeft, normalVectors[i]) < 0.01f)
            {
                leftTile = tiles[i];
                Vector3 faceUp = upVectors[i];
                leftRotation = Vector3.SignedAngle(faceUp,
                                   Vector3.forward,
                                   Vector3.left) + 180f;
            }

            if (Vector3.Distance(playerRight, normalVectors[i]) < 0.01f)
            {
                rightTile = tiles[i];
                Vector3 faceUp = upVectors[i];
                rightRotation = Vector3.SignedAngle(faceUp,
                                    Vector3.left,
                                    Vector3.back) + 90f;
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

        return new TileState(playerTile, rightTile, leftTile, rightRotation,
            leftRotation);
    }

    private void ControlRotateCube(PlayerInput playerInput)
    {
        if (playerInput.Right)
        {
            StartCoroutine(RotateCube(new Vector3(-1, 0, 0)));
        }
        else if (playerInput.Left)
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