using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGroupMirror : MonoBehaviour
{
    public GameObject Target;
    private GameObject _copy;

    void Start()
    {
        _copy = Instantiate(Target);
        _copy.transform.parent = this.transform;
        _copy.transform.localScale = new Vector3(Target.transform.localScale.x, -1, Target.transform.localScale.y);
        _copy.transform.localPosition = new Vector3(Target.transform.localPosition.x, -1, Target.transform.localPosition.y);
        UpdateMirror();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMirror();
    }

    public void UpdateMirror() {
        for (int i = 0, n = Target.transform.childCount; i < n; i++) {
            var toReflect = Target.transform.GetChild(i);
            var reflection = _copy.transform.GetChild(i);
            reflection.transform.localPosition = toReflect.transform.localPosition;
            reflection.transform.localRotation = toReflect.transform.localRotation;
        }
    }
}
