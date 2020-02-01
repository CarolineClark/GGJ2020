using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private const string FenceGameObjectName = "fence";

    public int faceNumber;
    [CanBeNull] public Transform Fence { get; private set; }

    void Start()
    {
        Fence = transform.Find(FenceGameObjectName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
