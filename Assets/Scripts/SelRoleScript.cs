using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ICT_Engine;

namespace ICT_Engine
{
    public class SelRoleScript : MonoBehaviour
    {

        public List<Transform> roleList;
        //int roleListNum = 1;
        public Color onColor;
        public Color offColor;

        public List<GameObject> rolesGr;

        int rolesGrNum = 1;

        public GameObject btnLeft;
        public GameObject btnRight;

        public void OnEnable()
        {
            ShowRoleSelected();
            rolesGrNum = 1;
            ShowRoles();

            //OnEnableScript();
        }

        public void ShowRoleSelected()
        {
            for (int r = 1; r <= roleList.Count; r++)
            {
                Transform roleBtn = roleList[r - 1].GetChild(0);
                int rNum = roleBtn.GetComponent<SetRoleNum>().roleNum;
                if (rNum != ConstDataScript.roleNum)
                    roleBtn.GetComponent<Image>().color = offColor;
                else
                    roleBtn.GetComponent<Image>().color = onColor;
            }
        }

        public void ClickArrowBtn(int num)
        {
            rolesGrNum += num;
            if (rolesGrNum < 1)
                rolesGrNum = 1;
            else
            if (rolesGrNum > rolesGr.Count)
                rolesGrNum = rolesGr.Count;
            ShowRoles();
        }

        void ShowRoles()
        {
            for (int i = 1; i <= rolesGr.Count; i++)
                if (i == rolesGrNum)
                    rolesGr[i - 1].SetActive(true);
                else
                    rolesGr[i - 1].SetActive(false);

            //if (rolesGrNum == 1)
            //{
            //    btnLeft.SetActive(false);
            //    btnRight.SetActive(true);
            //}
            //else if (rolesGrNum == rolesGr.Count)
            //{
            //    btnLeft.SetActive(true);
            //    btnRight.SetActive(false);
            //}
            //else
            //{
            //    btnLeft.SetActive(true);
            //    btnRight.SetActive(true);
            //}
            return;
        }

        void OnEnableScript()
        {
            //보여야 될 역할들//
            int[] scene1Ch = { 0, 1, 4, 9 };
            int[] scene2Ch = { 0, 4 };
            int[] scene3Ch = { 0, 1, 2, 4, 5, 6, 7, 9, 10, 11, 12, 13, 14, 15};

            //처음에 어떤 시나리오를 선택했는지 알아낸다//
            if (ConstDataScript.scenarioNum == 1)
                ActiveRoles(scene1Ch);
            else
            if (ConstDataScript.scenarioNum == 2)
                ActiveRoles(scene2Ch);
            else
            if (ConstDataScript.scenarioNum == 3)
                ActiveRoles(scene3Ch);
        }

        void ActiveRoles(int[] sceneCh)
        {
            //우선 모든 버튼을 비활성화하고//
            for (int i = 0; i < roleList.Count; i++)
                roleList[i].GetChild(0).GetComponent<Button>().enabled = false;

            //활성기록(scene1Ch)과 일치하는 롤넘버를 찾아내어//
            //그것들만 활성화 한다//
            for (int j = 0; j < sceneCh.Length; j++)
                for (int i = 0; i < roleList.Count; i++)
                    if (sceneCh[j] == i)
                        roleList[i].GetChild(0).GetComponent<Button>().enabled = true;
        }
    }
}