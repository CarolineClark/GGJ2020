using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private const string FenceGameObjectName = "fence";
    private const string PortalGameObjectName = "portal";

    public int faceNumber;
    [CanBeNull] public Transform Fence { get; private set; }
    [CanBeNull] public Portal Portal { get; private set; }

    void Start()
    {
        Fence = transform.Find(FenceGameObjectName);
        Portal = transform.Find(PortalGameObjectName).gameObject.GetComponentInChildren<Portal>();
    }
}