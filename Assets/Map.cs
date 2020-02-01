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
    private bool _rotating = false;

    public Vector3 _transformForward;
    public Vector3 _transformUp;
    public Vector3 _transformRight;

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
        MapCubeRotationToTilesVisible();
    }


    // Update is called once per frame
    void Update()
    {
        if (!_rotating)
        {
            ControlRotateCube();
        }

        ExampleOfHowMapCubeEtcWorks();
    }

    private void ExampleOfHowMapCubeEtcWorks()
    {
        var tileState = MapCubeRotationToTilesVisible();
        foreach (var tile in tiles)
        {
            tile.gameObject.SetActive(false);
        }

        tileState.Player.gameObject.SetActive(true);
        tileState.Right.gameObject.SetActive(true);
        tileState.Left.gameObject.SetActive(true);
        tileState.Player.gameObject.transform.position = new Vector3(0, 0, 0);
        tileState.Right.gameObject.transform.position = new Vector3(0, 0, 1.5f);
        tileState.Left.gameObject.transform.position = new Vector3(1.5f, 0, 0);

        rotateY(tileState.Right.gameObject, tileState.RightRotation);
        rotateY(tileState.Left.gameObject, tileState.LeftRotation);
    }

    public void rotateY(GameObject gameObject, float angle)
    {
        gameObject.transform.Rotate(0, gameObject.transform.rotation.eulerAngles.y - angle, 0);
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

        // 0, 1, 0 - player is on this one
        // 0, 0, 1 - left
        // 1, 0, 0 - right

        Debug.DrawRay(_cubeController.transform.position, _transformUp * 5, Color.blue);
        Debug.DrawRay(_cubeController.transform.position, _transformForward * 5, Color.red);
        Debug.DrawRay(_cubeController.transform.position, _transformRight * 5, Color.green);

        if (playerTile == null || rightTile == null || leftTile == null)
        {
            throw new Exception("WTF!?!?!?!?! yOu'rE tILE iS nUlL?!?!?!?!");
        }

        return new TileState(playerTile, rightTile, leftTile, rightRotation,
            leftRotation);
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

    IEnumerator RotateCube(Vector3 rotation)
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