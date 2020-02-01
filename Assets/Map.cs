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

    void Start()
    {
        _cubeController = GetComponentInChildren<CubeController>();
    }



    // Update is called once per frame
    void Update()
    {
        if (!_rotating)
        {
            ControlRotateCube();
        }
    }

    private Tile[] MapCubeRotationToTilesVisible()
    {
        return new Tile[]{};
    }

    private void ControlRotateCube()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(RotateCube(new Vector3(1, 0, 0)));
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(RotateCube(new Vector3(0, 0, -1)));
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
            if (angleLeft < 0) {
                angleThisFrame += angleLeft;
            }

            _cubeController.transform.Rotate(angleThisFrame * rotation.x, angleThisFrame * rotation.y, angleThisFrame * rotation.z, Space.World);
            // transform.Rotate(transform.forward, angleThisFrame);
            // transform.rotation = Quaternion.Euler(angleThisFrame * rotation);

            yield return new WaitForEndOfFrame();
        }
        // transform.rotation = Quaternion.Euler(90 * rotation);

        _rotating = false;
    }
}
