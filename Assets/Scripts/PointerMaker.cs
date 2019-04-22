using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;

namespace ICT_Engine
{
    public class PointerMaker : MonoBehaviour
    {
        public Color[] pointerColors;
        //포인터를_가로세로_몇개를_생성할지_결정한다//
        public int countPointersX;
        public int countPointersZ;

        //임시_포인터를_이용하여_포인터들이_생성될_영역을_지정한다//
        public Vector3 guidePos1;
        public Vector3 guidePos2;

        //순서대로_실행하여_포인터를_생성한다//
        public bool createGuide;    //1
        public bool getGuidePos;    //2
        public bool createPointers; //3


        private void OnDrawGizmos()
        {
            //가이드가_되는_두개의_포인터를_생성//
            if (createGuide == true)
            {
                DestroyChild();

                for (int i = 0; i < 2; i++)
                {
                    GameObject GO = new GameObject();
                    GO.name = "Guide" + i.ToString("00");

                    GO.AddComponent<PointGizmo>();
                    GO.AddComponent<PointerProperty>();

                    GO.GetComponent<PointerProperty>().pointerStyle = PointerStyle.GuidePointer;
                    GO.GetComponent<PointGizmo>().pointerColors = pointerColors;
                    GO.GetComponent<PointGizmo>().pProperty = GO.GetComponent<PointerProperty>();

                    GO.transform.parent = this.transform;
                    GO.transform.position = this.transform.position + new Vector3(0, 10, 0);
                }

                createGuide = false;
            }

            //가이드포인터에서_위치값만_저장한_후_가이드삭제//
            if (getGuidePos == true)
            {
                guidePos1 = transform.GetChild(0).position;
                guidePos2 = transform.GetChild(1).position;
                DestroyChild();

                getGuidePos = false;
                createPointers = true;
            }

            //가이드위치를_기준으로_지정된_영역에_액선포인터를_생성//
            if (createPointers == true)
            {
                //Debug.Log("Start");
                float pIntX = 1;
                float pIntZ = 1;

                //Vector3 pointC = guidePos1;

                if (guidePos1.x > guidePos2.x) pIntX *= -1;
                if (guidePos1.z > guidePos2.z) pIntZ *= -1;

                float interX = 0;
                float interZ = 0;

                if (countPointersX - 1 > 0)
                    interX = Mathf.Abs(guidePos1.x - guidePos2.x) / (countPointersX - 1) * pIntX;
                else
                    interX = Mathf.Abs((guidePos1.x - guidePos2.x) / 2);

                if (countPointersZ - 1 > 0)
                    interZ = Mathf.Abs(guidePos1.z - guidePos2.z) / (countPointersZ - 1) * pIntZ;
                else
                    interZ = Mathf.Abs((guidePos1.z - guidePos2.z) / 2);

                float interXadd = interX;
                float interZadd = interZ;

                for (int i = 0; i < countPointersZ; i++)
                {
                    if (countPointersZ > 1) interZadd = interZ * i;
                    for (int j = 0; j < countPointersX; j++)
                    {
                        GameObject GO = new GameObject();
                        GO.name = "Pointer" + (i * countPointersX + j).ToString("000");

                        GO.AddComponent<PointGizmo>();
                        GO.AddComponent<PointerProperty>();

                        GO.GetComponent<PointGizmo>().pointerColors = pointerColors;
                        GO.GetComponent<PointGizmo>().pProperty = GO.GetComponent<PointerProperty>();
                        GO.GetComponent<PointerProperty>().pointerStyle = PointerStyle.NotiPointer;
                        GO.transform.parent = this.transform;

                        if (countPointersX > 1) interXadd = interX * j;
                        GO.transform.position = guidePos1 + new Vector3(interXadd, 0, interZadd);
                        //Debug.Log("Make");
                    }
                }
                createPointers = false;
            }
        }

        /// <summary>
        /// 현재_오브젝트의_하위_차일드_오브젝트를_제거한다///
        /// </summary>
        void DestroyChild()
        {
            int cnt = transform.childCount;
            for (int h = 0; h < cnt; h++)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }
}