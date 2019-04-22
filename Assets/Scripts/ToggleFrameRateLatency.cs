using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFrameRateLatency : MonoBehaviour
{

    GameObject frl;

    private void Awake()
    {
        frl = transform.GetChild(0).gameObject;
        frl.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Q))
        {
            frl.SetActive(!frl.activeSelf);
        }
    }
}
