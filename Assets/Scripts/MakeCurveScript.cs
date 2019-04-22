using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeCurveScript : MonoBehaviour
{
    public bool showGizmo;
    public Color gColor;
    public Transform[] pathObjs;

    void OnDrawGizmos()
    {
        if (showGizmo == true)
        {
            Gizmos.color = gColor;
            pathObjs = GetComponentsInChildren<Transform>();
            int len = pathObjs.Length;
            for (int i = 1; i < len; i++)
            {
                Gizmos.DrawSphere(pathObjs[i].position, 0.2f);
                Gizmos.DrawLine(pathObjs[i].position, pathObjs[(i + 1) != len ? i + 1 : i].position);
            }
        }
    }

    public void SetGizmo()
    {
                    Gizmos.color = gColor;
            pathObjs = GetComponentsInChildren<Transform>();
            int len = pathObjs.Length;
            for (int i = 1; i < len; i++)
            {
                Gizmos.DrawSphere(pathObjs[i].position, 0.2f);
                Gizmos.DrawLine(pathObjs[i].position, pathObjs[(i + 1) != len ? i + 1 : i].position);
            }

    }
}
