using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ICT_Engine;


namespace ICT_Engine
{
    public class _1_ICT_Creator : EditorWindow
    {
        bool groupEnabled;

        //스트럭쳐와 액션포인터 트랜스폼을 저장하는 변수 선언//
        static Transform AllTransform;
        static Transform Characters;
        static Transform structures;
        static Transform actionPointers;


        static Color[] pointerColors;

        //방 생성후 나타나는 메뉴와 관련된 변수들//
        static List<Transform> area = new List<Transform>();

        public Vector2 scrollPosition;

        ////방 생성후 나타나는 메뉴와 관련된 함수, 변수들//
        int countPointersX = 1;
        int countPointersZ = 1;
        //임시 포인터를 이용하여 포인터들이 생성될 영역을 지정한다//
        Vector3 guidePos1;
        Vector3 guidePos2;

        //각각영역에 대한 폴딩 기능을 담당하는 불 변수 어레이//
        static List<bool> commonFoldout = new List<bool>();



        [MenuItem("ICT 프로젝트/1.기본환경 생성기")]
        public static void Open()
        {
            //올트랜스폼과 스트럭쳐, 액션포인터 정의//
            if (GameObject.FindWithTag("AllTransform"))
            {
                AllTransform = GameObject.FindWithTag("AllTransform").transform;
                AllTransform.name = "AllTransform";
            }
            else if (GameObject.Find("AllTransform"))
            {
                AllTransform = GameObject.Find("AllTransform").transform;
                AllTransform.tag = "AllTransform";
            }
            else
            {
                AllTransform = new GameObject().transform;
                AllTransform.name = "AllTransform";
                AllTransform.tag = "AllTransform";
            }

            Characters = SetImportantTransform("Characters", "Characters", "AllTransform");
            structures = SetImportantTransform("Structures", "Structures", "AllTransform");
            actionPointers = SetImportantTransform("ActionPointers", "ActionPointers", "AllTransform");

            EditorWindow.GetWindow(typeof(_1_ICT_Creator));

            pointerColors = new Color[4];
            pointerColors[0] = new Color32(0x83, 0x83, 0x83, 0x7F);//8383837F
            pointerColors[1] = new Color32(0x00, 0xA9, 0x40, 0xFF);//00A940FF
            pointerColors[2] = new Color32(0xFF, 0xC3, 0x00, 0xFF);//FFC300FF
            pointerColors[3] = new Color32(0x5E, 0x6C, 0xFF, 0xFF);//5E6CFFFF

            for (int i = 0; i < actionPointers.childCount; i++)
            {
                commonFoldout.Add(true);
            }
        }

        private static Transform SetImportantTransform(string tName, string tTagName, string pTagName)
        {
            GameObject Go;

            //태그를 이용하여 오브젝트를 찾는다//
            //태그로 된 오브젝트가 있으면...//
            if (GameObject.FindWithTag(tTagName))
            {
                //임시 변수 Go에 할당해준다//
                Go = GameObject.FindWithTag(tTagName);
            }
            //태그로 된 오브젝트가 없으면...//
            else
            {
                //이름으로 찾아본다//
                if (GameObject.Find(tName))
                    //이름으로 찾은 것을// 
                    //임시 변수 Go에 할당해준다//
                    Go = GameObject.Find(tName);
                //이름과 태그 둘다 없으면//
                else
                    //오브젝트가 존재하지 않으니 새로 생성한다.//
                    Go = new GameObject();
            }

            //태그와 이름을 지정하여 준다//
            Go.tag = tTagName;
            Go.name = tName;

            //최상위 트랜스폼(All Transform)이 아닐경우 최상위 트랜스폼에 차일드로 넣어준다.//
            if (tTagName != pTagName)
                Go.transform.parent = GameObject.FindWithTag(pTagName).transform;

            return Go.transform;
        }

        private void OnGUI()
        {
            groupEnabled = EditorGUILayout.BeginToggleGroup("Pointer Color Settings", groupEnabled);
            {
                GUILayout.Label("Base Settings", EditorStyles.boldLabel);
                EditorGUILayout.ColorField(pointerColors[0], GUILayout.Width(100));
                EditorGUILayout.ColorField(pointerColors[1], GUILayout.Width(100));
                EditorGUILayout.ColorField(pointerColors[2], GUILayout.Width(100));
                EditorGUILayout.ColorField(pointerColors[3], GUILayout.Width(100));
            }
            EditorGUILayout.EndToggleGroup();

            //버튼이 눌리면 Make Room을 실행한다.//
            if (GUILayout.Button("구역 생성"))
            {
                MakeRoom();
            }

            if (GUILayout.Button("구역 정렬"))
            {
                GameObject Go = new GameObject();
                int cnt = actionPointers.childCount;

                //재정렬을 위해 액션포인터스에 있는 모든 차일드를 밖으로 뺀다//
                for (int i = 0; i < cnt; i++)
                {
                    actionPointers.GetChild(0).parent = Go.transform;
                }

                //스트럭쳐스에 있는 차일드의 이름과 비교하여 순서대로 다시 액션포인터스에 다시 넣는다//
                for (int i = 0; i < structures.childCount; i++)
                {
                    string cName = structures.GetChild(i).name;
                    for (int j = 0; j < Go.transform.childCount; j++)
                    {
                        if (cName == Go.transform.GetChild(j).name)
                        {
                            Go.transform.GetChild(j).parent = actionPointers;
                            break;
                        }
                    }
                }
                for (int k = 0; k < commonFoldout.Count; k++)
                {
                    commonFoldout[k] = true;
                }

                DestroyImmediate(Go);
            }


            //포인터를 가로 세로 몇개를 생성할지 결정한다//
            countPointersX = EditorGUILayout.IntSlider(countPointersX, 1, 50);
            countPointersZ = EditorGUILayout.IntSlider(countPointersZ, 1, 50); ;

            //스크롤 뷰 시작//
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false);

            //생성된 룸을 윈도에 표시해준다//
            for (int i = 0; i < actionPointers.childCount; i++) PointerMakerM(i);
            //스크롤 뷰 끝//
            EditorGUILayout.EndScrollView();
        }

        void MakeRoom()
        {
            //스트럭쳐에서 액션 포인트가 만들어지지 않은 곳을 찾아서 자동으로 만들어 준다//
            while (true)
            {
                Transform sChild = CheckChild();
                if (sChild != null)
                {
                    //폴딩을 담당하는 불린값을 리스트에 추가하여 준다.
                    commonFoldout.Add(true);
                    Debug.Log("commonFoldout Count: " + commonFoldout.Count);

                    //오브젝트를 생성하고 리스트에 담는다//
                    GameObject GO = new GameObject();
                    area.Add(GO.transform);
                    GO.name = sChild.name;

                    //센터위치를 스트럭쳐의 룸의 바운드를 기준으로 생성한다//
                    if (sChild.childCount > 0)
                    {
                        GO.transform.position = sChild.GetChild(0).GetComponent<Renderer>().bounds.center;
                    }
                    else
                    {
                        GO.transform.position = sChild.position;
                    }
                    GO.transform.parent = actionPointers;
                    //GO.AddComponent<PointerMaker>();
                    //GO.GetComponent<PointerMaker>().pointerColors = pointerColors;
                }
                else
                {
                    return;
                }
            }

            //스트럭쳐에서 액션 포인트가 만들어지지 않은 곳을 찾아서 자동으로 만들어 준다//
        }

        void PointerMakerM(int i)
        {
            GUI.color = Color.white;

            //포인터가 추가되었는지를 기록함//

            Transform roomT = actionPointers.GetChild(i);

            commonFoldout[i] = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), commonFoldout[i], roomT.name, true);

            if (commonFoldout[i])
            {
                GUI.color = new Color32(0xF5, 0xF6, 0xCE, 0xFF);
                if (GUILayout.Button("가이드 생성"))
                {
                    //가이드나 포인터가 있는지 확인후 삭제를 결정한다//
                    if (roomT.childCount > 0 &&
                        EditorUtility.DisplayDialog("포인터(가이드)가 있습니다",
                            "기존 가이드 또는 포인터를 삭제하시겠습니까?",
                            "예", "아니오"))
                    {
                        DestroyChild(roomT);
                        MakeGuide(roomT);
                    }
                    else if (roomT.childCount == 0)
                    {
                        MakeGuide(roomT);
                    }
                }
                if (roomT.childCount == 2 && roomT.GetChild(0).name.Contains("Guide"))
                {
                    GUILayout.BeginHorizontal(GUILayout.Width(Mathf.FloorToInt(Screen.width * 0.9f)));
                    {
                        GUILayout.FlexibleSpace();
                        for (int c = 0; c < 2; c++)
                        {
                            if (GUILayout.Button("Guide" + c.ToString("00") + " 선택"))
                            {
                                Selection.activeGameObject = roomT.Find("Guide" + c.ToString("00")).gameObject;
                            }
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }

                GUI.color = new Color32(0xCE, 0xF6, 0xF5, 0xFF);
                if (GUILayout.Button("포인터 생성"))
                {
                    //정해진 갯수만큼 포인터를 생성한다//
                    #region 예외사항 부분 팝업창 띄움.
                    int cCnt = roomT.childCount;
                    if (cCnt == 0)
                    {
                        EditorUtility.DisplayDialog("가이드가 설정되지 않았습니다",
                            "가이드를 생성 후 영역지정을 해주세요.",
                            "확인");
                    }
                    else
                    {
                        #region 가이드 위치값 확보.
                        //가이드에서 위치값을 가져온다//
                        if (roomT.childCount == 2)
                        {
                            guidePos1 = roomT.GetChild(0).position;
                            guidePos2 = roomT.GetChild(1).position;
                        }
                        #endregion

                        if (roomT.GetChild(0).name.Contains("Pointer"))
                        {
                            EditorUtility.DisplayDialog("포인터가 있습니다",
                                "이미 포인터를 생성하였습니다.\n" +
                                "재생성을 원하면 가이드 생성부터 시작하세요.",
                                "확인");
                        }

                        else if (guidePos1 == guidePos2)
                        {
                            EditorUtility.DisplayDialog("가이드 위치를 설정해주세요",
                                "가이드 위치 설정이 되어 있지 않습니다.\n " +
                                "먼저 가이드 위치 설정을 해주세요.",
                                "확인");
                        }
                        #endregion

                        #region 확보된 위치값을 기준으로 포인터 생성.
                        else
                        {
                            //포인터가 추가되었음을 기록함//
                            DestroyChild(roomT);

                            //가이드 위치값을 기준으로 포인터를 생성한다//
                            float pIntX = 1;
                            float pIntZ = 1;

                            //Vector3 pointC = guidePos1;

                            if (guidePos1.x > guidePos2.x) pIntX *= -1;
                            if (guidePos1.z > guidePos2.z) pIntZ *= -1;

                            float interX = 0;
                            float interZ = 0;

                            if (countPointersX - 1 > 0)
                                interX = Mathf.Abs(guidePos1.x - guidePos2.x) / (countPointersX - 1) * pIntX;
                            else
                                interX = Mathf.Abs((guidePos1.x - guidePos2.x) / 2);

                            if (countPointersZ - 1 > 0)
                                interZ = Mathf.Abs(guidePos1.z - guidePos2.z) / (countPointersZ - 1) * pIntZ;
                            else
                                interZ = Mathf.Abs((guidePos1.z - guidePos2.z) / 2);

                            float interXadd = interX;
                            float interZadd = interZ;
                            //Debug.Log("interZadd: " + interZadd);

                            for (int k = 0; k < countPointersZ; k++)
                            {
                                if (countPointersZ > 1) interZadd = interZ * k;
                                for (int l = 0; l < countPointersX; l++)
                                {
                                    GameObject GO = new GameObject();
                                    GO.name = "Pointer" + (k * countPointersX + l).ToString("000");

                                    GO.AddComponent<PointGizmo>();
                                    GO.AddComponent<PointerProperty>();

                                    GO.GetComponent<PointGizmo>().pointerColors = pointerColors;
                                    GO.GetComponent<PointGizmo>().pProperty = GO.GetComponent<PointerProperty>();
                                    GO.GetComponent<PointerProperty>().pointerStyle = PointerStyle.NotiPointer;
                                    GO.transform.parent = roomT;

                                    if (countPointersX == 1 && countPointersZ == 1)
                                    {
                                        GO.transform.position = (guidePos1 + guidePos2) / 2f;
                                    }
                                    else
                                    {
                                        if (countPointersX > 1) interXadd = interX * l;
                                        GO.transform.position = guidePos1 + new Vector3(interXadd, 0, interZadd);
                                        //Debug.Log("Make");
                                    }
                                }
                            }
                            //가이드 위치값을 기준으로 포인터를 생성한다//
                        }
                    }
                    #endregion
                }

                GUI.color = new Color32(0x58, 0x82, 0xFA, 0xFF);
                if (GUILayout.Button("포인터 추가"))
                {
                    GameObject GO = new GameObject();
                    GO.name = "Pointer" + (roomT.childCount + 1).ToString("000");
                    GO.transform.parent = roomT;
                    GO.transform.localPosition = new Vector3(0, 10, 0);

                    GO.AddComponent<PointGizmo>();
                    GO.AddComponent<PointerProperty>();

                    GO.GetComponent<PointGizmo>().pointerColors = pointerColors;
                    GO.GetComponent<PointGizmo>().pProperty = GO.GetComponent<PointerProperty>();
                    GO.GetComponent<PointerProperty>().pointerStyle = PointerStyle.NotiPointer;

                    Selection.activeGameObject = GO;
                }

                if (roomT.childCount > 0)
                {
                    if (roomT.GetChild(0).name.Contains("Guide"))
                        GUI.color = Color.yellow;
                    if (roomT.GetChild(0).name.Contains("Pointer"))
                        GUI.color = Color.cyan;
                }
                else
                {
                    GUI.color = Color.white;
                }


                if (GUILayout.Button("구역 선택"))
                {
                    Selection.activeGameObject = structures.Find(roomT.name).gameObject;
                }
                GUI.color = Color.red;

                if (GUILayout.Button("구역 삭제"))
                {
                    if (roomT.childCount > 0 && roomT.GetChild(0).name.Contains("Pointer") &&
                        EditorUtility.DisplayDialog("생성된 포인터가 삭제됩니다.",
                        "이미 포인터를 생성하였습니다.\n" +
                        "삭제하시면 모든 데이터가 삭제됩니다.\n" +
                        "삭제하시겠습니까?",
                        "네", "아니오"))
                    {
                        commonFoldout.RemoveAt(i);
                        DestroyImmediate(roomT.gameObject);
                    }
                    else
                    {
                        commonFoldout.RemoveAt(i);
                        DestroyImmediate(roomT.gameObject);
                    }
                }
            }
        }

        Transform CheckChild()
        {
            //스트럭쳐에서 액션 포인트가 만들어지지 않은 곳을 찾아서 자동으로 만들어 준다//
            int structuresChildCount = structures.childCount;
            int actionPointersChildCount = actionPointers.childCount;
            bool hasSameNameChild = false;
            //이미 만들어진 액션포인터가 있는지 확인한다//
            //스트럭쳐스와 액션포인터스에 같은 이름의 차일드가 있는지 확인한다//
            //없다면 처음 발견된 그 차일드 이름으로 액션포인터를 생성한다//
            for (int i = 0; i < structuresChildCount; i++)
            {
                hasSameNameChild = false;
                Transform sChild = structures.GetChild(i);
                string schildName = sChild.name;


                for (int j = 0; j < actionPointersChildCount; j++)
                {
                    //차일드가 있는지 확인한다//
                    //차일드가 없어야 새로이 액션포인터의 차일드를 만들게 되니//
                    //이부분 확인은 꼭 필요하다//
                    if (schildName == actionPointers.GetChild(j).name)
                    {
                        hasSameNameChild = true;
                        break;
                    }
                }

                //스트럭쳐스와 액션포인터스 안에 같은 이름의 차일드가 없으면//
                //스트럭쳐스의 차일드 트랜스폼을 리턴해준다//
                //리턴해줌으로서 이 이름으로 액션포인터스에 같은 이름의 차일드를 만들 수 있다//
                if (hasSameNameChild == false) return sChild;
            }
            //아무 것도 발견되지 않고 for 루프를 다 돌면 null을 리턴해준다//
            //null을 리턴함으로서 아무런 액션포인터스의 차일드를 만들지 않는다//
            return null;
        }

        void DestroyChild(Transform t)
        {
            int cnt = t.childCount;
            for (int i = 0; i < cnt; i++)
            {
                DestroyImmediate(t.GetChild(0).gameObject);
            }
        }

        void MakeGuide(Transform roomT)
        {
            for (int j = 0; j < 2; j++)
            {
                GameObject GO = new GameObject();
                GO.name = "Guide" + j.ToString("00");
                GO.transform.parent = roomT;
                GO.transform.localPosition = new Vector3(0, 10, 0);

                GO.AddComponent<PointGizmo>();
                GO.AddComponent<PointerProperty>();

                GO.GetComponent<PointerProperty>().pointerStyle = PointerStyle.GuidePointer;
                GO.GetComponent<PointGizmo>().pointerColors = pointerColors;
                GO.GetComponent<PointGizmo>().pProperty = GO.GetComponent<PointerProperty>();
            }

            //Repaint();
        }
    }
}