using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertChildList : MonoBehaviour {

    public Transform pa;

    public bool isInvert = false;


    void Invert()
    {
        if (pa == null)
        {
            pa= this.transform;
        }
        int paChCnt = pa.childCount;
        GameObject tempPa = new GameObject();

        for (int i = 0 ; i < paChCnt; i++ )
        {
            pa.GetChild(0).parent = tempPa.transform;
            Debug.Log("Export :: "+ i);
        }
        Debug.Log("Done!_1");

        for (int i = 0; i < paChCnt; i++)
        {
            tempPa.transform.GetChild(tempPa.transform.childCount-1).parent = pa;
            Debug.Log("Import :: " + i);
        }
        Debug.Log("Done!_2");
        DestroyImmediate(tempPa);
        pa = null;
        Debug.Log("Finish!");
    }

    void OnDrawGizmos()
    {
        if (isInvert == true)
        {
            Invert();
            isInvert = false;
        }
    }

}
