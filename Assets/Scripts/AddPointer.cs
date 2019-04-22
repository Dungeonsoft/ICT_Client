using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPointer : MonoBehaviour
{

    public bool isReCalPath;
    // Use this for initialization
    void Start()
    {

    }

    private void OnDrawGizmos()
    {
        if(isReCalPath ==  true)
        {
            GetComponent<iTweenPath>().nodeCount = transform.childCount;


            for(int i = 0; i< GetComponent<iTweenPath>().nodeCount; i++)
            {
                GetComponent<iTweenPath>().nodes[i] = transform.GetChild(i).position;
            }

            isReCalPath = false;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
