using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject startingTile;
    public GameObject playerGraphicObject;

    private void Start()
    {
        var pos = startingTile.transform.position;
        transform.position = new Vector3(pos.x, 0.75f, pos.z);
        playerGraphicObject.transform.rotation = Quaternion.Euler(0, -135, 0);
    }
}
