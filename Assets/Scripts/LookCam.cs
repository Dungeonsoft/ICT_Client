using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;

namespace ICT_Engine
{
    public class LookCam : MonoBehaviour
    {

        Transform cam;
        // Use this for initialization
        void Start()
        {
            try
            {
                cam = GameObject.FindWithTag("MainCam").transform;
            }
            catch
            { }
        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(cam);
        }
    }
}