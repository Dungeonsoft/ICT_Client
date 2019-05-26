using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ICT_Engine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.XR.WSA.Input;


namespace ICT_Engine
{
    public class MoveOnPathScript : MonoBehaviour
    {
        public float moveSpeed = 1f;

        #region 변수들//
        public ControllerKeyInputTest touchInput;

        // 앞으로 캐릭터는 액션캐릭터(나)(자유이동)
        // 나 외의 캐릭터(다른 유저)(타인의 이동 데이터를 이용하여 움직임)
        // 지정이 없으면 그냥 엔피시 (패쓰 따라 움직임)
        public bool isActionCharacter = false;
        public bool isCharacter = false;
        public int modeNum;
        public WhenLoadScene wlScene;
        public int currentWayPointID = 0;
        public float speed;
        public float currentSpeed;
        private float reachDistance = 0.5f;
        public float rotationSpeed = 5.0f;
        AudioSource mainAudio;

        Vector3 angle;

        float distance = 0;
        Quaternion rotation;

        float dist;
        public int actionNum = 0;
        Del upDel;

        public bool updateConectedRoles = false;
        public bool resetConnectedRoles = false;

        public SetActionPointerInfoForCharacter[] setAction;
        public SetActionPointerInfoForCharacter sa;

        public Animator playerAnimator;

        //아래 솔브퀘스쳔 메소드를 혹시 두번 실행할까봐 막아놓는 것으로 쓰인 불 변수//
        public bool isSoveque = false;

        AddClass aClass;
        AudioClip click;
        #endregion Variables


        Quaternion lerpE;
        Vector3 lerpV;
        Quaternion startRot;
        Quaternion endRot;
        float by;

        // 자유 이동 관련 변수들.
        public bool isFreeMov = false;

        public bool isDebug = false;
        void ShowDebug(string s, bool isShow= true)
        {
            if (isDebug == true && isShow == true)
            {
                Debug.Log("Show Log: " + s);
            }
        }

        private void OnDrawGizmos()
        {
            //신호를 받는 것들을 찾아서 신호를 주는 쪽에서 입력하여 준다//
            if (updateConectedRoles == true)
            {
                ResetConnect();
                updateConectedRoles = false;
            }
            //각 롤별로 연결된 고리를 모두 초기화 한다//
            if (resetConnectedRoles == true)
            {
                StartCoroutine(ConnectInAction());
                resetConnectedRoles = false;
            }
        }

        public void ResetConnect()
        {
            for (int i = 0; i < setAction.Length; i++)
            {
                setAction[i].isInputActionB = false;
                setAction[i].connectedRole = null;
                setAction[i].setActionNum = 0;
            }

        }

        public IEnumerator ConnectInAction()
        {
            yield return new WaitForSeconds(0.15f);
            //각액션별로 인액션을 찾아줌//
            for (int i = 0; i < setAction.Length; i++)
            {
                if (setAction[i].inAction1 == null)
                {
                    //Debug.Log("inAction1: NULL");
                }
                if (setAction[i].inAction2 == null)
                {
                    //Debug.Log("inAction2: NULL");
                }
                ConnectRoles(setAction[i].inAction1, i);
                ConnectRoles(setAction[i].inAction2, i);
            }
        }

        //신호를 받는 것들을 찾아서 신호를 주는 쪽에서 입력하여 준다//
        //인액션의 내용을 받아서 정보가 있으면 연결처리를 해준다//
        void ConnectRoles(InputAction[] inAction, int actNum)
        {
            //InAction은 현재 스크립트가 가지고 있는 타 캐릭터와 연결되는 액션 정보이다.
            if (inAction == null)
            {
                return;
            }
            for (int j = 0; j < inAction.Length; j++)
            {

                #region 이넘을 이용하여 같은 이넘을 가지고 있는 액션을 찾아가서 연결하는 방법.
                // 먼저 연결된 캐릭터의 메인스크립트인(MoveOnPathScript)를 연결하여 가져온다.
                MoveOnPathScript otherCharScript = inAction[j].otherSA;
                // 그 다음 이 캐릭터와 연결된 것으로 입력되어있는 ActionEnum을 정보를 가지고 가지고 온다.
                ActionEnum aEnum = inAction[j].inputActionEnum;
                // 같은 이름의 인액션 이넘이 붙어있는 액션 넘버를 찾는다.
                int len = otherCharScript.setAction.Length;
                for (int i = 0; i < len; i++)
                {
                    ActionEnum ae = otherCharScript.setAction[i].aEnum;
                    // 연결하려하는 액션이넘과  연결되는 캐릭터쪽의 같은 이름의 이넘을 찾는다. 
                    if (aEnum == ae)
                    {
                        // 찾았으면 그 액션이넘이 속해 있는 액션넘버를 찾는다.
                        // 여기서는 i 값이 액션 넘버와 동일하다.
                        inAction[j].otherSA.setAction[i].isInputActionB = true;
                        inAction[j].otherSA.setAction[i].connectedRole = this.GetComponent<MoveOnPathScript>();
                        // 여기서 액션 넘버는 현재 신호를 주는 캐릭터쪽의 액션넘버이다.
                        inAction[j].otherSA.setAction[i].setActionNum = actNum;
                        // 이후에 다시 신호를 주고 받을 때 편하도록(이전구조를 이용할 수 있도록) 연결받는쪽 캐릭터의.
                        // 액션 넘버를 기록하여 준다.
                        inAction[j].inputActionNum = i;
                        break;
                    }
                }
                #endregion
            }
        }

        GameObject mrCam;
        GameObject ChSet;

        // Use this for initialization
        void Start()
        {
            touchInput = GameObject.Find("TouchInput").GetComponent<ControllerKeyInputTest>();
            //버튼 클릭 사운드 지정//
            click = Resources.Load<AudioClip>("Actions/Sounds/Click");


            aClass = GetComponent<AddClass>();
            wlScene = GameObject.Find("MainController").GetComponent<WhenLoadScene>();
            mainAudio = wlScene.GetComponent<AudioSource>();
            if (setAction.Length > 0)
            {
                //델리게이트하기 전에 선행되어야 될 것들을 처리한다//
               // ShowDebug("NextAction1 Method 01");
                NextAction1();
            }
            if (transform.GetChild(0) != null)
            {
                playerAnimator = transform.Find("CharacterAllSet").GetComponent<Animator>();

            }

            if (isActionCharacter == true)
            {
                // 자유이동시 사용되는 넥스트 포인터를 부르기 위한 메소드.
                // 넥스트 포인터는 이동 방향을 알려주는 화살표이다.
                GetListNextPointers();
                mrCam = GameObject.Find("MixedRealityCamera");
            }

            // 내 캐릭이 아니라 남의 캐릭인가 확인하는 코드//
            // 엔피씨가 아니라 남의 캐릭터면 자유이동한 값을 가지고 와서 그값을 대입한다.
            else
            {
                int cnt = ConstDataScript.userCharacters.Count;
                Transform par = this.transform.parent;
                int myChNum = 0;
                for (int i = 0; i < par.childCount; i++)
                {
                    if (this.transform == par.GetChild(i))
                    {
                        myChNum = i + 1;
                        break;
                    }
                }

                List<int> uChs = ConstDataScript.userCharacters;
                for (int i = 0; i < cnt; i++)
                {

                    if (uChs[i] == myChNum)
                    {
                        isCharacter = true;
                        break;
                    }
                }

            }
            ChSet = this.transform.Find("CharacterAllSet").gameObject;

            //try
            //{
            //    mainCam = GameObject.FindWithTag("MainCamera").transform;
            //}
            //catch
            //{ }

            mainCam = GameObject.FindWithTag("MainCamera").transform;

        }

        void GetListNextPointers()
        {
            GameObject nextPointersG = GameObject.Find("NextPointers");

            int childCnt = nextPointersG.transform.childCount;
            for (int i = 0; i < childCnt; i++)
            {
                GameObject childobj = nextPointersG.transform.GetChild(i).gameObject;
                if (childobj.name == this.name)
                {
                    int cnt = childobj.transform.childCount;
                    for (int j = 0; j < cnt; j++)
                    {
                        GameObject ccobj = childobj.transform.GetChild(j).gameObject;
                        ccobj.SetActive(false);
                        if (ConstDataScript.isMulti == false)
                        {
                            ccobj.GetComponent<CheckMeetChar>().enabled = false;
                        }
                        nextPointers.Add(ccobj);
                    }
                }
            }
        }

        // Update is called once per frame
        bool isChangeActionNum = false;
        GetOtherUserMoveData otherData = new GetOtherUserMoveData();

        Transform mainCam;
        void Update()
        {
            if (upDel != null)
            {
                upDel();
            }
            else
            {
                //Debug.Log("Updel is NULL! name: " + this.name);
            }
            if (isActionCharacter == true)
            {
                GetComponent<CharacterController>().Move(Physics.gravity);

                if (touchInput.isTouchPad == true && touchInput.touchPadInput != 0f)
                {
                    // 움직인다 //
                    float val = touchInput.touchPadInput;
                    Vector3 aForce = mrCam.transform.eulerAngles;
                    Vector3 chAngle = ChSet.transform.eulerAngles;
                    // 캐릭터 이동을 값을 실제 적용하는 부분.
                    Vector3 moveData = new Vector3(Mathf.Sin(aForce.y / 180f * Mathf.PI) * Time.deltaTime * 3f * val,
                                                         0.03f,
                                                         Mathf.Cos(aForce.y / 180f * Mathf.PI) * Time.deltaTime * 3f * val);
                    // 캐릭터 컨트롤러를 이용.
                    GetComponent<CharacterController>().Move(moveData * moveSpeed);
                    // 움직이니 애니데이터를 워킹으로 아웃풋한다.
                    // 아래 otherData 묶음과 같이 서버로 보내는 데이터이다.
                    otherData.chMotion = "Walking_01";
                }
                else
                {
                    // 멈춰있으니 애니데이터를 아이들로 아웃풋한다.
                    // 아래 otherData 묶음과 같이 서버로 보내는 데이터이다.
                    otherData.chMotion = "Idle_01";
                }

                //서버로 PC의 데이터를 보낼지 말지를 결정하는 부분이다.
                sendChDataToServer1();
                /////////////////////////////////////////////////////////////////
                // 바로 아래의 함수 sendChDataToServer 로 내용을 옮김//
                // 함수를 호출하여 사용할 것.
                /////////////////////////////////////////////////////////////////
                ////여기서 움직임의 데이터를 보낸다.//
                ////보내는 이름은 "userPozRot"//
                ////이부분에서 데이터가 초당 10번정도만 전달되도록 수정한다.//
                ////이동거리 변경사항도 적용하여 최소 10cm이상 위치가 변경되었을 때만 데이터가 서버에 전달되도록 수정한다.//
                ////otherData.pozX = transform.position.x.ToString();
                ////otherData.pozY = transform.position.y.ToString();
                ////otherData.pozZ = transform.position.z.ToString();

                ////otherData.rotX = mainCam.eulerAngles.x.ToString();
                ////otherData.rotY = mainCam.eulerAngles.y.ToString();
                ////otherData.rotZ = mainCam.eulerAngles.z.ToString();

                ////otherData.actionNum = actionNum.ToString();
                ////otherData.roleNum = ConstDataScript.roleNum.ToString();
                ////otherData.uid = ConstDataScript.uid;
                ////otherData.isChangeActionNum = isChangeActionNum.ToString();
                //// 액션이 변경되었다는 것을 알려줬으니 다시 false로 변경한다.
                ////isChangeActionNum = false;

                ////클라이언트 로비 서버 커넥트 스크립트를 통하여 데이터를 서버로 보낸다.
                //// 1. 먼저 스크립트가 붙어 있는 오브젝트를 찾고.
                ////ClientLobbyServerConnect cServerCon = GameObject.Find("ControlSystem").GetComponent<ClientLobbyServerConnect>();
                //// Emit를 이용하여 값을 전달한다.(정확히는 메소드를 호출한다)
                ////cServerCon.UserPozRot(otherData);
                /////////////////////////////////////////////////////////////////
                // 바로 아래의 함수 sendChDataToServer 로 내용을 옮김//
                // 함수를 호출하여 사용할 것.
                /////////////////////////////////////////////////////////////////

            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameObject mrCameraParent = GameObject.Find("MixedRealityCameraParent");

                Destroy(mrCameraParent);
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }


        float sendDataTimeInterval = 0.05f;   // 0.1초에한번씩 데이터를 서버로 보냄.
        public float sendDataDistInterval = 0.1f;   // 0.1m 이상 이동시 데이터를 서버로 보냄.
        
        // 2019년 5월 13일 추가. 현재회전이 안됨. 이동시 워킹이 작동하지 않고 아이들이 작동함.
        public float sendDataRotInterval = 1.0f;    // 5도 이상 회전시 데이터를 보냄.

        int oldActionNum = 0; // 액션 넘버가 변경되었는지 확인하기 위해 최근의 액션넘버를 저장하여 놓는다.
        float nextSendDataTime = 0; // 다음 데이터를 보내는 시간을 저장한다.
        Vector3 oldPosition = Vector3.zero; // 데이터를 보내기 위한 최소거리를 확인하기 위해 직전 캐릭터 위치를 저장한다.
        
        // 2019년 5월 13일 추가.
        Vector3 oldRot = Vector3.zero; // 데이터를 보내기 위한 최소회전을 확인하기 위해 직전 캐릭터 회전값을 저장한다.

        //지정한 최소값이 넘었는지를 판별하는데 사용하는 변수 세개를 선언한다.
        bool isOverTime = false;
        bool isOverDist = false;
        bool isOverRot = false;

        void sendChDataToServer1()
        {
            //먼저최초에 거리를 한번구한다.(기본)
            float dist = Vector3.Distance(oldPosition, transform.position);
            float rot = Mathf.Abs(oldRot.y - mainCam.eulerAngles.y);

            

            // 2019년 5월 13일 수정한 IF 조건문 내용.
            // 여기 조건문에서 이동,회전, 시간을 측정하여 서버에 데이터를 보낼지 여부를 결정한다.
            isOverTime = (nextSendDataTime <= Time.time) ? true:false;
            isOverDist = (sendDataDistInterval <= dist) ? true : false;
            isOverRot = (sendDataRotInterval <= rot) ? true : false;

            //if (isOverRot == true) {
            //    Debug.Log("--------Rot True--------|| ROT || "+ rot);
            //    // 회전값이 트루면 그 값이 얼마나 되는지 여기서 체크한다.
            //}

            if (isOverTime && (isOverDist || isOverRot))//타임인터벌과거리인터벌의 조건이 맞을때
            {
                //Debug.Log("N1 Time: " + nextSendDataTime);
                //Debug.Log("N2 Time: " + Time.time);
                nextSendDataTime = Time.time + sendDataTimeInterval;
                oldPosition = transform.position;
                
                // 2019년 5월 13일 추가. 현재의 회전값을 저장해준다.
                oldRot = GameObject.FindWithTag("MainCamera").transform.eulerAngles;
                sendChDataToServer2();
                return;
            }

            if (oldActionNum < actionNum) // 액션넘버가 숫자가 하나 올라가거나.
            {
                oldActionNum = actionNum;
                sendChDataToServer2();
                return;
            }
        }

        void sendChDataToServer2()
        {
            Debug.Log("서버로 데이터를 보낸다::: "+ Time.time);
            //여기서 움직임의 데이터를 보낸다.//
            //보내는 이름은 "userPozRot"//
            //이부분에서 데이터가 초당 10번정도만 전달되도록 수정한다.// 코드는 수정.
            //이동거리 변경사항도 적용하여 최소 10cm이상 위치가 변경되었을 때만 데이터가 서버에 전달되도록 수정한다.// 코드는 수정.
            otherData = new GetOtherUserMoveData
            {
                // 위치.
                pozX = transform.position.x.ToString(),
                pozY = transform.position.y.ToString(),
                pozZ = transform.position.z.ToString(),
                // 회전.
                rotX = mainCam.eulerAngles.x.ToString(),
                rotY = mainCam.eulerAngles.y.ToString(),
                rotZ = mainCam.eulerAngles.z.ToString(),
                // 그외 정의 데이터.
                actionNum = actionNum.ToString(),
                roleNum = ConstDataScript.roleNum.ToString(),
                uid = ConstDataScript.uid,
                isChangeActionNum = isChangeActionNum.ToString(),
                isWalk = isOverDist.ToString()
            };

            //Debug.Log("otherData-보내는 데이터- 회전값 : " + otherData.rotY);
            //Debug.Log("otherData-보내는 데이터- 움직임여부1 : " + isOverDist);
            //Debug.Log("otherData-보내는 데이터- 움직임여부2 : " + otherData.isWalk);

            // 액션이 변경되었다는 것을 알려줬으니 다시 false로 변경한다.
            isChangeActionNum = false;

            // 움직임이 있었다는 것을 알려줬으니 다시 false로 변경한다.
            isOverTime = false;
            isOverDist = false;
            isOverRot = false;

            //클라이언트 로비 서버 커넥트 스크립트를 통하여 데이터를 서버로 보낸다.
            // 1. 먼저 스크립트가 붙어 있는 오브젝트를 찾고.
            ClientLobbyServerConnect cServerCon = GameObject.Find("ControlSystem").GetComponent<ClientLobbyServerConnect>();
            // Emit를 이용하여 값을 전달한다.(정확히는 메소드를 호출한다)
            cServerCon.UserPozRot(otherData);
        }



        public bool isWaitAddFunction = false;

        void NextAction1()
        {
            // 액션의 첫시작.
            sa = setAction[actionNum];
            isEndAnimClips = false;
            //ShowDebug("NextAction1 IN");
            //ShowDebug(this.name + " ::: NextAction1::: action Number ::: "+ actionNum, true);
            if (playerAnimator != null)
            {
                //ShowDebug(this.name + " :: Player Animator HAS");
            }
            else
            {
                //ShowDebug(this.name + " :: Player Animator HASN'T");
                playerAnimator = this.transform.Find("CharacterAllSet").GetComponent<Animator>();
                //ShowDebug(this.name + " :: Player Animator HAS NOW");

            }

            #region 여기서 애니메이션의 이름을 가지고 와서 플레이 할 수 있게 한다.- 새로운 방식-여러개의 애니메이션을 로드할 수 있다.-
            if (playerAnimator != null)
            {
                //ShowDebug("First Motion::Action Number: "+ actionNum);
                StartCoroutine(PlayCharacterAnimation(sa.fChMotionNew, "First Motion"));
            }
            #endregion


            if (sa == null)
            {

                Debug.Log("SA NULL ACtion NUM is " + actionNum);
            }
            
            if (sa.isMeA == true)
            {
                //PC 일때만 이것을 체크한다.
                if (isActionCharacter == true)
                    isWaitAddFunction = true;
            }

            if (sa == null)
            {
                Debug.Log("Set Action is NULL :: ActionNum :: " + actionNum);
                Debug.LogError("Set Action Length is " + setAction.Length);
            }
            //웨이트타임을 -1로 했을경우 그 액션은 패쓰한다//
            if (sa.waitTime01 == -1)
            {
                //ShowDebug("Jump This Action");

                currentSpeed = 0;
                if (actionNum - 1 < setAction.Length)
                {
                    if (this.name == "001_Captain")
                        Debug.Log("308_Action Number 1을 더함" + " ::: " + actionNum);

                    isChangeActionNum = true;

                    //새로운형식.
                    AddActionNum();

                    // 아래 주석 부분은 구형식.
                    // 새로운 형식에(바로 위 호출되는 함수 참고)선.
                    // 특수동작이 있을경우 그것을 실행해야만 액션넘버가.
                    // 늘어나도록 수정.

                    ////당 액션에서는 더이상 실행할 내용이 없으니 넘어간다//
                    return;
                }
                else
                {
                    //ShowDebug(this.name + "캐릭터 액션을 종료합니다. 마지막 액션 넘버: " + actionNum);

                    //이 스크립트를 비활성화하는 코드를 실행하기 전에//
                    //종료 되었다는 것을 알려주는 창을 열어준다//
                    //이 창이 열리는 것은 모든 캐릭터의 동작이 완료되었을때 발생하도록 한다//

                    this.enabled = false;
                }
            }

            //무시하는 -1이 아니면 다음을 실행한다//
            else
            {
                //ShowDebug(this.transform.name + " :: 캐릭터 위치를 지정할 것인가?::: " + actionNum);

                //스탠딩 포지션을 정하는 트랜스폼이 지정이 되어 있으면//
                //그 트랜스폼에서 위치값과 회전값을 가지고 와서 캐릭터에 적용하여 준다//
                if (sa.standingPosition != null)
                {
                   // ShowDebug(this.transform.name + "::: standingPosition HAS", false);

                    transform.position = sa.standingPosition.position;
                    transform.rotation = sa.standingPosition.rotation;
                }
                else {
                    //ShowDebug(this.transform.name + "::: standingPosition is NULL");
                }

                //시간을 재는 변수값을 0으로 만들어 준다//
                nAction1_1SpendTime = 0;

                if (sa.isInputActionB == false)
                {
                    //외부에서 실행 신호를 주는 것이 없다면 정해진 대기시간만큼 기다리는//
                    //델리게이트를 실행한다//
                    //ShowDebug(this.name + " ::: NextAction1_1_1 Method");
                    upDel += NextAction1_1_1;
                }
                else
                if (sa.isInputActionB == true)
                {
                    //ShowDebug("외부 신호를 기다림01::: " + actionNum + " ::: Cahractor::" + this.name);

                    //외부의 실행 신호를 받고 실행을 해야 된다면//
                    //그 신호를 기다리는 델리게이트를 실행한다//
                    //ShowDebug(this.name + " ::: NextAction1_1_2 Method");
                    upDel += NextAction1_1_2;
                }
            }
        }

        //다중 애니메이션을 처리하는 함수를 구현한다.

        bool isEndAnimClips = false;
        float clipTime = 0;
        IEnumerator PlayCharacterAnimation(List<NextCharcterMotion> animList, string position)
        {
            //playerAnimator = transform.GetChild(0).GetComponent<Animator>();
            //ShowDebug(this.name + "---Start Anim Conroutine ::: AnimList Count :::" +animList.Count);
            if (playerAnimator.gameObject.activeSelf == false)
            {
                //Debug.Log(this.name+" is invisable.");
                yield return null;
            }
            else
            {
                //foreach (var fMotion in animList)
                for (int i =0; i<animList.Count; i++)
                {
                    NextCharcterMotion fMotion = animList[i];
                    playerAnimator.Play(fMotion.ToString());
                    yield return null;
                    AnimatorClipInfo[] m_CurrentClipInfo = playerAnimator.GetCurrentAnimatorClipInfo(0);
                    float pTime = m_CurrentClipInfo[0].clip.length;
                    int clpCnt = m_CurrentClipInfo.Length;
                    //ShowDebug(this.name + ":::" + position + " :: Clip Count: " + clpCnt + "___ Action Number: " + actionNum);

                    for (int ii = 0; ii < m_CurrentClipInfo.Length; ii++)
                    {
                        string clipName = m_CurrentClipInfo[ii].clip.name;
                        //ShowDebug(this.name + ":::" + position +" :: ii_Val : " +ii+" :: Clip name: " + clipName);
                    }
                    //이름이 같은 클립을 찾아서 플레이 타임 알아내기//
                    for (int j = 0; j < clpCnt; j++)
                    {
                        if (m_CurrentClipInfo[j].clip.name == fMotion.ToString())
                        {
                            pTime = m_CurrentClipInfo[j].clip.length;
                            break;
                        }
                    }

                    //ShowDebug(this.name + " - PlayBack Time: " + pTime);
                    yield return new WaitForSeconds(pTime);
                }
            }
            //ShowDebug(this.name +" ::: " +position +"Character Anim End");

            isEndAnimClips = true;
        }

        //아래 변수는 회부 신호를 기다릴지 말지를 결정하는 부분에서 시간을 체크하는데 쓰인다//
        float nAction1_1SpendTime;
        string[] inputTextBubble;
        string[] inputRoleName;
        AudioClip[] audioClip;
        void NextAction1_1_1()
        {
            //ShowDebug(this.name + " ::: NextAction1_1_1 IN");
            if (nAction1_1SpendTime < sa.waitTime01)
            {
                //기다리는 시간만큼 지나지 않았으면//
                //프레임당 시간을 추가하고 대기한다//
                nAction1_1SpendTime += Time.deltaTime;
            }
            else
            {

                //정해진 시간만큰 기다렸다면//
                //nAction1_1SpendTime을 0으로 변경하고 다음 함수를 실행하도록 한다//
                //현재 캐릭터가 액션캐릭터라면//
                //아래 내용(말풍선 보이기)를 실행한다//
                //아니면 다음 델리게이트로 넘어간다//
                if (isActionCharacter == true)
                {
                    //바로 아래의 인풋 텍스트버블이 말칸에 들어갈 내용들을 담고 있는 스트링(텍스트)이다.
                    //이것은 버블이 다 끝난 다음에 실행될 함수를 미리 기록한 것이다.
                    bubbleNextMethod = NextAction1_3;
                    //ShowDebug("시간을 기다렸고::: " + actionNum);
                    //ShowDebug("활성캐릭터라 말풍선을 보여준다.::: " + actionNum);

                    //대사//
                    //Debug.Log("TextBubbleLength:" + sa.textBubble1.Length);
                    inputTextBubble = sa.textBubble1;
                    //화자//
                    inputRoleName = sa.roleName1;
                    //대사사운드//
                    audioClip = sa.textBubble1Audio;

                    bubbleLenth = 0;
                    //ShowDebug("ShowBTextBubble_001");
                    ShowBTextBubble();

                    upDel -= NextAction1_1_1;
                }
                else
                {
                    if (isEndAnimClips == true)
                    {
                        //액션 캐릭터가 아니라면 이부분을 실행한다//
                        //이제 말풍선을 다 실행했으니 질문내용으로 넘어간다//
                        //ShowDebug("활성캐릭터가 아니니:::그다음으로 넘어간다::: " + actionNum);
                        isEndAnimClips = false;
                        nAction1_1SpendTime = 0;
                        //ShowDebug(this.name + " ::: NextAction1_3 Method");
                        NextAction1_3();
                        upDel -= NextAction1_1_1;
                    }
                }
                //기능을 다한 이 함수는 제거한다//
            }
        }

        void NextAction1_1_2()
        {
            //ShowDebug("NextAction1_1_2 IN");
            //외부에서 신호가 들어오면 isInputActionB 이부분이 false가 된다//
            //그러게 되면 현재 함수를 델리게이트에서 제거하고//
            //시간을 기다리는 부분을 실행하여 준다//
            if (sa.isInputActionB == false)
            {
                upDel += NextAction1_1_1;
                upDel -= NextAction1_1_2;
            }
        }

        //1차 텍스트 버블을 생성과 소멸을 담당하는 코드이다.
        int bubbleLenth;
        Del bubbleNextMethod;
        GameObject tBubble;
        IEnumerator ShowTextBubble()
        {
            yield return new WaitForSeconds(0.1f);
            tBubble.SetActive(true);
            //ShowDebug("Show Tbubble1 :: " + tBubble.activeSelf + " ____ " + tBubble.name);
        }

        void ShowBTextBubble()
        {
            //ShowDebug("ShowBTextBubble에 들어옴~ :: Action Number: "+actionNum+" ::");
            if (isActionCharacter == true)
            {
                //ShowDebug("Action Character: TRUE");
                //ShowDebug("bubbleLenth: "+ bubbleLenth+ " ::: inputTextBubble.Length: "+ inputTextBubble.Length);
                //ShowDebug("활성캐릭터면 말풍선을 보여준다::: " + actionNum);
                if (inputTextBubble != null && bubbleLenth < inputTextBubble.Length)
                {
                    if (inputTextBubble[bubbleLenth] != "" && inputTextBubble[bubbleLenth] != " " && inputTextBubble[bubbleLenth] != null)
                    {
                        tBubble = wlScene.textBubble[bubbleLenth % 2];

                        //버블을 보이고//
                        //ShowDebug("Show Tbubble1");
                        StartCoroutine(ShowTextBubble());
                        //tBubble.SetActive(true);

                        //대사를 넣고//
                        tBubble.transform.Find("ViewPort/Content/Text").GetComponent<Text>().text = inputTextBubble[bubbleLenth];

                        //이건 정상부분//
                        tBubble.transform.Find("Role").GetComponent<Text>().text = inputRoleName[bubbleLenth];

                        mainAudio.clip = null;
                        float clipLength = 0;

                        #region //음성파일이 있으면 음성파일의 길이를 넣어준다//
                        if (audioClip[bubbleLenth] != null)
                        {
                            mainAudio.clip = audioClip[bubbleLenth];
                            clipLength = audioClip[bubbleLenth].length;
                            mainAudio.loop = false;
                        }
                        #endregion

                        tBubble.GetComponent<AnimShowHide>().GetSoundA(mainAudio, clipLength, isActionCharacter, actionNum);

                        //음성을 출력한다//
                        //mainAudio.Play();
                        //말풍선 하나가 끝난다음에 실행할 코드(함수)를 미리 이곳에 심어놓는다//
                        //Debug.Log("WaitBubble 추가");
                        tBubble.transform.GetComponent<AnimShowHide>().InDel(WaitBubble);
                    }
                    else
                    {
                        //Debug.Log("Null 또는 빈칸입니다::: " + actionNum);
                        if (isActionCharacter == true)
                            upDel += WaitNoneBubble;
                    }
                    bubbleLenth++;
                    //ShowDebug("Increase Bubble Count: "+ bubbleLenth);
                }

                //모든 말풍선 내용을 실행했으면 아래의 함수를 실행한다//
                else
                {
                    //이제 말풍선을 다 실행했으니 질문내용으로 넘어간다//
                    //ShowDebug("질답 끝2 bubbleNextMethod::: " + actionNum);
                    bubbleNextMethod();
                    //더이상 쓰이지 않는 당 함수를 제거한다.//
                    upDel -= WaitNoneBubble;
                }
            }
            else
            {
                //ShowDebug("애니메이션 끝날 때 까지 대기"+ isEndAnimClips);
                //다음 액션을 위한 준비를 하는 곳//
                if (isEndAnimClips == true)
                {
                    //ShowDebug("질답 끝1 bubbleNextMethod::: " + actionNum);
                    //이제 말풍선을 다 실행했으니 질문내용으로 넘어간다//

                    bubbleNextMethod();

                    upDel -= WaitNoneBubble;

                    nAction1_1SpendTime = 0;
                    isEndAnimClips = false;
                }
            }
        }

        public void WaitBubble()
        {
            //ShowDebug("WaitBubble 실행");
            if (isActionCharacter == true)
            {
                //ShowDebug("ShowBTextBubble_002");
                ShowBTextBubble();
            }
        }

        public float noneBubbleWaitTime = 0.1f;
        float noneBubbleSpendTime = 0f;

        void WaitNoneBubble()
        {
            if (noneBubbleSpendTime < noneBubbleWaitTime)
            {
                noneBubbleSpendTime += Time.deltaTime;
            }
            else
            {
                noneBubbleSpendTime = 0f;
                //다음에 불러질 함수를 바로 아래에 기록한다.//
                if (isActionCharacter == true)
                {
                    //ShowDebug("ShowBTextBubble_003");
                    ShowBTextBubble();
                }
                upDel -= WaitNoneBubble;
            }
        }


        //연결된 타캐릭터를 움직이게 해준다//
        void AllowOtherChAction(InputAction[] inAction)
        {
            //Debug.Log("타 캐릭터 컨트롤 직전 길이: " + inAction.Length);
            if (inAction.Length > 0)
                for (int i = 0; i < inAction.Length; i++)
                {
                    int saNum = inAction[i].inputActionNum;
                    //값이 false여야만 다른 캐릭터가 움직인다//
                    inAction[i].otherSA.setAction[saNum].isInputActionB = false;
                    //Debug.Log(
                    //    "Ch Name: " + inAction[i].otherSA.transform.name +
                    //    " __ isInputActionB: " + inAction[i].otherSA.setAction[saNum].isInputActionB
                    //    );
                }
        }

        // 인액션만 실행하는 함수//
        // 바로 아래에 있는 함수에서 그부분만 떼어놓은 것//
        // 외부에서 움직임을 가져올경우(타인이 캐릭터를 조작하는 경우)
        // 액션넘버가 바뀔경우 타 캐릭터에 인액션정보를 전달해야 되서 필요한 부분//
        public void ChangeActionNumSendInAction()
        {
            //isWaitAddFunction 이 값이 false일때 아래 내용이 실행되게 한다.
            if (isWaitAddFunction == false)
            {
                //ShowDebug("NextAction1 Method 02");
                NextAction1();
                AllowOtherChAction(sa.inAction1);
                AllowOtherChAction(sa.inAction2);
            }
            else
            {
                Debug.Log("Waiting a Function Added!_001");
                StartCoroutine(WaitTime(ChangeActionNumSendInAction));
            }
        }

        public void ChangeActionNumSendInActionNum1()
        {
            //isWaitAddFunction 이 값이 false일때 아래 내용이 실행되게 한다.
            if (isWaitAddFunction == false)
            {
                AllowOtherChAction(sa.inAction1);
            }
            else
            {
                StartCoroutine(WaitTime(ChangeActionNumSendInActionNum1));
            }
        }

        public void ChangeActionNumSendInActionNum2()
        {
            //isWaitAddFunction 이 값이 false일때 아래 내용이 실행되게 한다.
            if (isWaitAddFunction == false)
            {
                AllowOtherChAction(sa.inAction2);
            }
            else
            {
                //Debug.Log("Waiting a Function Added!_003");
                StartCoroutine(WaitTime(ChangeActionNumSendInActionNum2));
            }
        }



        IEnumerator WaitTime(UnityEngine.Events.UnityAction uAction)
        {
            yield return new WaitForSeconds(0.1f);
            if (isActionCharacter == false) isWaitAddFunction = false;
            uAction();
        }

        //질문지를 보여주는 함수//
        void NextAction1_3()
        {
            //ShowDebug(this.name + " ::: NextAction1_3 IN");

            //ShowDebug("질문지를 보여주는 부분이다::: " + actionNum);

            //타캐릭터의 동작을 제어할때//
            //ShowDebug("첫번째 타캐릭터 제어를 한다::: " + actionNum);

            if (sa.inAction1 == null)
                Debug.Log("Sa.Action is NULL");
            else
            {
                //Debug.Log("sa.inAction1 Length" + sa.inAction1.Length);
                ChangeActionNumSendInActionNum1();
            }

            //액션 캐릭터일 경우는 질문을 보여야 되니 아래 내용을 실행토록 한다.
            if (isActionCharacter == true)
            {
               // ShowDebug("활성캐릭터면 질문지를 보여준다: " + name + " ::: " + actionNum);

                if (sa.question != "")
                {
                    wlScene.qPanel.SetActive(true);
                    wlScene.uiBG.SetActive(true);
                    wlScene.que.GetComponent<Text>().text = sa.question;

                    int correctNum = 0;
                    if (ConstDataScript.modeNum == 1)
                        for (int i = 0; i < sa.correct.Count; i++)
                            if (sa.correct[i] == true)
                                correctNum = i;

                    for (int j = 0; j < wlScene.ans.Length; j++)
                    {
                        int val = j;
                        int acVal = actionNum;
                        wlScene.ans[j].GetComponent<Text>().text = sa.answers[j];
                        wlScene.ans[j].transform.parent.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                        wlScene.ans[j].transform.parent.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { SolveQue1(val, acVal); });
                        if (ConstDataScript.modeNum == 1)
                            if (correctNum != j)
                                wlScene.ans[j].transform.parent.GetChild(0).GetComponent<Button>().enabled = false;
                            else
                                wlScene.ans[j].transform.parent.GetChild(0).GetComponent<Button>().enabled = true;
                    }
                }
                else
                {
                    //Debug.Log("질문이 없다. ::: " + actionNum);
                    //Because There ane no Questions.
                    //it dosen't answer for the Question. 
                    //ShowDebug("활성 캐릭터이나 질문이 없어서 넘어감");
                    SolveQue2();
                }
            }
            //액션 캐릭터가 아니면 바로 다음으로 넘어간다.
            else
            {
                //ShowDebug("활성 캐릭터가 아니라서 넘어감");
                SolveQue2();
            }
        }

        //답을 선택하면 틀렸는지 맞았는지 확인하는 부분//
        public void SolveQue1(int num, int acNum)
        {
            // Debug.Log("버튼 누름" + num + "====" + acNum);
            if (isActionCharacter == true)
            {
                //Debug.Log("버튼누름 소리를 낸다 - 시작");
                //버튼누름 소리를 낸다//
                mainAudio.loop = false;
                mainAudio.clip = click;
                mainAudio.Play();
                //Debug.Log("버튼누름 소리를 낸다 - 끝");
            }

            //혹시 동시에 두번이상 들어오면 튕겨내도록 만들어 놓은 IF문

            if (isSoveque == true) return;

            isSoveque = true;

            Debug.Log("Answer: " + num);
            if (sa.correct[num] == true)
                Debug.Log("Correct!" + " ::: " + acNum);
            else
                Debug.Log("Rong!" + " ::: " + acNum);

            wlScene.qPanel.SetActive(false);
            wlScene.uiBG.SetActive(false);

            //모든 리스너의 값을 초기화 한다//

            //ShowDebug("질답후 다음으로 넘어간다" + " ::: " + actionNum);
            SolveQue2();
        }



        //데모이거나 액션 캐릭터가 아닐때는 바로 이곳으로 이동한다//
        //액션캐릭터일 경우에는 solveQue1을 먼저 실행하고 이부분으로 들어온다//
        void SolveQue2()
        {
            if (sa.inAction2 == null) Debug.Log("sa.inAction2 is  NULL");
            else
                //ShowDebug("다른 캐릭터의 행동을 전달할때: " + sa.inAction2.Length + " ::: " + actionNum);

            //말풍선에 들어갈 대화들이다.
            //말풍선이 다끝나면 실행될 함수이다.
            bubbleNextMethod = NextAction2;
            //말풍선 함수를 실행한다.
            //ShowDebug("두번째 말풍선을 실행한다" + " ::: " + actionNum);
            if (isActionCharacter == true)
            {
                //대사//
                inputTextBubble = sa.textBubble2;
                //화자//
                inputRoleName = sa.roleName2;
                //사운드//
                audioClip = sa.textBubble2Audio;
                bubbleLenth = 0;
                //ShowDebug("ShowBTextBubble_004");
                ShowBTextBubble();
            }
            else
            {
                //ShowDebug("액션 캐릭터가 아니면 바로 다음 행동으로 간다" + " ::: " + actionNum);
                //ShowDebug("NextAction2 Method");
                NextAction2();
            }
        }
        int pointCount = 0;

        // 이동 위치를 표시하는 화살표를 담을 그릇 //
        GameObject arrows;
        void NextAction2()
        {
            //ShowDebug("NextAction2 IN");

            if (sa.isMeA == true)                aClass.MeA(isActionCharacter);

            if (sa.isMeB == true)                aClass.MeB(isActionCharacter);

            if (sa.isMeC == true)                aClass.MeC(isActionCharacter);

            if (sa.isWaterOff == true)
            {
                aClass.isWater = false;
                aClass.fireWater.SetActive(false);
            }

            //Debug.Log("텍스트 내용을 비운다.");
            inputTextBubble = null;

            if (sa.inAction2 == null)
                Debug.Log("Sa.Action is NULL");
            else
                ChangeActionNumSendInActionNum2();

            // 그 어느것도 아니고 단순히 NPC 일때.
            // 이때는 줄 따라 이동한다.
            if (!isCharacter && sa.movePath != null && isFreeMov == false)
            {
                ///////////////////////////////////////////////////////////////////
                // 화살표를 보여주기 위한 코드를 여기서 실행한다.                //
                // 현재 캐릭터가 PC 일 경우에 화살표를 보여주는 코드를 실행한다. //
                if (isActionCharacter == true)
                {
                    Transform mpTransform = sa.movePath.transform;
                    Debug.Log("패쓰들이 묶여있는 상위 오브젝트: " + mpTransform.name);
                    if (mpTransform.parent.Find(mpTransform.name + "_Arrows") != null)
                    {
                        arrows = mpTransform.parent.Find(mpTransform.name + "_Arrows").gameObject;
                        arrows.SetActive(true);
                    }
                }
                // 화살표를 보여주기 위한 코드를 여기서 실행한다.                //
                ///////////////////////////////////////////////////////////////////

                //ShowDebug("무빙을 한다" + " ::: " + actionNum);

                if (isActionCharacter == true)
                {
                    //발자국소리를 낸다//
                    AudioClip walk = Resources.Load<AudioClip>("Actions/Sounds/WalkNormal");
                    mainAudio.loop = true;
                    mainAudio.clip = walk;
                    mainAudio.Play();
                }

                #region 여기서 애니메이션의 이름을 가지고 와서 플레이 할 수 있게 한다.- 새로운 방식-여러개의 애니메이션을 로드할 수 있다.-
                if (playerAnimator != null)
                {
                    //ShowDebug(this.name+ " :: Next Motion");
                    StartCoroutine(PlayCharacterAnimation(sa.nChMotionNew, "Next Motion"));
                }
                #endregion

                currentSpeed = 0;
                currentWayPointID = 0;

                pointCount = sa.movePath.pathObjs.Count;

                rotation = Quaternion.LookRotation(sa.movePath.pathObjs[1].position - sa.movePath.pathObjs[0].position);
                startPos = transform.position;
                nextPos = sa.movePath.pathObjs[1].position;
                moveLerpVal = 0f;
                snDist = Vector3.Distance(startPos, nextPos);
                upDel += MoveAction;

                //움직임 속도 정보를 가지고 와서 세팅을 해준다.
                switch (sa.movePath.moveSpeed)
                {
                    case CharacterSpeed.slow:
                        speed = ConstDataScript.mSlow;
                        break;
                    case CharacterSpeed.normal:
                        speed = ConstDataScript.mNormal;
                        break;
                    case CharacterSpeed.high:
                        speed = ConstDataScript.mHigh;
                        break;
                }
            }
            //다음 캐릭터 무브가 없을때//
            else if (sa.movePath == null)
            {
                //ShowDebug("무브액션없이 다음으로" + " ::: " + actionNum);
                currentSpeed = 0;
                if (actionNum + 1 < setAction.Length)
                {
                    if (this.name == "001_Captain")
                        isChangeActionNum = true;
                    //새로운형식.
                    AddActionNum();
                }
                else
                {
                    //Debug.Log(this.name + "캐릭터 액션을 종료합니다. 마지막 액션 넘버: " + actionNum + " ::: " + actionNum);
                    this.enabled = false;
                }
            }

            // 자유이동을 위해서 추가한 코드.(20180530)
            // 프리무브가 활성화 되면 다음 넥스트 포인트를 보여준다.
            // 액션 캐릭터이고 온라인이 되어서 프리무브가 활성화 될때.
            else if (isActionCharacter == true && isFreeMov == true)
            {
                //Debug.Log("넥스트 포인트 보임");
                nextPointers[nextPointVal].SetActive(true);
            }
        }

        void AddActionNum()
        {
            isEndAnimClips = false;
            #region 여기서 애니메이션의 이름을 가지고 와서 플레이 할 수 있게 한다.- 새로운 방식-여러개의 애니메이션을 로드할 수 있다.-
            if (playerAnimator != null)
            {
                //ShowDebug(this.name + " :: Next Motion");
                StartCoroutine(PlayCharacterAnimation(sa.nChMotionNew, "Next Motion"));
            }
            #endregion

            //ShowDebug(this.name+":::AddActionNum Method Run:::Action Number: "+actionNum);
            AddActionNum2();
        }

        void AddActionNum2()
        {
            if (isWaitAddFunction == false && isEndAnimClips == true)
            {
                upDel -= MoveAction;
                isSoveque = false;

                // 다음 액션으로 넘어간다.
                actionNum++;
                //ShowDebug("Increase Action Number: " + actionNum);
                //ShowDebug("NextAction1 Method 03");
                NextAction1();
            }
            else
            {
                StartCoroutine(WaitTime(AddActionNum2));
            }
        }

        //마지막에 종료 관련 데이터를 서버로 보낸다.
        public void SendCompleteDataToServer()
        {
            ClientLobbyServerConnect cServerCon = GameObject.Find("ControlSystem").GetComponent<ClientLobbyServerConnect>();
            SendEnd sEnd = new SendEnd
            {
                roleNum = ConstDataScript.roleNum.ToString(),
                uid = ConstDataScript.uid,
                actionClear = "true"
            };
            Debug.Log("서버로 종료정보보냄");
            cServerCon.Alarmclear(sEnd);
        }

        public void EndAction()
        {
            isEndRot = false;
            if (sa.isWaterOn == true)
            {
                aClass.isWater = true;
            }

            upDel -= MoveAction;
            currentSpeed = 0;
            currentWayPointID = 0;
            if (actionNum + 1 < setAction.Length)
            {
                isChangeActionNum = true;
                //새로운형식.
                AddActionNum();
                // 아래 주석 부분은 구형식.
                // 새로운 형식에(바로 위 호출되는 함수 참고)선.
                // 특수동작이 있을경우 그것을 실행해야만 액션넘버가.
                // 늘어나도록 수정.
            }
            else
            {
                Debug.Log(this.name + "캐릭터 액션을 종료합니다. 마지막 액션 넘버: " + actionNum);

                wlScene.uiBG.SetActive(true);
                wlScene.backToMain.SetActive(true);
            }

            // 넥스트 포인트를 꺼주고 다음 포인트를 보여주기 위해 넥스트 포인트  밸류를 +1한다.
            if (isActionCharacter == true)
            {
                if (nextPointers.Count > (nextPointVal + 1))
                {
                    //Debug.Log("nextPointVal ::: " + nextPointVal);
                    nextPointers[nextPointVal].SetActive(false);
                    nextPointVal++;
                }
            }
        }

        void EndRot()
        {
            lerpV = Vector3.Lerp(startRot.eulerAngles, endRot.eulerAngles, by);
            if (by >= 1f)
            {
                EndAction();
                upDel -= EndRot;
            }
            transform.eulerAngles = lerpV;
            by += Time.deltaTime / 2f;
        }

        // 자유 이동을 하면 필요한 넥스트 포인터를 모아 놓기 위한 변수.
        // 이변수는 외부에서 불러오게 하지 않고 바로 입력시킨다.
        public List<GameObject> nextPointers = new List<GameObject>();
        int nextPointVal = 0;
        float exDist;
        bool isEndRot = false;

        Vector3 startPos;
        Vector3 nextPos;
        float snDist;
        float moveLerpVal = 0;
        float rotVal =0;
        void MoveAction()
        {
            if (currentSpeed < speed)
                currentSpeed += speed * Time.deltaTime;
            else
                currentSpeed = speed;

            if (currentWayPointID + 1 <= pointCount)
                distance = Vector3.Distance(sa.movePath.pathObjs[currentWayPointID].position, transform.position);

            moveLerpVal +=  (speed * Time.deltaTime)/ snDist;

            transform.position = Vector3.Lerp(startPos, nextPos, moveLerpVal);

            rotVal += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotVal);
            angle = transform.eulerAngles;
            transform.eulerAngles = new Vector3(0, angle.y, 0);

            exDist = dist;
            dist = Vector3.Distance(transform.position, sa.movePath.pathObjs[sa.movePath.pathObjs.Count - 1].position);

            if (moveLerpVal >= 1f)
            {
                if (this.name.Contains("002"))
                {
                    Debug.Log("currentWayPointID + 1 === "+ currentWayPointID + 1);
                    Debug.Log("pointCount ::: "+ pointCount);

                }

                if ((currentWayPointID + 1) >= pointCount)
                {

                    // 여기서 부터 디버그 해보자.
                    // 멈추는 위치만 알아내면 됨.
                    //Debug.Log("크다!!! 111");
                    if (isActionCharacter == true)
                    {
                        mainAudio.Stop();
                    }
                    startRot = transform.rotation;
                    endRot = sa.movePath.pathObjs[sa.movePath.pathObjs.Count - 1].rotation;

                    if (arrows != null) arrows.SetActive(false);
                    currentWayPointID = 0;
                    //Debug.Log("크다!!! 222");

                    by = 0;
                    upDel += EndRot;
                    upDel -= MoveAction;
                }
                else
                {
                    // 아래 조건문은 확인용.
                    // 테스트가 끝나면 삭제.
                    if (this.name.Contains("002")) Debug.Log(" ::: moveLerpVal 값3: "+ moveLerpVal);

                    moveLerpVal = 0;
                    startPos = transform.position;
                    //Debug.Log("거리 초기화");
                    currentWayPointID++;
                    
                    // 아래 조건문은 확인용.
                    // 테스트가 끝나면 삭제.
                    if (this.name.Contains("002")) Debug.Log("웨이포인트값 1 증가");

                    nextPos = sa.movePath.pathObjs[currentWayPointID].position;
                    dist = 0;
                    exDist = 10000;
                    snDist = Vector3.Distance(startPos, nextPos);
                    rotVal = 0;
                    rotation = Quaternion.LookRotation(sa.movePath.pathObjs[currentWayPointID].position - sa.movePath.pathObjs[currentWayPointID - 1].position);
                }
            }
        }

        public GameObject TriggerEnterObj
        {
            set;
            get;
        }
    }

}