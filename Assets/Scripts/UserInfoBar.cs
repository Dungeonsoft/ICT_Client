using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UserInfoBar : MonoBehaviour {

    public GameObject userRole;
    public GameObject userName;
    public GameObject userStatus;
    public GameObject userResult;
    public GameObject userSelect;

    public float getHeight
    {
        get
        {
            float height = 0;
            height = userRole.GetComponent<RectTransform>().rect.height;
            return height;
        }
    }

	// Use this for initialization
	void Awake () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
