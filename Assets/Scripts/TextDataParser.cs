using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;
using System;


namespace DataParser
{
    public class TextDataParser : MonoBehaviour
    {
        string actiontextPath = "Actions/Texts/";
        string actionSoundPath = "Actions/Sounds/";
        public string fileName = "act_001_text_data";
        string language;
        string langEng = "_en";
        string langKor = "_kr";

        string loadedBaseData;

        MainController mainCon;
        MoveOnPathScript mPath;

        public List<Dictionary<string, object>> data;
        public void UserAwake()
        {
            //Debug.Log("UserAwake Run :: "+ this.name);
            //if (this.name == "005_EngineerChief")
            //{
            //    //Debug.Log("005_EngineerChief Data :: " + data);
            //    Debug.Log("005_EngineerChief 유저 어웨이크 시작: " + this.transform.name);
            //}

            //Debug.Log("유저 어웨이크 시작: "+this.transform.name);
            mainCon = GameObject.FindWithTag("MainCon").GetComponent<MainController>();
            mPath = GetComponent<MoveOnPathScript>();
            LoadOriginalData();
        }
        void LoadOriginalData()
        {
            //한영 구분을 먼저 한다음//
            switch (ConstDataScript.langNum)
            {
                case 0:
                    language = langEng;
                    break;
                case 1:
                    language = langKor;
                    break;
            }
            //Debug.Log("Lang Num: " + ConstDataScript.langNum);
            //적절한 경로를 생성한다(텍스트)
            string loadDataPath = actiontextPath + fileName + language;
            //string loadDataPath = fileName + language;
            //경로를 이용하여 데이터를 로드한다.//

            //참고인덱스//
            //ActionNumber TextBubble1 textBubble1Audio Question    Answers Correct TextBubble2 textBubble2Audio

            //csv 데이터를 사용할 수 있게 파싱한다//
            //Debug.Log("loadDataPath :: "+ loadDataPath);
            data = CSVReader.Read(loadDataPath);

            if(this.name == "005_EngineerChief")
            {
                //Debug.Log("005_EngineerChief Data :: "+ data);
            }
            //if (this.name == "005_EngineerChief")
            //{
            //    for (int i = 0; i < data.Count; i++)
            //    {
            //        Debug.Log("Index :: " + i + "-----" + data[i]["TextBubble1"]);
            //        Debug.Log("Index :: " + i + "-----" + data[i]["WaitTime01"]);
            //    }
            //}

            InputActionData();
        }

        void InputActionData()
        {
            //Debug.LogError(this.name+ " :: Data Count: " + data.Count);

            //액션의 총 길이를 가지고 와서 setAction을 초기화 한다//
            mPath.setAction = new SetActionPointerInfoForCharacter[data.Count];

            for (int actionNumber = 0; actionNumber < data.Count; actionNumber++)
            {
                mPath.setAction[actionNumber] = new SetActionPointerInfoForCharacter();

                //대기시간세팅 WaitTime01//
                //Debug.Log("ActionNum :: "+ actionNumber + " ___ Wait Time01: " + data[actionNumber]["WaitTime01"]);
                mPath.setAction[actionNumber].waitTime01 = float.Parse(data[actionNumber]["WaitTime01"].ToString());

                //시작시 애니 세팅//


                //이 부분은 정리가 되면 삭제할 예정.
                #region 기존 애니세팅-하나의 동작만 표시한 경우.-현재 주석 처리-문제발생시 복구- 새로운 코드는 바로 아래 작성-
                //string getAnimName = data[actionNumber]["FChMotion"].ToString();
                //NextCharcterMotion cMotion = (NextCharcterMotion) Enum.Parse(typeof(NextCharcterMotion), getAnimName);

                //mPath.setAction[actionNumber].fChMotion = cMotion;
                //Debug.Log("F Motion: " + mPath.setAction[actionNumber].fChMotion);
                #endregion
                //이 부분은 정리가 되면 삭제할 예정.

                /////////////////////////////////////////////////////////////////////////////////
                ////이 부분을 정리하여 한번에 여러개의 애니메이션을 부르는 기능을 작동시키자.////
                //이 부분을 정리하여 한번에 여러개의 애니메이션을 부르는 기능을 작동시키자.
                #region 새로운 애니세팅-여러개의 동작을 불러와서 표시할 경우.

                string[] getAnimNameNew = data[actionNumber]["FChMotion"].ToString().Split('|');
                //Debug.Log(this.name + " - First Anim Count: " + getAnimNameNew.Length);
                mPath.setAction[actionNumber].fChMotionNew = new List<NextCharcterMotion>();
                // 애니 이름을 받을 이넘 변수를 임시로 하나 세팅 한다.
                for (int i = 0; i < getAnimNameNew.Length; i++)
                {
                    //Debug.Log(this.name+ " I Value: "+i);
                    //Debug.Log("First Anim Name: " + getAnimNameNew[i]);
                    NextCharcterMotion cMotionNew = (NextCharcterMotion)Enum.Parse(typeof(NextCharcterMotion), getAnimNameNew[i]);
                    //Debug.Log("First This Name: " + cMotionNew);
                    mPath.setAction[actionNumber].fChMotionNew.Add(cMotionNew);
                }
                #endregion
                ////이 부분을 정리하여 한번에 여러개의 애니메이션을 부르는 기능을 작동시키자.////
                /////////////////////////////////////////////////////////////////////////////////

                //첫번째 직책, 말풍선을 세팅한다//
                //말풍선에 맞는 소리를 불러온다//
                GetTextBubble(data[actionNumber]["TextBubble1"].ToString(),
                    actionNumber,
                    "TextBubble1",
                    out mPath.setAction[actionNumber].roleName1,
                    out mPath.setAction[actionNumber].textBubble1,
                    out mPath.setAction[actionNumber].textBubble1Audio);
                

                #region 퀘스쳔//
                mPath.setAction[actionNumber].question = data[actionNumber]["Question"].ToString();

                //Debug.Log("질문"+ actionNumber + "::: "+ mPath.setAction[actionNumber].question);
                #endregion 퀘스쳔//

                #region 앤서스 질답문//
                string[] answers = data[actionNumber]["Answers"].ToString().Split('|');

                if (answers.Length < 1 || answers[0] != "")
                {
                    mPath.setAction[actionNumber].answers = new List<string>();

                    for (int j = 0; j < answers.Length; j++)
                    {
                        //Debug.Log("답: " + j + " :: " + answers[j]);
                        mPath.setAction[actionNumber].answers.Add(answers[j]);
                    }
                }
                #endregion 앤서스 질답문//

                #region 코렉트 정답//
                if (data[actionNumber]["Correct"].ToString() != "")
                {
                    int correctBool = (int)data[actionNumber]["Correct"];
                    mPath.setAction[actionNumber].correct = new List<bool>();
                    for(int cs = 0; cs< 3; cs++)
                    {
                        mPath.setAction[actionNumber].correct.Add(false);
                    }

                    //Debug.Log("====================================");
                    //Debug.Log("140==============================140");
                    //Debug.Log("======::"+this.name+"::===========");
                    //Debug.Log("140==============================140");
                    //Debug.Log("====================================");
                    //Debug.Log("ActionNumber :: "+ actionNumber);

                    mPath.setAction[actionNumber].correct[correctBool] = true;
                }
                #endregion 코렉트 정답//

                // 두번째 직책, 말풍선을 세팅한다//
                // 말풍선에 맞는 소리를 불러온다//
                GetTextBubble(
                    data[actionNumber]["TextBubble2"].ToString(),
                    actionNumber,
                    "TextBubble2",
                    out mPath.setAction[actionNumber].roleName2,
                    out mPath.setAction[actionNumber].textBubble2,
                    out mPath.setAction[actionNumber].textBubble2Audio);

                //액션 마지막 애니 세팅//
                #region 예전것.
                //string getAnimName2 = data[actionNumber]["NChMotion"].ToString();
                //NextCharcterMotion nMotion = (NextCharcterMotion)Enum.Parse(typeof(NextCharcterMotion), getAnimName2);
                //mPath.setAction[actionNumber].nChMotion = nMotion;
                //Debug.Log("N Motion: " + mPath.setAction[actionNumber].nChMotion);
                #endregion
                #region 여러개를 받아드릴 수 있도록 수정한 것.
                string[] getAnimNameNew2 = data[actionNumber]["NChMotion"].ToString().Split('|');
                //Debug.Log(this.name + " - Next Anim Count: " + getAnimNameNew2.Length);
                mPath.setAction[actionNumber].nChMotionNew = new List<NextCharcterMotion>();
                // 애니 이름을 받을 이넘 변수를 임시로 하나 세팅 한다.
                for (int i = 0; i < getAnimNameNew2.Length; i++)
                {
                    //Debug.Log(this.name + " I Value: " + i);
                    //Debug.Log("Next Anim Name: " + getAnimNameNew2[i]);
                    NextCharcterMotion cMotionNew = (NextCharcterMotion)Enum.Parse(typeof(NextCharcterMotion), getAnimNameNew2[i]);
                    //Debug.Log("Next This Name: " + cMotionNew);
                    mPath.setAction[actionNumber].nChMotionNew.Add(cMotionNew);
                }
                #endregion


                //스탠딩 포지션 세팅//
                //Debug.Log(this.name + " ::: STANDING POSITION SETTING ::");

                mPath.setAction[actionNumber].standingPosition = null;
                string getStPoistion = data[actionNumber]["StandingPosition"].ToString();
                if (getStPoistion != "" && getStPoistion != " " && getStPoistion != null)
                {
                    //Debug.Log(this.name + " ::: Get Position:" + getStPoistion + "::");
                    string path = getStPoistion;

                    if (mainCon.isPointsCsvData == false)
                        mPath.setAction[actionNumber].standingPosition = mainCon.actionPointers.Find(this.name + "/" + path);
                    else if (mainCon.isPointsCsvData == true)
                        mPath.setAction[actionNumber].standingPosition = mainCon.actionPointers.Find(path);
                }
                else
                {
                    //Debug.Log(this.name + " ::: Get Position is failed!");

                    mPath.setAction[actionNumber].standingPosition = null;
                }

                //경로 세팅//
                //패쓰에 설명을 적고 싶으면 괄호를() 열어서 필요한 내용을 적으면 된다//
                //'(' 이것 뒤의 내용들은 설명을 위해 적은 것이기에 다 삭제하고 배열 0번의 문자만 가지고 와서 적용하면 path이름이 된다//
                string[] pathString = data[actionNumber]["MovePath"].ToString().Split('(');
                string getPath = pathString[0];
                //Debug.Log(this.name+ " ::: Get Path: "+ getPath);
                if (mainCon.isPointsCsvData == false)
                    mPath.setAction[actionNumber].movePath = mainCon.actionPointers.Find(this.name + "/" + getPath).GetComponent<EditorPathScript>();
                else if (mainCon.isPointsCsvData == true)
                    mPath.setAction[actionNumber].movePath = mainCon.actionPointers.Find(getPath).GetComponent<EditorPathScript>();


                // 신형 방식-
                // 2018.8월 추가된 신기능 액션정보를 Enum으로 지정한다.(InAction 연결방식 변경으로 필요한 부분)
                // 먼저 액션넘버로 연결되는 것을 대체하는 액션이넘을 설정하여준다.
                mPath.setAction[actionNumber].aEnum = (ActionEnum)Enum.Parse(typeof(ActionEnum), data[actionNumber]["ActionEnum"].ToString());


                // 그외 처리되는 버추얼 메소드와 물뿌리는 기능//
                mPath.setAction[actionNumber].isMeA = Convert.ToBoolean(data[actionNumber]["IsMeA"]);
                mPath.setAction[actionNumber].isMeB = Convert.ToBoolean(data[actionNumber]["IsMeB"]);
                mPath.setAction[actionNumber].isMeC = Convert.ToBoolean(data[actionNumber]["IsMeC"]);

                mPath.setAction[actionNumber].isWaterOn = Convert.ToBoolean(data[actionNumber]["IsWaterOn"]);
                mPath.setAction[actionNumber].isWaterOff = Convert.ToBoolean(data[actionNumber]["IsWaterOff"]);
            }

            // 인액션 연결은 약간 시간을 늦춰서 처리하게 한다.
            // 이렇게 해야 다른 캐릭터들의 기본액션정보가 입력된 다음에 처리가 되게 된다.
            StartCoroutine(InActionIEsetting());
        }

        // 인액션 연결은 다른 캐릭터들도 기본 세팅이 끝난다음에 해야되니 이부분은 약간의 시간차를 두어서 실행되게 한다.
        IEnumerator InActionIEsetting()
        {
            // 여기서 data는 캐릭터 하나의 시나리오에 들어있는 액션의 총량이다.
            //Debug.Log("Data Count ::: "+ data.Count);
            yield return new WaitForSeconds(0.1f);
            for (int actionNumber = 0; actionNumber < data.Count; actionNumber++)
            {
                // 인액션1 세팅//
                GetInAction(data[actionNumber]["InAction1"].ToString(), out mPath.setAction[actionNumber].inAction1);
                // 인액션2 세팅//
                GetInAction(data[actionNumber]["InAction2"].ToString(), out mPath.setAction[actionNumber].inAction2);
            }
        }


        /// <summary>
        /// 텍스트 버블 세팅 그리고 말풍선에 맞는 소리도 불러온다. //
        /// </summary>
        /// <param name="textData"></param>
        /// <param name="actionNumber"></param>
        /// <param name="bubbleName"></param>
        /// <param name="roleName"></param>
        /// <param name="textBubble"></param>
        /// <param name="textBubbleAudio"></param>
        void GetTextBubble(string textData,
            int actionNumber,
            string bubbleName,
            out string[] roleName,
            out string[] textBubble,
            out AudioClip[] textBubbleAudio)
        {
            // csv에서 불러올 데이터를(미가공, 롤과 버블, 그리고 오디오 정보가 있다)
            // 담기위한 그릇으로 list dic을 선언한다.
            List<Dictionary<string, object>> dataBubble;

            // 데이터가 있다는 신호인 true가 적혀있으면 
            // 바로 아래의 내용을 실행하여 텍스트버블만 불러오는 코드를 실행한다.
            if(textData.Contains("true"))
            {
                string loadDataPath = actiontextPath + fileName +"_"+ bubbleName + language;
                //Debug.Log("Load Data Path: "+ loadDataPath);
                dataBubble = CSVReader.Read(loadDataPath);

                //if(dataBubble == null)
                //{
                //    Debug.LogError(this.name+ " :: 텍스트 데이타가 없습니다.");
                //}
                //else
                //{
                //    Debug.Log(this.name + " :: 텍스트 데이타가 있습니다.");
                //    Debug.Log("데이터 이름은 " + loadDataPath + " 입니다.");
                //}

                bool isCheck = false;
                int actionNumStartPnt = 0;
                int actionNumEndPnt = 0;

                // 액션넘버와 텍스트 버블의 액션 넘버를 비교하여 같은 부분만 찾아낸다.
                // 범위 지정 시점: actionNumStartPnt
                // 종점: actionNumEndPnt
                for (int i = 0; i <dataBubble.Count; i++)
                {
                    int fromActionNum = int.Parse(dataBubble[i]["ActionNumber"].ToString());
                    if(fromActionNum == actionNumber && isCheck ==false)
                    {
                        isCheck = true;
                        actionNumStartPnt = i;
                    }
                    if(isCheck == true && fromActionNum != actionNumber )
                    {
                        actionNumEndPnt = i;
                        isCheck = false;
                        break;
                    }
                    if(isCheck == true && i + 1 == dataBubble.Count && fromActionNum == actionNumber)
                    {
                        actionNumEndPnt = i;
                        isCheck = false;
                        break;
                    }
                }
                //Debug.Log("actionNumber :: " + actionNumber);

                //Debug.Log("dataBubble.Count :: " + dataBubble.Count);
                //Debug.Log("actionNumStartPnt :: " + actionNumStartPnt);
                //Debug.Log("actionNumEndPnt :: " + actionNumEndPnt);
                // 롤과 버블 버블오디오 모두의 배열 크기를 똑같이 지정한다.
                int aNumAmount = actionNumEndPnt- actionNumStartPnt;
                //Debug.LogError("=================aNumAmount is "+ aNumAmount);
                if (aNumAmount < 0) aNumAmount = 0;
                roleName = new string[aNumAmount];
                textBubble = new string[aNumAmount];
                textBubbleAudio = new AudioClip[aNumAmount];

                // 크기가 지정되었으니 데이터를 불러서 각각의 배열안에 넣는다.

                for (int i = actionNumStartPnt; i < actionNumEndPnt; i++)
                {
                    // 배열의 시작은 0부터 시작되어야되니 ActionNumStartCnt 값을 i에서 뺀다.
                    int arrayCnt = i - actionNumStartPnt;

                    if (this.name == "001_Captain")
                    {
                        //Debug.Log("Captain ActionNum :: " + actionNumStartPnt);
                    }
                    // Role와 Text와 오디오 지정하여 준다.
                    // Null이 아닐 경우에만 불러준다.
                    Dictionary<string, object> dic = dataBubble[i];
                    if (dic.ContainsKey(bubbleName + "Role"))
                    {
                        roleName[arrayCnt] = dataBubble[i][bubbleName + "Role"].ToString();
                        textBubble[arrayCnt] = dataBubble[i][bubbleName + "Text"].ToString();
                        //Debug.Log("textBubble[" + arrayCnt + "]" + textBubble[arrayCnt]);
                        // 오디오는 단순 텍스트가 아니니 경로를 지정하여 호출 할 수 있도록 한다.
                        string audioPath = actionSoundPath + dataBubble[i][bubbleName + "Audio"].ToString();
                        if (audioPath != "" && audioPath != null)
                        {
                            textBubbleAudio[arrayCnt] = Resources.Load(audioPath) as AudioClip;
                        }
                    }
                }

            }
            // true가 아니면 데이터가 없다는 뜻이니 롤,텍스트버블,오디오 모두를 없음('0')으로 만든다.
            else
            {
                roleName = new string[0];
                textBubble = new string[0];
                textBubbleAudio = new AudioClip[0];
            }

            // 말풍선에 맞는 소리를 불러온다.
            // GetTextBubbleAudio 참고하여 작성.




            #region OLD 앞으로 안쓸거다.
            /*
            //텍스트버블1 과 롤네임1을 먼저 처리한다//

            string[] textBubbleRoleName = textData.Split('|');

            //데이터가 없으면 오류를 잡아주는 코드를 삽입한다//
            if (textBubbleRoleName.Length == 1 && textBubbleRoleName[0] == "")
            {
                roleName = new string[0];
                textBubble = new string[0];
            }
            else
            {
                Debug.Log("캐릭터 이름: "+ transform.name);
                Debug.LogError("textBubbleRoleName Length: "+ textBubbleRoleName.Length);
                roleName = new string[textBubbleRoleName.Length / 2];
                textBubble = new string[textBubbleRoleName.Length / 2];

                for (int j = 0; j < textBubbleRoleName.Length; j++)
                {
                    //Debug.Log("Base Data - Index: " + j + " :" + textBubbleRoleName[j] + "i % 2 = " + (j % 2));
                    if (j % 2 == 0)
                    {
                        Debug.Log("Role Nmame - Index: " + j + " :" + textBubbleRoleName[j]);
                        roleName[j / 2] = textBubbleRoleName[j];
                    }
                    else
                    {
                        Debug.Log("Text Bubble - Index: " + j + " :" + textBubbleRoleName[j]);
                        textBubble[j / 2] = textBubbleRoleName[j];
                    }
                }
            }
            */
            #endregion

        }


        /// <summary>
        /// 텍스트 버블 오디오 세팅//
        /// </summary>
        /// <param name="tbAudio"></param>
        /// <param name="textBubbleAudio"></param>
        void GetTextBubbleAudio(string tbAudio, out AudioClip[] textBubbleAudio)
        {
            string[] tBubble1Audio = tbAudio.Split('|');

            if (tBubble1Audio.Length == 1 && tBubble1Audio[0] == "")
            {
                textBubbleAudio = new AudioClip[0];
            }
            else
            {
                textBubbleAudio = new AudioClip[tBubble1Audio.Length];

                for (int j = 0; j < tBubble1Audio.Length; j++)
                {
                    textBubbleAudio[j] = Resources.Load(actionSoundPath + tBubble1Audio[j]) as AudioClip;
                }
            }
        }

        /// <summary>
        /// 인액션 세팅//
        /// </summary>
        /// <param name="textData"></param>
        /// <param name="inAction"></param>
        /// 
        /// 여기서 텍스트 데이터는 인액션줄에 있는 텍스트 내용이다.
        /// 이 내용들을 앞부분은 캐릭터 고유 번호 그리고 연결되는 액션 넘버를 기준으로 작성이 되어 있다.
        /// 수정해야 될것은 액션넘버로 되어있는 것을 이넘으로 바꾸어서 작동되게 하는 것이다.
        /// 기 생성 해놓은 InputAction 클래스에 새롭게 이넘변수인
        /// inputActionEnum을 선언해놨다.
        /// 문서에도 수정하여 들어가겠지만.
        /// 액션넘버말고도 액션넘버 자체에 고유 아이덴터티를 주기 위해 이넘으로 선언되는 이름을 찾아가서
        /// 액션을 연결해주는 작업을 할 예정이다.
        void GetInAction(string textData, out InputAction[] inAction)
        {
            //Debug.Log("GetInAction Running of "+ this.name);
            string[] textDataRoleNum = textData.Split('|');

            if (textDataRoleNum.Length == 1 && textDataRoleNum[0] == "")
            {
                inAction = new InputAction[0];
                //Debug.Log("인액션 없음");
            }
            else
            {
                //Debug.Log("인액션 있음");
                //Debug.Log("Length: "+ textDataRoleNum.Length / 2);
                inAction = new InputAction[textDataRoleNum.Length / 2];

                for (int j = 0; j < textDataRoleNum.Length; j++)
                {
                    // j가 0을 포함한 짝수일 경우//
                    // 이름(번호)로 지정한 캐릭터를 찾기 위해 아래 코드를 실행한다//
                    string containString = textDataRoleNum[j];
                    // 이래 내용에서 tP는 캐릭터 묶음인 상위 폴더 부분을 뜻한다.
                    // 실제 그부분으로 초기화한다. 아래 변수 선언에서 transform은 현재.
                    // 이 스크립트가 붙어있는 오브젝트, 즉 캐릭터 자체를 의미하며.
                    // 이 것의 상위라는 것(parent)는 캐릭터를 담고 있는 characters라는 상위 폴더가 된다.
                    Transform tP = transform.parent;
                    // thatChar은 tP 아래의 차일드 오브젝트인 임의이 하나를 가리키게 된다.
                    // tP 아래의 모든 오브젝트는 캐릭터들이니 자연스럽게 캐릭터가 된다.
                    Transform thatChar = transform;
                    for (int childNum = 0; childNum < tP.childCount; childNum++)
                    {
                        Transform gChild = tP.GetChild(childNum);
                        if (gChild.name.Contains(containString))
                        {
                            inAction[j / 2] = new InputAction();
                            thatChar = tP.GetChild(childNum);
                            //Debug.Log("인액션 제이 : "+j+" : 캐릭터 이름: " + thatChar.name);
                            inAction[j / 2].otherSA = new MoveOnPathScript();
                            inAction[j / 2].otherSA = thatChar.GetComponent<MoveOnPathScript>();

                            //if (this.name.Contains("Captain"))
                            //{
                            //    Debug.Log("Captain In Action");
                            //    Debug.Log(thatChar.name);
                            //}
                            break;
                        }
                    }
                    j++;
                    // 구버전형태:
                    // 액션 넘버로 연결된 부분을 입력하여 놓는다.
                    //inAction[j / 2].inputActionNum = int.Parse(textDataRoleNum[j].ToString());

                    // 신버전형태:
                    // 이넘형태로 연결된 액션을 입력하여 놓는다.
                    // 아래 파싱하는 형태를 잘 기억해야함.


                    inAction[j / 2].inputActionEnum = (ActionEnum)Enum.Parse(typeof(ActionEnum),textDataRoleNum[j].ToString());

                    if (this.name.Contains("Captain"))
                    {
                        //Debug.Log("Action :: " + inAction[j / 2].inputActionEnum);
                    }
                }
            }
        }
    }
}
