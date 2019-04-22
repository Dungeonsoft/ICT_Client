using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRoleNum : MonoBehaviour
{
    public int roleNum;

    public void SendRoleNumber()
    {
        GameObject.Find("AppControl").GetComponent<ICT_Engine.FAT_Control>().ClickRole(roleNum);
    }
}
