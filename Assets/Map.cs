using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{
    // IMPORTANT  - face0 = faces[0]
    public Tile[] tiles;
    public float rotationSpeed = 100f;


    private Tile _currentTile;
    private CubeController _cubeController;
    private bool _rotating = false;

    public Vector3 _transformForward;
    public Vector3 _transformUp;
    public Vector3 _transformRight;
    public Quaternion TransformRotation;
    public Vector3 _transformRotationEulerAngles;

    private TileState _previousTileState;

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
        MapCubeRotationToTilesVisible();
    }


    // Update is called once per frame
    void Update()
    {
        if (!_rotating)
        {
            ControlRotateCube();
            ExampleOfHowMapCubeEtcWorks();
        }
    }

    private void ExampleOfHowMapCubeEtcWorks()
    {
        var tileState = MapCubeRotationToTilesVisible();
        foreach (var tile in tiles)
        {
            tile.gameObject.SetActive(false);
        }

        var playerGameObject = tileState.Player.gameObject;
        playerGameObject.SetActive(true);
        var leftGameObject = tileState.Left.gameObject;
        leftGameObject.SetActive(true);
        var rightGameObject = tileState.Right.gameObject;
        rightGameObject.SetActive(true);

        playerGameObject.transform.position = new Vector3(0, 0, 0);
        leftGameObject.transform.position = new Vector3(0, 0, 1.5f);
        rightGameObject.transform.position = new Vector3(1.5f, 0, 0);

        _previousTileState = tileState;
    }

    private void TransitionTileMovement(TileState previousTileState, TileState newTileState)
    {
        if (previousTileState.Left == newTileState.Left)
        {
            // then the player is moving right
            // lerp the old player tile going left and back
            // lerp the old right to player
            // lerp the new right out of range to the new right
            // rotate the old left
        }
        else if (previousTileState.Right == newTileState.Right)
        {
            
        }
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

        // 0, 1, 0 - player is on this one
        // 0, 0, 1 - left
        // 1, 0, 0 - right

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

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(RotateCube(new Vector3(0, 0, 1)));
        }
    }

    private IEnumerator RotateCube(Vector3 rotation)
    {
        _rotating = true;
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
            // transform.Rotate(transform.forward, angleThisFrame);
            // transform.rotation = Quaternion.Euler(angleThisFrame * rotation);

            yield return new WaitForEndOfFrame();
        }
        // transform.rotation = Quaternion.Euler(90 * rotation);

        _rotating = false;
    }
}