using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;

namespace ICT_Engine
{
    public class TestRayScript : MonoBehaviour
    {
        float distance;
        Vector3 pos;

        // Use this for initialization
        void Start()
        {
        }

        private void Update()
        {
            Plane zeroPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (zeroPlane.Raycast(ray, out distance))
            {
                pos = ray.origin + ray.direction * distance;
            }
        }
    }
}
