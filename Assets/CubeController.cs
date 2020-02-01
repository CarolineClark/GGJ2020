﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float rotationSpeed = 100f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(RotateCube(new Vector3(1, 0, 0)));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(RotateCube(new Vector3(0, 0, 1)));
        }
    }

    IEnumerator RotateCube(Vector3 rotation)
    {
        var angleLeft = 90f;
        while (angleLeft > 0)
        {
            var angleThisFrame = Time.deltaTime * rotationSpeed;
            angleLeft -= angleThisFrame;
            if (angleLeft < 0)
            {
                angleThisFrame += angleLeft;
            }

            // transform.Rotate(angleThisFrame * rotation.x, angleThisFrame * rotation.y, angleThisFrame * rotation.z);
            // transform.Rotate(transform.forward, angleThisFrame);
            // transform.rotation = Quaternion.Euler(angleThisFrame * rotation);

            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.Euler(90 * rotation);

        Debug.Log(transform.rotation.y);
    }
}