using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 이클래스를 기반으로 서버에서 기본 데이터를 받아온다.
public class GetUserBaseData
{
    public string uid;
    public string name;
    public string role;
    public string status;
    public string result;
    public string _id;
}

//최초 아이디와 날짜를 받아올 때 사용하기 위해 만들어 놓은 클래스.
public class IdDate
{
    public string id;
    public string date;
}

public class GetOtherUserMoveData
{
    public string pozX;
    public string pozY;
    public string pozZ;
    public string rotX;
    public string rotY;
    public string rotZ;
    public string actionNum;
    public string roleNum;
    public string uid;
    public string chMotion;
    public string isChangeActionNum;
    public string isWalk;
}

public class SendEnd
{
    //여기서는 액션 클리어 트루 폴스로 값을 넣어준다.
    public string roleNum;
    public string uid;
    public string actionClear;
}

