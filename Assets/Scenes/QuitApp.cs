using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitApp : MonoBehaviour {

public void QuitWin()
    {
        Debug.Log("Application Quit!");
        Debug.Log("Application Quit!");
        Debug.Log("Application Quit!");
        Debug.Log("Application Quit!");
        Debug.Log("Application Quit!");
        Debug.Log("Application Quit!");
        Debug.Log("Application Quit!");
        Debug.Log("Application Quit!");
        Debug.Log("Application Quit!");

        StartCoroutine(QuitIE());
    }

    IEnumerator QuitIE()
    {
        yield return new WaitForSeconds(3f);
        Application.Quit();
    }
}