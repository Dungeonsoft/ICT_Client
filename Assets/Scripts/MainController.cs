using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;
using DataParser;

namespace ICT_Engine
{
    public class MainController : MonoBehaviour
    {
        Transform allTransform;

        public Transform structures;
        public Transform actionPointers;
        public Transform characters;

        public GetDataFromServer getData;

        [Header("패쓰와위치를 외부csv파일을 통해서 가지고 온 경우 아래를 true로 변경")]
        public bool isPointsCsvData = false;

        Transform[] allChildren;

        private void Awake()
        {
            allTransform = transform.parent;

            GetAllChildren(allTransform);

            //지정된_이름의_트랜스폼을_찾는다//
            structures = FindChildDeep(getData.StructuresName);

            if (isPointsCsvData == false)
                actionPointers = FindChildDeep(getData.sctionPointersName);
            else if (isPointsCsvData == true)
                actionPointers = GameObject.Find(getData.sctionPointersName).transform;

            characters = FindChildDeep(getData.CharctersName);

            //Get Data From Server//
            //                    //  
            //Get Data From Server//

            List<TextDataParser> tParser = new List<TextDataParser>();
            
            for( int i = 0; i< characters.childCount; i++)
            {
                GameObject ch = characters.GetChild(i).gameObject;
                if (ch.activeSelf == true)
                {
                    if (ch.GetComponent<TextDataParser>())
                    {
                        //Debug.Log("Ch name :: "+ ch.name);
                        tParser.Add(ch.GetComponent<TextDataParser>());
                    }
                }
            }

            Debug.LogWarning("T parser Count1: "+tParser.Count);
            for(int i =0; i< tParser.Count; i++ )
            {
                //Debug.Log("I Value :: "+i);
                //Debug.Log("T Name: "+ tParser[i].name);
                tParser[i].UserAwake();           
            }

            //Debug.LogWarning("T parser Count2: " + tParser.Count);
            foreach (TextDataParser t in tParser)
            {
                //Debug.Log("T Name ConnectInAction: " + t.name);
                StartCoroutine(t.GetComponent<MoveOnPathScript>().ConnectInAction());
            }
        }

        private void Start()
        {
        }

        private Transform FindChildDeep(string findName)
        {

            int cnt = allChildren.Length;
            for (int i = 0; i < cnt; i++)
            {
                if (allChildren[i].name == findName)
                    return allChildren[i].transform;
            }

            //Debug.LogError("찾고_있는_트랜스폼이_없습니다");
            return null;
        }

        private void GetAllChildren(Transform tParent)
        {
            allChildren = tParent.GetComponentsInChildren<Transform>();
        }
    }
}