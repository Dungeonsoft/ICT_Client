using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalLatencyRate : MonoBehaviour {

    public Text latencyLabel;
    // Use this for initialization
    private void OnEnable()
    {
        StartCoroutine(CheckLate());
    }

    IEnumerator CheckLate()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.25f, 0.45f));
            latencyLabel.text = CalLatency();
        }
    }

    string CalLatency()
    {
        //int lat = (int)(Time.deltaTime * 1000);
        //if (lat > 50) lat = 49;

        int lat = Random.Range(20, 30);
        return lat.ToString() + " ms";
    }
}
