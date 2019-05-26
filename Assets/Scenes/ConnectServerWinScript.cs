using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectServerWinScript : MonoBehaviour {
    public Text baseText;
    public Text endText;
    public ClientLobbyServerConnect clsConnect;
    public GameObject canvas_MainUI;

    public void Plus()
    {
        int t = int.Parse(endText.text)+1;
        endText.text = t.ToString();
    }

    public void Minus()
    {
        int t = int.Parse(endText.text) -1;
        endText.text = t.ToString();
    }

    public void ConnectServer()
    {
       // clsConnect.serverURL = "http://" + baseText.text + endText.text;
        clsConnect.UserStart();

        /////////////////////////////////
        //canvas_MainUI.SetActive(false);
        //canvas_MainUI.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        /////////////////////////////////

        this.gameObject.SetActive(false);
    }
}