using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMainScene : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        Debug.Log("Load Empty Scene!!!!!!!!");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
