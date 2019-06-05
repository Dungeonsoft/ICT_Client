using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickProp : MonoBehaviour {
    Color defaultColor;

    public string[] ConnectedRoll;


    //private void Start()
    //{
    //    GetActCharacter().GetComponent<ICT_Engine.MoveOnPathScript>().TriggerEnterObj = null;

    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Canvas_ConLine"))
        {
            //Debug.Log("+++++++++++++++++++++++This is On Trigger!!!+++++++++++++++++++++++");
            //Debug.Log("Name: " + this.name);

            GetActCharacter().GetComponent<ICT_Engine.MoveOnPathScript>().TriggerEnterObj = this.gameObject;
            //Debug.Log("여기옴1");
            //Debug.Log("여기옴2");
        }
        else
        {
            //Debug.Log("컨트롤러와 접촉 상태가 아님");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("+++++++++++++++++++++++This is Exit Trigger!!!+++++++++++++++++++++++");
        //Debug.Log("Name: " + this.name);

        GetActCharacter().GetComponent<ICT_Engine.MoveOnPathScript>().TriggerEnterObj = null;
    }

    Transform GetActCharacter()
    {
        Transform characters = GameObject.Find("Characters").transform;

        for (int i = 0; i < characters.childCount; i++)
        {
            if (characters.GetChild(i).name.Contains(ConstDataScript.roleNum.ToString()))
            {
                //Debug.Log("Action Character!!");

                return characters.GetChild(i);
            }
        }
        return null;
    }

    //private void AfterAnimation()
    //{
    //}




}
