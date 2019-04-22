using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneScript : MonoBehaviour {


    public void ChangeScene(GameObject destObj,int num)
    {
        DestroyImmediate(destObj);
        // UnityEngine.SceneManagement.SceneManager.LoadScene(num);
        UnityEngine.SceneManagement.SceneManager.LoadScene("EmptyScene");
    }
}
