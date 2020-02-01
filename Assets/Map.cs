using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update
    private Face[] _faces;
    private Face _currentFace;
    private Face _face0;
    private Face _face1;
    private Face _face2;
    private Face _face3;
    private Face _face4;
    private Face _face5;

    void Start()
    {
        _faces = GetComponentsInChildren<Face>();
        _faces.Where(face => face.faceNumber == 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
