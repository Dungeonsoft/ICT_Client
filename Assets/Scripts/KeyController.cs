using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyController : MonoBehaviour
{
    public KeyCode kCode;

    private void Update()
    {
        if(Input.GetKeyDown( kCode))
        {
            //GetComponent<Button>().onClick.l
        }
    }

}
