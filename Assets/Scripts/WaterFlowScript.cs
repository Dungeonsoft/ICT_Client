using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFlowScript : MonoBehaviour {
    public  Material mat;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
        mat.mainTextureOffset = new Vector2(0, Time.time/10f+.2f);
        mat.SetTextureOffset("_DetailAlbedoMap", new Vector2(0, Time.time / 12f + .25f));
	}
}
