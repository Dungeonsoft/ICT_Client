using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;
using UnityEngine.PostProcessing;

namespace ICT_Engine
{
    public class WhenLoadScene : MonoBehaviour
    {
        public GameObject vrOrigin;
        public bool isMr;
        public List<GameObject> Roles;

        public GameObject activeRole;

        //public GameObject uiFrameRate;
        //public GameObject uiLatency;

        public GameObject qPanel;
        public GameObject uiBG;
        public GameObject[] textBubble;
        public GameObject que;
        public GameObject[] ans;

        public GameObject backToMain;

        [Header("-----------------------------")]
        [Header("NPC를 보일지 여부를 결정한다.")]
        public bool isShowNPC = true;


        public GameObject mainUI;
        public PostProcessingProfile camPostProcess;

        Transform[] allChildren;
        // Use this for initialization
        void Start()
        {

            // 윈도우 MR 일 경우 아래와 같이 세팅을 해준다 //
            if (isMr)
            {
                vrOrigin.SetActive(false);
                GameObject.Find("MixedRealityCamera").AddComponent<PostProcessingBehaviour>();
                GameObject.Find("MixedRealityCamera").GetComponent<PostProcessingBehaviour>().profile = camPostProcess;

                vrOrigin = GameObject.Find("MixedRealityCameraParent");

                vrOrigin.SetActive(true);

                // 새롭게 카메라와 UI 오브젝트들을 지정 //
                GameObject sContent = new GameObject();
                sContent = GameObject.Find("SceneContent");

                if (sContent != null)
                {
                    sContent.transform.Find("Canvas").gameObject.SetActive(false);
                    mainUI = sContent.transform.Find("Canvas_MainUI").gameObject;
                    mainUI.SetActive(true);
                    mainUI.transform.parent.localPosition = Vector3.zero;

                    // 매인 유아이 아래의 모든 오브젝트를 차일드로 등록하여 검색하기 용이하게 준비한다. //
                    GetAllChildren(mainUI.transform);

                    //uiFrameRate = FindChildDeep("UiFrameRate").gameObject;
                    //uiLatency = FindChildDeep("UiLatency").gameObject;
                    qPanel = FindChildDeep("QuestionPanel").gameObject;
                    uiBG = FindChildDeep("BG").gameObject;

                    textBubble = new GameObject[2];
                    textBubble[0] = FindChildDeep("TextBubble01").gameObject;
                    textBubble[1] = FindChildDeep("TextBubble02").gameObject;
                    //textBubble[0].SetActive(false);
                    //textBubble[0].SetActive(false);

                    que = FindChildDeep("QuestionText").gameObject;
                    //que.SetActive(false);

                    ans = new GameObject[3];
                    ans[0] = FindChildDeep("A1Text").gameObject;
                    ans[1] = FindChildDeep("A2Text").gameObject;
                    ans[2] = FindChildDeep("A3Text").gameObject;
                    //ans[0].SetActive(false);
                    //ans[1].SetActive(false);
                    //ans[2].SetActive(false);

                    backToMain = FindChildDeep("EndPanel").gameObject;
                    //backToMain.SetActive(false);
                }

            }
            else
            {
                if(GameObject.Find("MixedRealityCameraParent") != null)
                GameObject.Find("MixedRealityCameraParent").SetActive(false);
            }

            //액션 롤인것과 아닌것을 구분해준다.
            for (int i = 1; i <= Roles.Count; i++)
            {
                if (Roles[i - 1].GetComponent<MoveOnPathScript>() == null)
                    Roles[i - 1].AddComponent<MoveOnPathScript>();

                if (i != ConstDataScript.roleNum)
                {
                    Roles[i - 1].GetComponent<MoveOnPathScript>().isActionCharacter = false;
                    //Roles[i - 1].SetActive(false);
                    //시나리오 2나 3이면 액티브 롤 말고는 다 꺼준다//
                    //if (ConstDataScript.scenarioNum == 2 || ConstDataScript.scenarioNum == 3)
                    if (isShowNPC == false)
                    {
                        Roles[i - 1].SetActive(false);
                    }
                }
                else
                {
                    //Debug.Log("액티브 롤 찾음");
                    MoveOnPathScript role = Roles[i - 1].GetComponent<MoveOnPathScript>();
                    role.isActionCharacter = true;
                    role.isFreeMov = ConstDataScript.isMulti;

                    if(role.isFreeMov == false)
                    {
                        role.GetComponent<CharacterController>().enabled = false;
                        //GameObject.DestroyImmediate(role.GetComponent<Rigidbody>());
                    }

                    activeRole = Roles[i - 1];
                    vrOrigin.transform.parent = activeRole.transform;
                    vrOrigin.transform.localPosition = Vector3.zero;
                }

                //if (ConstDataScript.isFpsOn == true) uiFrameRate.SetActive(true); else uiFrameRate.SetActive(false);
                //if (ConstDataScript.isLatOn == true) uiLatency.SetActive(true); else uiLatency.SetActive(false);

                qPanel.SetActive(false);
                uiBG.SetActive(false);
                foreach (var v in textBubble)
                {
                    v.SetActive(false);
                }

                if (Roles[i - 1].GetComponent<MoveOnPathScript>().isActionCharacter == true)
                {
                    Roles[i - 1].transform.GetChild(0).gameObject.SetActive(false);
                    Roles[i - 1].transform.Find("Canvas").gameObject.SetActive(false);
                }

                backToMain.SetActive(false);
            }

            //날씨 세팅하는 함수.
            SetWeather();


        }

        //날씨 정보를 가지고 와서 세팅을 한다.
        void SetWeather()
        {
            string weatherType = ConstDataScript.weatherType;
            switch (weatherType)
            {
                case "sunny":
                    RenderSettings.fog = false;
                    break;
                case "fog":
                    RenderSettings.fog = true;
                    RenderSettings.fogColor = new Color(0.42f,0.47f,0.57f);
                    RenderSettings.fogMode = FogMode.ExponentialSquared;
                    RenderSettings.fogDensity = 0.015f;
                    break;

                default:
                    RenderSettings.fog = false;
                    break;
            }
        }


        private Transform FindChildDeep(string findName)
        {

            int cnt = allChildren.Length;
            for (int i = 0; i < cnt; i++)
            {
                if (allChildren[i].name == findName)
                    return allChildren[i].transform;
            }

            //Debug.LogError("찾고_있는_트랜스폼이_없습니다");
            return null;
        }

        private void GetAllChildren(Transform tParent)
        {
            allChildren = tParent.GetComponentsInChildren<Transform>();
        }
    }
}