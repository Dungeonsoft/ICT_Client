using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenClose01 : MonoBehaviour
{
    public AudioClip doorOpen;
    AudioSource aSource;
    bool isAnim =false;
    private void Start()
    {
        aSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("DoorOpen");
            GetComponent<Animation>().Play("EngineDoorOpen");
            aSource.clip = doorOpen;
            aSource.loop = false;
            aSource.Play();
    }
}
