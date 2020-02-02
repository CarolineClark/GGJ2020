using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject playerGraphicObject;

    private void Start()
    {
    }

    public void SetTile(GameObject tile)
    {
        var pos = tile.transform.position;
        transform.position = new Vector3(pos.x + 1f, pos.y + 0.75f, pos.z);
        playerGraphicObject.transform.rotation = Quaternion.Euler(0, -135, 0);
    }
}