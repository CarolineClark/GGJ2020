using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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
        var portalParent = transform.Find(PortalGameObjectName);
        if (portalParent) {
            Portal = portalParent.gameObject.GetComponentInChildren<Portal>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}