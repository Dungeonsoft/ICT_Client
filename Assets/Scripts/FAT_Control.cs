using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.UI;
using ICT_Engine;

namespace ICT_Engine
{
    public class FAT_Control : MonoBehaviour
    {
        #region Variables
        public Transform mainCamOrigin;
        public GameObject[] activeControled;
        public GameObject uiCanvas;
        public GameObject uiStart;
        public GameObject uiStartBtns;
        public GameObject uiSetting;
        public GameObject uiSelectScenario;
        public GameObject uiSelectMode;
        public GameObject uiSelectRole;

        int sNum = 1;

        Del delUpdate;

        // csv 파일로 저장된 롤 정보를 가지고 오기위해 성성한 변수.
        public List<Dictionary<string, object>> sceneRoleCsvData;
        // csv 파일 패쓰(파일이름) 기록을 위한 변수.
        public string activeRoleInfoPath;

        #endregion

        // Use this for initialization
        void Start()
        {
            ConstDataScript.scenarioNum = 1;
            ConstDataScript.modeNum = 2;
            ConstDataScript.roleNum = 1;

            uiCanvas.SetActive(true);
            uiStart.SetActive(true);
            uiSetting.SetActive(false);

            uiSelectScenario.SetActive(false);
            //uiSelectMode.SetActive(false);
            uiSelectRole.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (delUpdate != null)
            {
                delUpdate();
            }
        }

        #region 타이틀 화면 메소드//
        public void ClickStartBtn()
        {
            Debug.Log("ClickStartBtn");
            uiStart.SetActive(false);        //스타트버튼을 누르고 발생하는 내용을 기록한다.//
            uiSelectScenario.SetActive(true);
        }

        public void ClickSettingOnBtn()
        {
            uiStartBtns.SetActive(false);
            uiSetting.SetActive(true);
        }

        public void ClickLanguageBtn(int num)
        {
            ConstDataScript.langNum = num;
        }

        public void ClickFpsBtn(bool isOn)
        {
            ConstDataScript.isFpsOn = isOn;
        }

        public void ClickLatencyBtn(bool isOn)
        {
            ConstDataScript.isLatOn = isOn;
        }

        public void ClickSettingOffBtn()
        {
            uiStartBtns.SetActive(true);
            uiSetting.SetActive(false);
        }
        
        public void ClickEndBtn()
        {
            Application.Quit();
        }
        #endregion

        #region 시나리오 화면 메소드//
        public void ClickBackToStartBtn()
        {
            uiStart.SetActive(true);
            uiSelectScenario.SetActive(false);
        }

        public void ClickScenario(int num)
        {
            Debug.Log("시나리오 버튼 누름");

            ConstDataScript.scenarioNum = sNum = num;

            ChangeRoleStatus();

            uiSelectRole.SetActive(true);
            uiSelectScenario.SetActive(false);
        }

        public void ChangeRoleStatus()
        {
            Debug.Log("CSV 파일 로드");
            sceneRoleCsvData = CSVReader.Read(activeRoleInfoPath);

            // 해당씬의 롤의 수를 따로 저장한다(일반적으로 17)
            int rCnt = sceneRoleCsvData.Count;

            // 씬넘버를 가지고 온다.
            int scNum = ConstDataScript.scenarioNum;
            // 가지고 온 씬 넘버를 이용하여 롤정보가 있는 csv 데이터에서 부를수 데이터를.
            // 찾아올수 있도록 Scene** 형태로 씬넘버를 문자로 변환한다.
            string sName = "Scene" + scNum.ToString("00");

            // 롤 선택창에 나오는 롤리스트 네개를 가지고 와서 따로 저장해 놓는다.
            List<Transform> roleList = uiSelectRole.GetComponent<SelRoleScript>().roleList;
            int rListCnt = 0;

            for(int i = 0; i< rCnt; i++)
            {
                string roleStatus = sceneRoleCsvData[i][sName].ToString();
                if (roleStatus == "active")
                {
                    // 이름 넣고.
                    roleList[rListCnt].Find("Text").GetComponent<Text>().text = SetRoleToString(i + 1);
                    // 이름을 대표하는 숫자를 넣는다.
                    roleList[rListCnt].Find("Button").GetComponent<SetRoleNum>().roleNum = i + 1;
                    rListCnt++;
                }
            }
        }
        #endregion

        #region 모드 화면 메소드//
        public void ClickBacktoScenarioBtn()
        {
            uiSelectRole.SetActive(false);
            uiSelectScenario.SetActive(true);
        }
        public void ClickMode(int modeType)
        {
            ConstDataScript.modeNum = modeType;


            //원래 이부분은 모드를 설정하는 곳이나 모드 설정이 없어짐으로//
            //시나리오 선택또는 자유모드 선택으로 한다//
            //1은 시나리오(Tut), 2는 자유모드//
            //ConstDataScript.scenarioNum = sNum = modeType;


            //모드를 선택하고 롤 선택으로 넘어갈때 강제로 선장으로 만들어준다//
            ConstDataScript.roleNum = 1;
            uiSelectMode.SetActive(false);
            uiSelectRole.SetActive(true);
            
            //if(modeType == 2)
            //{
            //    UnityEngine.SceneManagement.SceneManager.LoadScene(sNum);
            //}
        }
        #endregion

        #region 롤 화면 메소드//
        public void ClickBacktoModeBtn()
        {
            uiSelectMode.SetActive(true);
            uiSelectRole.SetActive(false);
        }
        public void ClickRole(int rNum)
        {
            ConstDataScript.roleNum = rNum;
            Debug.Log("Role Num: "+ ConstDataScript.roleNum); 
        }

        public void ClickRoleStart()
        {
            uiSelectRole.SetActive(false);
            //shipDataInst.SetActive(true);
            for (int i = 0; i < activeControled.Length; i++)
            {
                activeControled[i].SetActive(true);
            }
            uiStart.SetActive(false);

            switch (sNum)
            {
                case 1:
                    Debug.Log("1번: 화재");
                    break;
                case 2:
                    Debug.Log("2번: 퇴선");
                    break;
                case 3:
                    Debug.Log("3번: 밀폐구역");
                    break;

            }

            UnityEngine.SceneManagement.SceneManager.LoadScene(sNum);
            // 1번: 화재//
            // 2번: 퇴선//
            // 3번: 밀폐구역//
        }
        #endregion


        public string SetRoleToString(int r)
        {
            string rName = "";
            switch (r)
            {
                case 1:
                    rName = "Captain";
                    break;
                case 2:
                    rName = "Officer 1st";
                    break;
                case 3:
                    rName = "Officer 2nd";
                    break;
                case 4:
                    rName = "Officer 3rd";
                    break;
                case 5:
                    rName = "Engineer Chief";
                    break;
                case 6:
                    rName = "Engineer 1st";
                    break;
                case 7:
                    rName = "Engineer 2nd";
                    break;
                case 8:
                    rName = "Engineer 3rd";
                    break;
                case 9:
                    rName = "Bosun";
                    break;
                case 10:
                    rName = "Crew A";
                    break;
                case 11:
                    rName = "Crew B";
                    break;
                case 12:
                    rName = "Crew C";
                    break;
                case 13:
                    rName = "Oiler 1st";
                    break;
                case 14:
                    rName = "Oiler A";
                    break;
                case 15:
                    rName = "Oiler B";
                    break;
                case 16:
                    rName = "Oiler C";
                    break;
                case 17:
                    rName = "Cook Chief";
                    break;
            }
            return rName;
        }


    }
}