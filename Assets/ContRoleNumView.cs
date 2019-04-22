using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContRoleNumView : MonoBehaviour {

    public int ActRoleNum;

	void Start () {
        ActRoleNum = ConstDataScript.roleNum;
    }
	
}
