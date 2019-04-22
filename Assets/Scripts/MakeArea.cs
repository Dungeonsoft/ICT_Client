using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;

namespace ICT_Engine
{
    //룸이_기본이_되는_빈_오브젝트를_생성한다//
    public class MakeArea : MonoBehaviour
    {
        public bool makeRoom;
        public int roomNum;

        public Color[] pointerColors;

        private void OnDrawGizmos()
        {
            if (makeRoom == true)
            {
                GameObject GO = new GameObject();
                GO.name = "Area" + roomNum.ToString("000");
                GO.transform.parent = this.transform;
                GO.AddComponent<PointerMaker>();
                GO.GetComponent<PointerMaker>().pointerColors = pointerColors;

                roomNum++;
                makeRoom = false;
            }
        }
    }
}