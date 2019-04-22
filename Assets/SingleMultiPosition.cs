using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMultiPosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(ConstDataScript.isMulti == true)
        {
            this.transform.position = new Vector3(-6.98f, 19.57f, 2.23f);
            this.transform.eulerAngles = new Vector3(0f,90f,2.023f);
        }
        else
        {
            this.transform.position = new Vector3(-5.86f, 19.356f, 7.59f);
            this.transform.eulerAngles = new Vector3(0f, 90f, 2.023f);
        }
    }
	
}
