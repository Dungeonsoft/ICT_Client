using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;

namespace ICT_Engine
{
    public class PointGizmo : MonoBehaviour
    {

        public string baseAreaName;
        public float gSize = 0.35f;
        Vector3 p0;
        Vector3 p1;
        Vector3 p2;


        public Vector3 GizmoSize
        {
            get
            {
                return new Vector3(gSize, gSize, gSize);
            }
        }

        public Vector3 GizmoPos
        {
            get
            {
                //Plane Plane = new Plane()

                RaycastHit hit;
                Vector3 dir = Vector3.down;

                if (Physics.Raycast(this.transform.position, dir, out hit, Mathf.Infinity))
                {
                    //어떤_오브젝트_위에_있는지_이름을_추출한다//
                    baseAreaName = hit.transform.name;
                    //1차_콜라이더_추출//
                    MeshFilter mCol = hit.collider.GetComponent<MeshFilter>();
                    //2차_콜라이더_메쉬화//
                    Mesh mesh = mCol.sharedMesh;
                    //메쉬_버텍스벡터_정보들을_어레이로_저장//
                    Vector3[] mVertices = mesh.vertices;
                    //메쉬의_트라이앵글_번호를_어레이로_저장//
                    int[] mTriangles = mesh.triangles;

                    //Debug.Log("Hit TriAngle Index: "+ (hit.triangleIndex+3));

                    //기즈모_아래에_위치한_오브젝트의_트랜스폼값을_따로_저장한다//
                    Vector3 pos = hit.transform.position;
                    Vector3 sc = hit.transform.localScale;
                    Vector3 rot = hit.transform.eulerAngles;

                    //포인트의_원시값_(위치_회전_스케일_모두적용되지_않은_상태)을_찾아_저장한다//
                    p0 = mVertices[mTriangles[(hit.triangleIndex) * 3 + 0]];
                    p1 = mVertices[mTriangles[(hit.triangleIndex) * 3 + 1]];
                    p2 = mVertices[mTriangles[(hit.triangleIndex) * 3 + 2]];

                    //스케일을_적용한다//
                    p0 = new Vector3(p0.x * sc.x, p0.y * sc.y, p0.z * sc.z);
                    p1 = new Vector3(p1.x * sc.x, p1.y * sc.y, p1.z * sc.z);
                    p2 = new Vector3(p2.x * sc.x, p2.y * sc.y, p2.z * sc.z);

                    //회전값을_적용한다//
                    p0 = RotVector3(p0, rot);
                    p1 = RotVector3(p1, rot);
                    p2 = RotVector3(p2, rot);

                    //위치값을_적용한다//
                    p0 += pos;
                    p1 += pos;
                    p2 += pos;

                    //추출된_세개의_위치값을_이용하여_가상의_플랜을_생성한다//
                    Plane hitPlane = new Plane(p0, p1, p2);

                    float hightY = hitPlane.GetDistanceToPoint(this.transform.position);
                    //Debug.Log("Hight Y: "+ hightY);

                    Vector3 outVec = new Vector3(transform.position.x, transform.position.y - hightY, transform.position.z);

                    Debug.DrawLine(this.transform.position, outVec);
                    //Debug.DrawLine(p0, p1);
                    //Debug.DrawLine(p1, p2);
                    //Debug.DrawLine(p2, p0);

                    //Gizmos.DrawSphere(p0, 0.1f);
                    //Gizmos.DrawSphere(p1, 0.1f);
                    //Gizmos.DrawSphere(p2, 0.1f);

                    return new Vector3(transform.position.x, transform.position.y - hightY, transform.position.z);
                }
                else
                {
                    return transform.position;
                }
            }

        }

        /// <summary>
        /// _버텍스를(p)_원점을기준으로_지정된(r)_라디안만큼_회전시킨다///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        Vector3 RotVector3(Vector3 p, Vector3 r)
        {
            float radX = r.x * Mathf.Deg2Rad;
            float radY = r.y * Mathf.Deg2Rad;
            float radZ = r.z * Mathf.Deg2Rad;
            float sinX = Mathf.Sin(radX);
            float cosX = Mathf.Cos(radX);
            float sinY = Mathf.Sin(radY);
            float cosY = Mathf.Cos(radY);
            float sinZ = Mathf.Sin(radZ);
            float cosZ = Mathf.Cos(radZ);

            //Vector3 xAxis = new Vector3(
            //    cosY * cosZ,
            //    cosX * sinZ + sinX * sinY * cosZ,
            //    sinX * sinZ - cosX * sinY * cosZ
            //);
            //Vector3 yAxis = new Vector3(
            //    -cosY * sinZ,
            //    cosX * cosZ - sinX * sinY * sinZ,
            //    sinX * cosZ + cosX * sinY * sinZ
            //);
            //Vector3 zAxis = new Vector3(
            //    sinY,
            //    -sinX * cosY,
            //    cosX * cosY
            //);

            //return xAxis * p.x + yAxis * p.y + zAxis * p.z;


            ////////////////////////////////////////////////////////////////
            //x축_회전//
            p = new Vector3(
                p.x,
                p.y * cosX + p.z * -sinX,
                p.y * sinX + p.z * cosX
                );

            //y축_회전//
            p = new Vector3(
                p.x * cosY + p.z * sinY,
                p.y,
                p.x * -sinY + p.z * cosY
                );

            //z축_회전//
            p = new Vector3(
                p.x * cosZ + p.y * -sinZ,
                p.x * sinZ + p.y * cosZ,
                p.z
                );

            return p;
        }

        //public bool isCreate;
        //Vector3 gizmoPosGet;
        Vector3 DefaultPos;


        public PointerProperty pProperty;
        public Color[] pointerColors;

        // Use this for initialization
        void OnDrawGizmos()
        {
            DefaultPos = GizmoPos + new Vector3(0, 1, 0);
            Gizmos.color = pointerColors[(int)pProperty.pointerStyle];
            Gizmos.DrawCube(DefaultPos, GizmoSize);
            transform.position = GizmoPos;
        }

        private void OnDrawGizmosSelected()
        {

            Vector3 pos = GizmoPos + new Vector3(0, 1, 0);

            Gizmos.color = pointerColors[(int)pProperty.pointerStyle] * new Color(1, 1, 1, 0.3f);
            Gizmos.DrawCube(pos, GizmoSize * 1.8f);

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(pos, GizmoSize);
            //Gizmos.DrawWireCube(pos, GizmoSize * 1.8f);

        }

    }
}