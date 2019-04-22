using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataParser
{

    public class PointerDataParser : MonoBehaviour
    {
        public bool isRun = false;

        string actiontextPath = "Actions/Texts/";

        public string fileNamePosition = "data_007_position";
        public string fileNamePath = "data_007_path";

        [SerializeField]
        public string[][] dataPos;
        [SerializeField]
        public string[][] dataPath;


        private void OnDrawGizmos()
        {
            if (isRun == true)
            {
                LoadOriginalData();
                isRun = false;
            }
        }

        void LoadOriginalData()
        {
            string loadDataPos = actiontextPath + fileNamePosition;
            string loadDataPath = actiontextPath + fileNamePath;

            dataPos = CSVReader.ReadS(loadDataPos);
            dataPath = CSVReader.ReadS(loadDataPath);

            InputActionData();
        }

        void InputActionData()
        {
            if (this.transform.childCount > 0)
            {
                int childCnt = this.transform.childCount;
                for (int i = 0; i < childCnt; i++)
                {
                    DestroyImmediate(this.transform.GetChild(0).gameObject);
                }
            }

            for (int i = 1; i < dataPos.Length; i++)
            {
                GameObject GO = new GameObject();
                GO.transform.parent = this.transform;
                GO.name = dataPos[i][0];
                //Debug.Log("dataPos.Length :: "+ dataPos.Length);
                GO.transform.position = new Vector3(float.Parse(dataPos[i][1]), float.Parse(dataPos[i][2]), float.Parse(dataPos[i][3]));
                //Debug.Log("다시 가로줄");
                //for (int j = 0; j < dataPos[0].Length; j++)
                //{
                //    Debug.Log("J Value :: " + j + " ::: dataPos.Length ::" + dataPos.Length);
                //    Debug.Log("dataPos[" + i + "][" + j + "] :: " + dataPos[i][j]);
                //}
            }

            for (int i = 1; i < dataPath[0].Length; i+=3)
            {
                GameObject GO = new GameObject();

                if (i % 3 == 1)
                {
                    GO.transform.parent = this.transform;
                    string str = dataPath[0][i];
                    string[] strr = str.Split('_');
                    GO.name = strr[0] + "_" + strr[1];
                    Debug.Log("Make GameObject :: "+ GO.name);

                    GO.AddComponent<ICT_Engine.EditorPathScript>();
                }

                //Debug.Log("dataPath.Length :: "+ dataPath.Length);
                for (int j = 1; j < dataPath.Length; j ++)
                {
                    //Debug.Log("J Value :: " + j + " ::: dataPath.Length ::" + dataPath.Length);
                    if (dataPath[j][i] != null && dataPath[j][i] != "")
                    {
                        Vector3 pos = new Vector3(float.Parse(dataPath[j][i]) , float.Parse(dataPath[j][i+1]), float.Parse(dataPath[j][i+2]));
                        Debug.Log("dataPath[" + j + "][" + i + "] :: " + pos.x+" :: "+ pos.y + " :: " + pos.z);

                        GameObject point = new GameObject();
                        point.transform.parent = GO.transform;
                        point.transform.position = pos;
                    }
                }
            }
        }
    }
}