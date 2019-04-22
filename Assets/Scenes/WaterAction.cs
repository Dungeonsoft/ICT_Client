using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;
using HoloToolkit.Unity.InputModule;

public class WaterAction : MonoBehaviour
{

    bool isWater;
    public AddClass aClass;

    // 물 나옴과 회전을 테스트 하기 위해 넣은 임시 변수 
    public bool isForcedWater = false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown(XboxControllerMapping.XboxLeftBumper)
            ||
            Input.GetButtonDown(XboxControllerMapping.XboxRightBumper))
        {
            Debug.Log("=====!!!!!PRESS BUMPER!!!!!=====");

            if (ConstDataScript.roleNum == 10)
            {
                // 바로 아래의 코드는 물을 쓸수 있을때만 쓸수 있게 해주는 코드이다.
                if (GetComponent<MoveOnPathScript>().sa.isMeA == true || isForcedWater == true)
                {
                    if (isWater == false)
                    {
                        Debug.Log("isWater = true");
                        aClass.ClickIn();
                    }


                    if (isWater == true)
                    {
                        Debug.Log("isWater = false");
                        aClass.ClickOut();
                    }
                    //트리거를 눌렀을때 물건이 타게팅이 되어 있는가에 대한 정보를 가지고 와서//
                    // 타게팅이 되어있으면 그에 관련된 기능이 되게 해주면 된다.//
                    // 현재는 박스를 선택하면 옷을 갈아입는 것이니//
                    //VirAddClass와 관련이 있다.
                    // 실제 코드는 어디에 작성해야 될지 다시 생각해보자.

                    isWater = !isWater;
                }
            }
        }

        if ( isForcedWater == false  && ( isWater == true && GetComponent<MoveOnPathScript>().sa.isMeA == false))
        {
            Debug.Log("isWater = false");
            aClass.ClickOut();
            isWater = false;
        }
    }
}
