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
        transform.position = new Vector3(pos.x + 1f, pos.y + 0.75f, pos.z);
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
        var angleThisFrame = dir < 0 ? 30 : -30;
        var rotation = transform.rotation;
        transform.Rotate(angleThisFrame * rotation.x, angleThisFrame * rotation.y,
            angleThisFrame * rotation.z, Space.World);
    }
}