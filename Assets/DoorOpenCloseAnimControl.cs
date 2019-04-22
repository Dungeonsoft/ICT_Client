using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ActChars {
    public Transform[] chars;
}

public delegate void delegateUAction(Transform[] T);


public class DoorOpenCloseAnimControl : MonoBehaviour
{

    public float OpenVal = 30;
    public float closeVal = 180;
    public float regalDistance = 1;

    public Transform[] characters;

    Action updateAction;
    delegateUAction UAction;
    Transform thisTrans;
    Transform ch;

    public Transform[] actChar1;
    public Transform[] actChar2;

    // 점심 전까지 확인하고 수정한 부분.
    public ActChars[] actChars;

    public AudioClip openSound;



    int actCharCnt = 0;
    private void Awake()
    {
        thisTrans = this.transform;
        if (actChars.Length > 0)
        {
            updateAction = Act;
        }
    }

    public void DoorOpen()
    {
        #region 예전방식.
        //float rotZ = thisTrans.eulerAngles.z;
        //if (rotZ < 0)
        //{
        //    rotZ += 360;
        //}

        //if (addRotZ > 150f)
        //{
        //    addRotZ = 0;
        //    thisTrans.eulerAngles = new Vector3(-90f, 0f, 30f);
        //    updateAction -= DoorOpen;
        //}
        //else
        //{
        //    addRotZ += Time.deltaTime * 150f;
        //    thisTrans.eulerAngles -= new Vector3(0f, 0f, Time.deltaTime * 150f);
        //}
        #endregion

        AnimationClip cp = GetComponent<Animation>().GetClip("DoorOpen");
        Debug.Log("DoorOpen");
        GetComponent<Animation>().Play(cp.name);
        updateAction -= DoorOpen;
    }

    float addRotZ =0;
    public void DoorClose()
    {
        #region 예전 방식.
        //float rotZ = thisTrans.eulerAngles.z;
        //if (rotZ < 0)
        //{
        //    rotZ += 360;
        //}

        //if (addRotZ > 150f)
        //{
        //    addRotZ = 0;
        //    thisTrans.eulerAngles = new Vector3(-90f,0f,180f);

        //    GetComponent<AudioSource>().Play();

        //    updateAction -= DoorClose;
        //}
        //else
        //{
        //    addRotZ += Time.deltaTime * 150f;
        //    thisTrans.eulerAngles += new Vector3(0f, 0f, Time.deltaTime * 150f);
        //}
        #endregion 

        AnimationClip cp = GetComponent<Animation>().GetClip("DoorClose");
        GetComponent<Animation>().Play(cp.name);
        updateAction -= DoorClose;
    }

    List<Transform> detectedChList = new List<Transform>();
    Transform GetCloseChar()
    {
        int cnt = characters.Length;
        ch = null;
        for (int i = 0; i < cnt; i++)
        {
            Transform chCheck = characters[i];
            float dist = Vector3.Distance(thisTrans.position, chCheck.position);

            if (dist < regalDistance)
            {
                if (detectedChList != null)
                {
                    for (int j = 0; j < detectedChList.Count; j++)
                    {
                        if (detectedChList[j] == chCheck)
                        {
                            return ch;
                        }
                    }
                }

                detectedChList.Add(characters[i]);

                //Debug.Log("char name : " + characters[i].name + " ::: dist : " + dist);
                ch = characters[i];
                break;
            }
        }
        return ch;
    }

    private void Update()
    {
        if(updateAction != null)
        {
            updateAction();
        }
    }


    int detectedCh =0;

    void Act()
    {

        Transform[] chArray = actChars[actCharCnt].chars;
        Transform ch = GetCloseChar();
        for (int i = 0; i < chArray.Length; i++)
        {
            if (ch == chArray[i])
            {
                detectedCh++;

                //if (ch.GetComponent<ICT_Engine.MoveOnPathScript>().isActionCharacter == false)
                //{
                    //Debug.Log("Add Char1 ::: "+detectedCh +" ::: name :: "+actChar1[i].name);

                    if (detectedCh == 1)
                    {
                        GetComponent<AudioSource>().Play();

                        updateAction += DoorOpen;
                    }
                //}
            }
        }
        if (detectedCh == actChar1.Length)
        {
            detectedCh = 0;
            StartCoroutine(DelayTimeClose1());

            detectedChList.Clear();

            updateAction -= Act;
        }
    }

    #region 예전 방식.
    /*
    void Act001()
    {
        Transform ch = GetCloseChar();
        for (int i = 0; i < actChar1.Length; i++)
        {
            if (ch == actChar1[i])
            {
                detectedCh++;

                //Debug.Log("Add Char1 ::: "+detectedCh +" ::: name :: "+actChar1[i].name);

                if (detectedCh == 1)
                {
                    GetComponent<AudioSource>().Play();

                    updateAction += DoorOpen;
                }
            }
        } 

        if(detectedCh == actChar1.Length)
        {

            detectedCh = 0;
            StartCoroutine(DelayTimeClose1(Act002));
            detectedChList.Clear();
            updateAction -= Act001;

        }
    }

    void Act002()
    {
        Transform ch = GetCloseChar();
        for (int i = 0; i < actChar2.Length; i++)
        {
            if (ch == actChar2[i])
            {
                detectedCh++;

                Debug.Log("Add Char2 ::: " + detectedCh + " ::: name :: " + actChar2[i].name);

                if (detectedCh == 1)
                {
                    GetComponent<AudioSource>().Play();

                    updateAction += DoorOpen;
                }
            }
        }

        if (detectedCh == actChar2.Length)
        {
            detectedCh = 0;
            StartCoroutine(DelayTimeClose1(Act002));
            updateAction -= Act002;
        }
    }
    */
    #endregion

    IEnumerator DelayTimeClose1()
    {
        yield return new WaitForSeconds(2f);
        updateAction += DoorClose;


        #region 예전방식
        // 두번째 턴부터는 캐릭터가 싱글이거나 멀티라도 문에 접근하는 캐릭터가 액션캐릭터가 아닐경우는 자동으로열고 닫히게 한다.

        //Transform[] chars = actChars[val].chars;
        //bool isActch = false;
        //foreach (var c in chars)
        //{
        //    if(c.GetComponent<ICT_Engine.MoveOnPathScript>().isActionCharacter == true)
        //    {
        //        isActch = true; break;
        //    }
        //}

        //if (ConstDataScript.isMulti ==  false || isActch == false)
        //{
        //    yield return new WaitForSeconds(10f);
        //    UAction(actChars[val].chars);
        //}
        #endregion

        actCharCnt++;
        if (actCharCnt < actChars.Length)
        {
            yield return new WaitForSeconds(10f);
            updateAction += Act;
        }
        else
        {
            actCharCnt = 0;
        }
    }

    void Open()
    {
        updateAction += DoorOpen;
    }

}
