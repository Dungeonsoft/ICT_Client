using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CamDepth : MonoBehaviour {

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.Depth;
    }
}

