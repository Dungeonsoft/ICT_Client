using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstDataScript 
{
    //유저 고유의 아이디를 저장한다.//
    public static string uid = "";

    public static bool isMulti = false;

    public static int scenarioNum = 1; //1 = 화재, 2 = 퇴선, 3 = 밀폐구역, 4 = 거주구역//
    public static int modeNum = 2; //1 = 연습. 2= 평가// 
    public static int roleNum = 3; //아래 롤 정보 참고/

    public static int langNum = 1; //0 = 영어,  1 = 한국어//
    public static bool isFpsOn = true;
    public static bool isLatOn = true;

    public static float firstDelay = 10;

    public static float mSlow = 0.695f;
    public static float mNormal = 1.11f;
    public static float mHigh =1.67f;

    public static float bubbleWaitTime = 1.5f;

    public static List<int> userCharacters = new List<int>();

    // weather type 은 sunny,fog 두가지로 구분한다.
    public static string weatherType = "sunny";
    

    //웨더데이터를 서버에서 받아서 저장하는 부분까지 완료.
    //월요일에 와서 서버에서 받은 정보를 클라씬에서 처리가 되도록(안개가 보이도록) 만들것.

    //Role
    //0: 랜덤 셀렉트//
    //1: 선장//
    //2: 1항사//
    //3: 2항사//
    //4: 3항사//
    //5: 기관장//
    //6: 1기사//
    //7: 2기사//
    //8: 3기사//
    //9: 갑판장//
    //10: 갑판수A//
    //11: 갑판수B//
    //12: 갑판수C//
    //13: 조기장//
    //14: 조기수A//
    //15: 조기수B//
    //16: 조기수C//
    //17: 조리장//
}
