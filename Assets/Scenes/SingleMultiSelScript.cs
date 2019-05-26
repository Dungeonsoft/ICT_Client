using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMultiSelScript : MonoBehaviour
{
    public GameObject singleModeWin;
    public GameObject multiModeWin;

    public ClientLobbyServerConnect clsConnect;

    public void SingleSel()
    {
        Debug.Log("싱글 모드 실행");
        ConstDataScript.isMulti = false;
        singleModeWin.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void MultiSel()
    {
        Debug.Log("멀티 모드 실행");

        //clsConnect.serverURL = "http://192.168.0.2"; // 여러대의 컴퓨터로 실행시
        ////clsConnect.serverURL = "http://localhost"; // 한대의 컴터에 서버와 클라가 같이 있을 때.
        clsConnect.UserStart();

        ConstDataScript.isMulti = true;
        this.gameObject.SetActive(false);
    }
}
