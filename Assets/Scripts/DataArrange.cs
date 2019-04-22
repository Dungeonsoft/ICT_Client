using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataArrange : MonoBehaviour {
    public GameObject scrollBase;
    public GameObject userTagPrefab;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddChild(string uData)
    {

    }

    public void ChildrenArrange()
    {
        List<Transform> chData = new List<Transform>();

        int chCount = scrollBase.transform.childCount;
        for (int i = 0; i < chCount; i++)
        {
            Transform ch = scrollBase.transform.GetChild(i);
            float h= ch.GetComponent<RectTransform>().rect.height;

        }
    }
}
