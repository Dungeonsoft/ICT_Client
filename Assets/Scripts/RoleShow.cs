using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;

namespace ICT_Engine
{
    public class RoleShow : MonoBehaviour
    {

        #region variables
        Vector3 gizmoAddPos = new Vector3(0, 0.5f, 0);
        public float gizmoSize = 0.5f;
        #endregion

        #region 캐릭터 더미 표시용
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + gizmoAddPos, gizmoSize);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position + gizmoAddPos, gizmoSize);
        }
        #endregion
    }
}