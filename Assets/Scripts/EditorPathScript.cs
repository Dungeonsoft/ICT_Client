using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;

namespace ICT_Engine
{
    public class EditorPathScript : MonoBehaviour
    {
        #region Variables
        public Color rayColor = Color.magenta;
        //[System.NonSerialized]
        public List<Transform> pathObjs = new List<Transform>();
        public Transform[] theArray;

        public NextCharcterMotion startMotion = NextCharcterMotion.Walking_01;
        public NextCharcterMotion endMotion = NextCharcterMotion.Idle_01;

        [Header("초당 0.695f 미터로 이동시 시속 약 2.5km가 나온다: Slow")]
        [Header("초당 1.67f 미터로 이동시 시속 약 6km가 나온다: Normal")]
        [Header("초당 1.11f 미터로 이동시 시속 약 4km가 나온다: HIgh")]
        public CharacterSpeed moveSpeed = CharacterSpeed.normal;
        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = rayColor;
            pathObjs.Clear();

            SetGizmo();
        }

        private void Awake()
        {
            pathObjs.Clear();
            foreach (Transform pathObj in theArray)
            {
                if (pathObj != this.transform)
                {
                    pathObjs.Add(pathObj);
                }
            }
        }

        public void SetGizmo()
        {
            theArray = GetComponentsInChildren<Transform>();

            foreach (Transform pathObj in theArray)
            {
                if (pathObj != this.transform)
                {
                    pathObjs.Add(pathObj);
                }
            }

            for (int i = 0; i < pathObjs.Count; i++)
            {
                Vector3 position = pathObjs[i].position;
                if (i > 0)
                {
                    Vector3 previous = pathObjs[i - 1].position;
                    Gizmos.DrawLine(previous, position);
                    Gizmos.DrawSphere(position, 0.05f);
                }
            }

        }
    }
}