using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMeetChar : MonoBehaviour
{
    string thisName;
    private void Awake()
    {
        thisName = this.transform.parent.name.ToString().Split('_')[1].ToLower();
    }

    void OnTriggerEnter(Collider col)
    {
        ICT_Engine.MoveOnPathScript mscript =
        col.transform.GetComponent<ICT_Engine.MoveOnPathScript>();

        if (mscript.name == transform.parent.name)
        {
            Debug.Log("End Action1");
            mscript.EndAction();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        string hitName = hit.transform.name.ToString().Split('_')[1].ToLower();

        if (thisName != hitName) return;

        ICT_Engine.MoveOnPathScript mscript =
        hit.transform.GetComponent<ICT_Engine.MoveOnPathScript>();

        if (mscript.name == transform.parent.name)
        {
            Debug.Log("End Action2");
            mscript.EndAction();
        }
    }
}
