using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject playerGraphicObject;
    private Animator _animator;
    private static readonly int Walking = Animator.StringToHash("walking");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetTile(GameObject tile)
    {
        var pos = tile.transform.position;
        transform.position = new Vector3(pos.x - 0.2f, pos.y + 0.1f, pos.z - 0.3f);
        playerGraphicObject.transform.rotation = Quaternion.Euler(0, -135, 0);
    }

    public void SetWalkingAnimation()
    {
        _animator.SetBool(Walking, true);
    }

    public void SetIdleAnimation()
    {
        _animator.SetBool(Walking, false);
    }

    public void FlipPlayer(int dir)
    {
        var rotation = dir < 0 ?  new Vector3(0, 90, 0) : new Vector3(0, 0, 0);
        transform.rotation = Quaternion.Euler(rotation);
        // transform.Rotate(rotation, Space.World);
    }
}