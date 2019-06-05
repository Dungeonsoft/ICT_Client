using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using UnityEngine.Events;

public class ControllerKeyInputTest : MonoBehaviour {

    void Start()
    {
        InteractionManager.InteractionSourceDetected += SourceDetected;
        InteractionManager.InteractionSourceUpdated += SourceUpdated;
        InteractionManager.InteractionSourceLost += SourceLost;
        InteractionManager.InteractionSourcePressed += SourcePressed;
        InteractionManager.InteractionSourceReleased += SourceReleased;
    }
    private void OnDestroy()
    {
        InteractionManager.InteractionSourceDetected -= SourceDetected;
        InteractionManager.InteractionSourceUpdated -= SourceUpdated;
        InteractionManager.InteractionSourceLost -= SourceLost;
        InteractionManager.InteractionSourcePressed -= SourcePressed;
        InteractionManager.InteractionSourceReleased -= SourceReleased;

    }

    void SourceDetected(InteractionSourceDetectedEventArgs obj)
    {
        Debug.Log("1 ::: "+ obj);
        // Detect
    }

    void SourceUpdated(InteractionSourceUpdatedEventArgs obj)
    {
        //Debug.Log("2 ::: " + state);
        // Update
    }

    void SourceLost(InteractionSourceLostEventArgs obj)
    {
        Debug.Log("3 ::: " + obj.state);
        // Lost
    }

    void SourcePressed(InteractionSourcePressedEventArgs obj)
    {
        if (obj.state.touchpadPosition.y != 0f)
        {
            //Debug.Log("4 touchpadPosition::: " + obj.state.touchpadPosition);
            //if (obj.state.touchpadPosition.y > 0)
            //    touchPadInput = 1f;
            //else
            //    touchPadInput = -1f;
            touchPadInput = obj.state.touchpadPosition.y;
        }

        if(obj.state.touchpadTouched) isTouchPad = true;


        if (obj.state.grasped == true)
        {
            Debug.Log("4 grasped::: " + obj.state.grasped);
        }
        if (obj.state.selectPressed == true)
        {
            Debug.Log("4 selectPressed(Trigger)::: " + obj.state.selectPressed);
        }
        if (obj.state.selectPressedAmount != 0f)
        {
            //Debug.Log("4 selectPressedAmount(Trigger)::: " + obj.state.selectPressedAmount);
            OnSelect();
        }

        // Press
    }

    public float touchPadInput = 0;
    public bool isTouchPad = false;


    void SourceReleased(InteractionSourceReleasedEventArgs obj)
    {
        //Debug.Log("5");
        if (obj.state.touchpadTouched == false)
        {
            //Debug.Log("isTouchPad false");

            touchPadInput = 0f;
            isTouchPad = false;
        }
        if (obj.state.grasped == false)
        {
            //Debug.Log("5 grasped::: " + obj.state.grasped);
        }
        if (obj.state.selectPressed == false)
        {
            isTouchPad = false;

            //Debug.Log("5 selectPressed(Trigger)::: " + obj.state.selectPressed);
        }
        // Release
    }

    bool isOnSelect = false;
    Transform pc;
    // 트리거 클릭시 선택된 오브젝트가 있는지 점검//
    void OnSelect()
    {

        if (GameObject.Find("TextBubble01") || GameObject.Find("TextBubble02") || GameObject.Find("QuestionPanel"))
        {
            //Debug.Log("UI 인터페이스가 활성화 되어있습니다.");
            return;
        }

        if (uAction == null)
        {
            Debug.Log("어떤 함수도 준비되지 않았습니다.");
            //Debug.Log("시나리오 넘버:"+ConstDataScript.scenarioNum +" ::멀티상태: "+ ConstDataScript.isMulti + " ::롤넘버: "+ ConstDataScript.roleNum + " ::액션넘버: "+ GameObject.Find("Characters/010_CrewA").GetComponent<ICT_Engine.MoveOnPathScript>().actionNum );


            //수동문열기를 하면 리턴을 하는 것이 아니라 아래 주석처리한 부분을 살려주어야 한다.
            //return;

            // 문을 여는 조건은 여기서 정해주어야 한다.//
            // 다른 시나리오 다른 캐릭터가 되면 또 그에 맞는 조건을 만들어주면 된다.//

            // 시나리오 1번(화재) 조건

            #region 수동 문열기 기능
            
            if (
                (ConstDataScript.scenarioNum == 1 && 
                ConstDataScript.isMulti == true && 
                ConstDataScript.roleNum == 10 && 
                GameObject.Find("Characters/010_CrewA").GetComponent<ICT_Engine.MoveOnPathScript>().actionNum == 10)
                ||
                // 시나리오 4번(거주구역화재)조건
                (ConstDataScript.scenarioNum == 4 && 
                ConstDataScript.isMulti == true && 
                (ConstDataScript.roleNum == 10 || ConstDataScript.roleNum == 13) && 
                GameObject.Find("Characters/010_CrewA").GetComponent<ICT_Engine.MoveOnPathScript>().actionNum == 10)
                )
            {
                Debug.Log("문을 열 수 있습니다.");
                //GameObject prop = pc.GetComponent<ICT_Engine.MoveOnPathScript>().TriggerEnterObj;
                //Debug.Log("선택된 프랍이 있습니다. ::: " + prop.name);

                //prop.GetComponent<Animation>().Play("DoorOpen");
                //return;
            }
            else
            {
                return;
            }
            
            #endregion
        }
        if (isOnSelect == true) return;
        isOnSelect = true;
        Transform characters = GameObject.Find("Characters").transform;
        for (int i = 0; i < characters.childCount; i++)
        {

            // 플레이어 캐릭터를 찾는다.
            if (characters.GetChild(i).name.Contains(ConstDataScript.roleNum.ToString()))
            {
                Debug.Log("Player Character!!");
                pc = characters.GetChild(i);
                // 플레이어 캐릭터는 찾았고...
                // PC에 붙어있는 MoveOnPathScript의 TriggerEnterObj에 오브젝트가 들어가 있는지 확인한다.
                // (오브젝트가 들어갔다는 것은 현재 컨트롤러가 오브젝트(프랍)를 가리키고 있다는 것이다).
                GameObject prop = pc.GetComponent<ICT_Engine.MoveOnPathScript>().TriggerEnterObj;
                Debug.Log("선택된 프랍이 있습니다. ::: " + prop.name);
                if(prop != null)
                {

                    // Null이 아니라면 현재 캐릭터와 프랍과 관련있는 것인지 알아본다.
                    // 프랍이 어떤 캐릭터와 관련이 있는지는 아래 배열변수의 내용을 보면 알 수 있다.
                    string[] rollNum = prop.GetComponent<ClickProp>().ConnectedRoll;

                    for(int j = 0; j<rollNum.Length; i++)
                    {
                        if (pc.name.Contains(rollNum[j]))
                        {
                            AnimationClip cp;
                            // 연결된 플레이어 넘버를 보여준다.
                            Debug.Log("J number: "+ j+ " :: 연결된 캐릭터: " + rollNum[j]);


                            #region 수동문열기(문인지 확인하는 부분).
                            
                            // 문을 여는 경우.
                            if (prop.tag.ToLower().Contains("door"))
                            {
                                //prop.GetComponent<Animation>().Play("DoorOpen");
                                //Debug.Log("애니메이션 이름: " + prop.GetComponent<Animation>().clip.name);

                                cp = prop.GetComponent<Animation>().GetClip("DoorOpen");
                                prop.GetComponent<Animation>().Play(cp.name);
                            }
                            // 다른 경우.(경우의 수가 많아지면 나중에 스위치를 이용하자.)
                            else
                            
                            #endregion
                            {
                                prop.GetComponent<Animation>().Play();
                                cp = prop.GetComponent<Animation>().clip;
                            }
                            Debug.Log("델리게이트 부를 준비1");
                            
                            float clpLength = 2f;
                            Debug.Log("ClpLength:: " + cp.length);
                            clpLength = cp.length;

                            // 여기서 이벤트를 삽입하여야 한다.
                            // 여기서 이벤트는 플레이어 캐릭터에 전달하거나 또는 그곳에서
                            // 바로 실행이 되어야 될 코드(함수)를 지칭한다.
                            Debug.Log("델리게이트 부를 준비2");
                            StartCoroutine(AfterMethod(clpLength, prop));
                            // 4 touchpadPosition::: (0.1, -1.0) - 터치패드 눌렀을 때.
                            // 4 grasped::: True - 옆구립 그립 눌렀을 때.
                            // 4 selectPressedAmount(Trigger)::: 0.1764706 - 앞쪽의 트리거 눌렀을 때.

                            break;

                        }
                    }
                }
            }
        }
    }
    
    UnityAction uAction;

    // isWaitAddFunction을 락 걸어준 것(true)을 풀 수 있게(false) 해주는 함수.
    // 이 함수를 어디서 부르는지 찾아보자. 위치를 찾으면 기록할 것
    IEnumerator AfterMethod(float waitTime, GameObject prop)
    {
        yield return new WaitForSeconds(waitTime);
        //Debug.Log("!!!!델리게이트 액션 실행!!!!!!");
        //prop.SetActive(false);
        uAction();
        yield return null;
        uAction = null;
        isOnSelect = false;
        yield return new WaitForSeconds(1f);
        prop.SetActive(false);
        pc.GetComponent<ICT_Engine.MoveOnPathScript>().isWaitAddFunction = false;
        pc = null;
    }

    public void AddNewAction(UnityAction action = null)
    {
        uAction = action;
    }


}
