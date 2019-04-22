using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using socket.io;
using System;

public class ClientLobbyServerConnect : MonoBehaviour
{

    public static ClientLobbyServerConnect instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }


    Socket socket;

    public string idGotten;
    public string dateFromServer;

    public string serverURL = "http://localhost";

    public string urlInfoPath = @"c:\InnoIctUrl\url.txt";

    public GameObject show_Url_win;
    public GameObject logIning_Win;
    public GameObject logIn_Success_Win;
    public GameObject userRole_State_Win;
    public GameObject logIn_Fail_Win;

    List<GameObject> winList = new List<GameObject>();

    public Text LogText;
    private List<string> loglist = new List<string>();


    public void UserStart()
    {

        winList.Add(logIning_Win);
        winList.Add(logIn_Success_Win);
        winList.Add(userRole_State_Win);
        winList.Add(logIn_Fail_Win);

        // 처음 로그인중 화면을 보여줌.
        ShowWin(0f, logIning_Win);

        #region 외부에서 주소를 불러오는 코드를 작성한것-안됨
        //로컬 위치에서 서버주소를 불러오기//
        //string urlText;
        //try { 
        //   urlText = System.IO.File.ReadAllText(urlInfoPath);
        //}
        //catch ( Exception e)
        //{
        //    Debug.LogException(e);
        //    urlText = "http://192.168.0.3";
        //}

        //if (string.IsNullOrEmpty(urlText))
        //{
        //    Debug.Log("urlText is nullor empty");
        //}
        //else
        //{
        //    serverURL = urlText;
        //    Debug.Log(urlText);
        //    logIning_Win.transform.Find("Text").GetComponent<Text>().text = serverURL;
        //}
        #endregion



        logIning_Win.transform.Find("Text").GetComponent<Text>().text = serverURL;


        Debug.Log("@@@@@@@@ Server Connecting Start!!!");
        if (socket == null)
        {
            Debug.Log("@@@@@@@@ Socket Is NULL!!!");
            // 소켓은 주어진 주소를 기반으로 연결한다.(접속시도)
            //socket = Socket.Connect(serverURL);
            try
            {
                socket = Socket.Connect(serverURL);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            if (socket == null)
            {
                Debug.Log("SOCKET IS NULL");
            }
            else
            {
                Debug.Log("SOCKET IS NOT NULL");

               
            }

            // 연결되면 이곳이 실행이 된다.//
            socket.On("connect", () =>
            {
                Debug.Log("111111 -커넥트 되었다.-");
                Debug.Log("-처리할 내용을 이곳에 구현한다-");
                Debug.Log("Connected");
            });


            //로그인을 시도하며 통제툴이 먼저 로그인 되었는지 확인을 한다.//
            //통제툴이 아닌 일반 클라이언트에서 작동할 코드.//
            socket.On("GetIdMade", (string idAndDate) =>
            {
                // 받아온 아이디와 날짜를 데이터를(json Type data) 유니티에서 쓸수 있게//
                // 컨버팅을 한다. 컨버팅을 할때는 이를 받아쓸수 있게 되어있는 기 선언된 클래스//
                // 껍데기가 하나 필요하다. 앞으로 이런 형태가 발생하면 미리 준비할것.//
                Debug.Log("GetIdMade >> : " + idAndDate);

                // 뉴튼제이슨을 쓰는 부분- UWP에서는 사용불가능.
                //IdDate idDate = JsonConvert.DeserializeObject<IdDate>(idAndDate);
                // 제이슨을 유니티 기본 제공되는 것으로 사용.
                IdDate idDate = JsonUtility.FromJson<IdDate>(idAndDate);

                idGotten = idDate.id;
                dateFromServer = idDate.date;
                Debug.Log("Id Gotten: " + idGotten);
                ConstDataScript.uid = idGotten;
                // 통제툴이 로그인했는가 여부를 가지고 와서 확인을 해준다.//
                socket.Emit("loginClient", idGotten, (string isSuccessFromServer) =>
                {
                    Debug.Log("loginClient  실행이 되었다");
                    isSuccessFromServer = CleanupString(isSuccessFromServer);
                    // 통제툴이 준비되었는지 먼저 확인후 작동하도록 만든다.//
                    // 통제툴이 준비되지 않았으면 잠시후 다시 로그인을 하라는 창이 뜨고//
                    // 대기 상태가 되도록 만들어 준다.//
                    if (isSuccessFromServer == "true")
                    {
                        Debug.Log("Prefaring is " + isSuccessFromServer);
                        // 대기 화면을 보여준다.//
                        // 여기에 표시되어야 될 것은//
                        // 기본적인 유저 이름(그리스전시회에서는 직책과 이름(trainee로 표기))
                        ShowWin(0f, logIn_Success_Win);
                        ShowWin(2f, userRole_State_Win);
                    }
                    else
                    {
                        Debug.Log("Prefaring is " + isSuccessFromServer);
                        // 통제툴이 준비되지 않았기 때문에//
                        // 통제툴이 준비되면 다시 로그인하라는//
                        // 화면이 뜨고 아래 재로그인 버튼을 붙인다.//
                        ShowWin(0f, logIn_Fail_Win);
                        // 실패 화면이 뜨면 종료가 처리되도록 메소드를 실행한다.
                        logIn_Fail_Win.GetComponent<QuitApp>().QuitWin();
                    }
                });
            });

            socket.On("startClient", (string data) =>
            {
                Debug.Log("Start Client::: "+data);
                Debug.Log(data + "!!!!!!!");
                Debug.Log(data + "!!!!!!!");
                Debug.Log(data + "!!!!!!!");
                Debug.Log(data + "!!!!!!!");
                Debug.Log(data + "!!!!!!!");

                var roleData = CleanupString(data).Split(',');
                Debug.Log("roleData.Length: "+ roleData.Length);
                for (int i = 0; i < roleData.Length; i++)
                {
                    Debug.Log("roleData[" + i + "]" + roleData[i]);

                    if (roleData[i] != "" && roleData[i] != null)
                    {
                        ConstDataScript.userCharacters.Add(int.Parse(roleData[i]));
                    }
                }
                Debug.Log("스트링에서 인트로 변화된 값");
                foreach(var a in ConstDataScript.userCharacters) 
                {
                    Debug.Log(a);
                }

                var sNum = ConstDataScript.scenarioNum;

                StartCoroutine(StartNextScene(sNum));
                //UnityEngine.SceneManagement.SceneManager.LoadScene(sNum);
            });

            // sNum 참고.
            // 1번: 화재.
            // 2번: 퇴선.
            // 3번: 밀폐구역.
        }

        socket.On("changeScenario",(string sNum) => {
            sNum = sNum.Trim('"'); 
            Debug.Log("실행될 시나리오가 변경된다. 변경 시나리오 :: " + sNum);
            Debug.Log("sNum Type: " + sNum.GetType());
            ConstDataScript.scenarioNum = int.Parse(sNum);
        });

        // 통제툴에서 클라이언트의 직책을 바꾸면 여기까지 전달이 되서.
        // 클라이언트에서 보여지는 직책을 바꾸어 보여준다.
        socket.On("changeRole", (string roleNum) =>
        {
            Debug.Log(roleNum.GetType());
            Debug.Log("롤 정보가 변경되는가?" + roleNum);
            int rn = int.Parse(CleanupString(roleNum));
            Debug.Log("rn ::: " + rn);
            string rName = SetRoleToString(rn);
            userRole_State_Win.transform.Find("Text").GetComponent<Text>().text = rName;
            //롤이 변경 된 것을 컨스트데이타에 입력한다.
            ConstDataScript.roleNum = rn;
        });

        socket.On("getOtherPoz", (string userData) =>
        {
            // 제이슨 형식의 스트링 파일을 서버로 부터 받아서.
            // 클래스 모양으로 변경하여 준다.

            // 유니티 제이슨으로 변경하여 데이터 변환.
            GetOtherUserMoveData oUserData
            = JsonUtility.FromJson<GetOtherUserMoveData>(userData);
            
            //if (oUserData.uid == idGotten)
            //{
            //    //Debug.Log("자신의 데이터 입니다.");
            //    return;
            //}

            // 먼저 최상의 캐릭터들 묶음을 찾고.
            GameObject charac = GameObject.Find("Characters");
            // 실제 캐릭터를 찾는다.
            GameObject oUserChar = charac.transform.GetChild(int.Parse(oUserData.roleNum)-1).gameObject;
            // 캐릭터에 붙어있는 전체를 컨트롤 하는 스크립트를 찾고.
            ICT_Engine.MoveOnPathScript pScript = oUserChar.GetComponent<ICT_Engine.MoveOnPathScript>();
            // 액션 넘버를 맞춰준다.
            pScript.actionNum = int.Parse(oUserData.actionNum);

            // 액션넘버가 바뀐기록이 있으면 액션내용을.
            // 바꿔주고 인액션 전달을 해주는 함수를 실행하도록 해준다.
            if (oUserData.isChangeActionNum =="true")
                pScript.ChangeActionNumSendInAction();

            //if (oUserChar.transform.position.x != float.Parse(oUserData.pozX))
            //{
                // 위치를 맞추고.
                oUserChar.transform.position = new Vector3(float.Parse(oUserData.pozX),
                                                 float.Parse(oUserData.pozY),
                                                  float.Parse(oUserData.pozZ));
            //}
            //if(oUserChar.transform.eulerAngles.y != float.Parse(oUserData.rotY))
            //{
                //회전 값을 맞춘다.
                oUserChar.transform.eulerAngles = new Vector3(0f,float.Parse(oUserData.rotY),0f);
            //}

            Animator playerAnimator;
            playerAnimator = oUserChar.transform.GetChild(0).GetComponent<Animator>();
            //playerAnimator.Play("Walking_01");
            playerAnimator.Play(oUserData.chMotion);
        });

        socket.On("quitApp", (string data) =>
        {
            Debug.Log("Quit App!!!!!!!"+ data);
            ShowWin(0f, logIn_Fail_Win);
            logIn_Fail_Win.GetComponent<QuitApp>().QuitWin();
        });

        socket.On("stopClient", (string data) =>
        {
            Debug.Log("data");
            Debug.Log("stopClient를 실행한다.");
            DestroyImmediate(GameObject.Find("InputManager"));
            DestroyImmediate(GameObject.Find("(singleton) socket.io.SocketManager"));
            DestroyImmediate(GameObject.Find("MixedRealityCameraParent"));
            ConstDataScript.scenarioNum = 1;
            GameObject.Find("ChangeScene").GetComponent<ChangeSceneScript>().ChangeScene(this.gameObject, 0);
        });

        // 컨트롤툴을 지나 서버를 통해 들어온 날씨 정보를
        // 지정된 변수(ConstDataScript.weatherType)에 저장하는 역할을 한다.
        socket.On("getChangeWeather", (string userData) =>
        {
            GetWeatherData userWeatherdata
                = JsonUtility.FromJson<GetWeatherData>(userData);

            ConstDataScript.weatherType = userWeatherdata.weatherIs;

        });
    }

    public void UserPozRot(GetOtherUserMoveData udata)
    {
        //Debug.Log("내정보 서버로 보냄,위치, 회전,액션넘버,롤넘버: " + udata.roleNum + ", ---- uid");
        string data = JsonUtility.ToJson(udata);
        if (socket != null)
        {
            socket.EmitJson("userPozRot", @data);
        }
    }

    public void Alarmclear(SendEnd sEnd)
    {
        string data = JsonUtility.ToJson(sEnd);
        if(socket != null)
        {
            socket.EmitJson("alarmclear", @data);
        }
    }

    IEnumerator StartNextScene(int sNum)
    {
        Debug.Log("다음씬 준비: "+sNum);
        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sNum);
    }

    public void sendChPosRot(string posRot)
    {
        socket.EmitJson("sendPosRot", posRot);
    }

    // 데이터가 배열 형태로 들어오니 배열형을 제거하고 문자열 형태만 남겨놓는다.
    string CleanupString(string data)
    {
        string[] ridCha = new string[] { "[", "]", "\"", "\'" };

        for (int i = 0; i < ridCha.Length; i++)
            data = data.Replace(ridCha[i], "");

        return data;
    }

    void ShowWin(float t, GameObject win)
    {
        StartCoroutine(ShowWinIE(t, win));
    }

   IEnumerator ShowWinIE(float t, GameObject win)
    {
        yield return new WaitForSeconds(t);
        foreach (var winL in winList)
        {
            winL.SetActive(false);
        }
        win.SetActive(true);
    }


    string SetRoleToString(int r)
    {
        string rName = "";
        switch (r)
        {
            case 1:
                rName = "Catptain";
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

public class GetWeatherData
{
    public string weatherIs;
}
