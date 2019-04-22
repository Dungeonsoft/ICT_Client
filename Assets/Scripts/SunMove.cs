using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMove : MonoBehaviour {

    Transform[] sunPosition;
    MakeCurveScript mcScript;
    float timeSpand;
    public float journeyTime = 5f;
    public Transform pa;

	// Use this for initialization
	void Awake () {
        sunPosition = pa.GetComponentsInChildren<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 center = (sunPosition[1].position + sunPosition[2].position) * 0.5f;
        center -= new Vector3(0, 1, 0);
        Vector3 riseCenter = sunPosition[1].position - center;
        Vector3 setCenter = sunPosition[2].position - center;
        float flacComplete = timeSpand / journeyTime;
        transform.position = Vector3.Slerp(riseCenter, setCenter, flacComplete);
        //transform.position += center;
        timeSpand += Time.deltaTime;
    }
}
