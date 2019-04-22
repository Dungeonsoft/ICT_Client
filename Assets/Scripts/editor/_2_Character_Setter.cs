using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ICT_Engine;

namespace ICT_Engine
{
    public class _2_Character_Setter : EditorWindow
    {

        #region Variables
        //스트럭쳐와 액션포인터 트랜스폼을 저장하는 변수 선언//
        static Transform AllTransform;
        static Transform Characters;
        static Transform structures;
        static Transform actionPointers;

        public Vector2 scrollPosition;

        //각각영역에 대한 폴딩 기능을 담당하는 불 변수 어레이//
        static List<bool> commonFoldout = new List<bool>();
        #endregion

        [MenuItem("ICT 프로젝트/2.기본캐릭터 설정")]
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

            //주요 위치가 되는 게임오브젝트(트랜스폼)을 정의한다//
            Characters = SetImportantTransform("Characters", "Characters", "AllTransform");
            structures = SetImportantTransform("Structures", "Structures", "AllTransform");
            actionPointers = SetImportantTransform("ActionPointers", "ActionPointers", "AllTransform");


            for (int i = 0; i < Characters.childCount; i++)
            {
                commonFoldout.Add(true);
            }

            EditorWindow.GetWindow(typeof(_2_Character_Setter));
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
            //스크롤 뷰 시작//
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false);
            ///////////////////////////////
            //그 리스트에서 액션 포인터를 추가 삭제할 수 있게 하자//
            //그 액션 포인터의 속성에 따라//
            //노티와 플레이어 포인터 두가지가 있는데//
            //이 두가지에 따라서 어떤 반응이 될 것인지를//
            //설정할 수 있게 하자//
            //내일 꼭 완성하고 못하고 철야//
            ////////////////////////////////

            //여기서 플레이어를 리스트로 보이게 하고//
            for (int i = 0; i < Characters.childCount; i++) ListCharacter(i);

            //스크롤 뷰 끝//
            EditorGUILayout.EndScrollView();
        }

        void ListCharacter(int i)
        {
            string chName = Characters.GetChild(i).name;
            //Debug.Log("chName: "+ chName);
            commonFoldout[i] = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), commonFoldout[i], chName, true);

            if (commonFoldout[i])
            {
                //Debug.Log("Show I Value: " + i);
                Transform thisT = Characters.GetChild(i);

                if (!thisT.GetComponent<RoleShow>())
                {
                    thisT.gameObject.AddComponent<RoleShow>();
                }

                if (!thisT.GetComponent<RoleEvent>())
                {
                    thisT.gameObject.AddComponent<RoleEvent>();
                }

                //캐릭터를 선택//
                GUI.color = Color.cyan;
                if (GUILayout.Button(chName + " 선택"))
                {
                    Selection.activeGameObject = Characters.GetChild(i).gameObject;
                }

                GUI.color = Color.white;
                //캐릭터에 추가 액션 포인터를 지정//
                if (GUILayout.Button(chName + " 액션포인터 추가"))
                {
                    Transform gC = Characters.GetChild(i);

                    if (!Selection.activeTransform.GetComponent<PointerProperty>())
                    {
                        EditorUtility.DisplayDialog("액션 포인터를 선택하지 않았습니다.", "액션포인터를 선택하고 다시 시작하여 주세요", "확인");
                    }
                    else
                    {
                        foreach (var t in Selection.transforms)
                        {
                            Debug.Log("t 이름: " + t.name);
                            Debug.Log("gC 이름: " + gC.name);
                            gC
                                .GetComponent<RoleEvent>()
                                .pList
                                .Add
                                (t.GetComponent<PointerProperty>());
                            SetActionPointerInfoForCharacter spic = new SetActionPointerInfoForCharacter();
                            spic.pointerName = t.name;
                            gC.GetComponent<RoleEvent>().setAPIC.Add(spic);
                        }

                        if (gC.position != gC.GetComponent<RoleEvent>().pList[0].transform.position)
                        {
                            gC.position = Selection.activeTransform.position;
                        }
                    }
                }
                //Debug.Log("Show I Value: " + i);
                //포인터 리스트가 0보다 크면(캐릭터에 지정된 포인터가 있으면)//

                RoleEvent cInfo
                 = Characters.GetChild(i).GetComponent<RoleEvent>();

                //pList와 setAPIC의 수가 틀릴경우 맞춰준다//
                #region
                //if (cInfo.pList != null && cInfo.pList.Count > 0)
                //{

                //    //포인터의 갯수(cInfo.pList)와 각 포인터에서 실행될 내용을 정의하는 것(cInfo.setAPIC)의 수가 다를 경우//
                //    if (cInfo.pList.Count != cInfo.setAPIC.Count)
                //    {
                //        if (cInfo.pList.Count > cInfo.setAPIC.Count)
                //        {
                //            //이 부분에서 시작시 SetAPIC가 하나 이상일경우가 있으니 우선 그부분은 제하고 추가하여야 한다//
                //            int startVal = cInfo.setAPIC.Count;

                //            for (int j = startVal; j < cInfo.pList.Count; j++)
                //            {
                //                SetActionPointerInfoForCharacter p = new SetActionPointerInfoForCharacter();

                //                p.answers = new List<string>();
                //                p.correct = new List<bool>();
                //                cInfo.setAPIC.Add(p);
                //            }
                //        }
                //        else
                //        {
                //            //혹시 setAPIC가 pList보다 크면 넘치는 부분은 제거한다//
                //            cInfo.setAPIC.RemoveRange(cInfo.pList.Count, cInfo.setAPIC.Count - 1);
                //        }
                //    }
                //}
                #endregion

                //포인터의 속성을 표시하고 선택과 제거를 할 수 있게 한다//
                #region
                for (int k = 0; k < cInfo.pList.Count; k++)
                {
                    GUILayout.BeginHorizontal(GUILayout.Width(Mathf.FloorToInt(Screen.width * 0.9f)));
                    GUILayout.FlexibleSpace();

                    //Debug.Log("cInfo.pList.Count: " + cInfo.pList.Count);
                    if (cInfo.pList[k] != null)
                    {
                        //Debug.Log("널이 아닙니다.");
                        cInfo.pList[k].pointerStyle
                                = (PointerStyle)EditorGUILayout.EnumPopup(cInfo.pList[k].name + "속성", cInfo.pList[k].pointerStyle);

                        GUI.color = Color.cyan;
                        if (GUILayout.Button("선택"))
                        {
                            Selection.activeTransform = cInfo.pList[k].transform;
                            break;
                        }

                        //필요없는 포인터를 제거할때//
                        //포인터의 리스트인 plist와 정보를 관리하는setAPIC도 제거한다//
                        GUI.color = Color.red;
                        if (GUILayout.Button("제거"))
                        {
                            cInfo.pList.RemoveAt(k);
                            cInfo.setAPIC.RemoveAt(k);
                            GUILayout.EndHorizontal();
                            break;
                        }
                    }
                    GUI.color = Color.white;

                    //    //포인터가 캐릭터포인터일경우 이부분에서 세팅을 한다//
                    //    if (cInfo.pList[k] != null && cInfo.pList[k].pointerStyle == PointerStyle.CharacterPointer)
                    //    {
                    //        Debug.Log("Answer(" + k + ") Count: " + cInfo.setAPIC[k].answers.Count);
                    //        if (cInfo.setAPIC[k].answers == null || cInfo.setAPIC[k].answers.Count < 1)
                    //        {
                    //            Debug.Log(cInfo.name + ":: 포인터스타일이 캐릭터이나 Null 상태여서 세팅을 한다.");

                    //            cInfo.setAPIC[k].answers = new List<string>();
                    //            cInfo.setAPIC[k].correct = new List<bool>();

                    //            //기존 정보가 없을때는 새롭게 만든다//
                    //            for (int l = 0; l < 3; l++)
                    //            {
                    //                Debug.Log("I Value: " + l);
                    //                cInfo.setAPIC[k].Question = "여기에 질문을 적으세요";
                    //                cInfo.setAPIC[k].answers.Add("여기에 답지를 적으세요");
                    //                cInfo.setAPIC[k].correct.Add(false);
                    //            }

                    //            Debug.Log("cInfo.setAPIC[" + k + "].querys.Count" + cInfo.setAPIC[k].answers.Count);
                    //        }
                    //        else
                    //        {
                    //            cInfo.setAPIC[k].answers.Clear();
                    //            cInfo.setAPIC[k].correct.Clear();
                    //            //있으면 그냥 유지한다//
                    //            //Debug.Log("cInfo.setAPIC[" + k + "].querys is " + "existant");
                    //        }

                    //    }
                    //    else
                    //    {
                    //        cInfo.setAPIC[k] = new SetActionPointerInfoForCharacter();
                    //        //if (cInfo.setAPIC[k].querys != null)
                    //        //    if (cInfo.setAPIC[k].querys.Count != 0)
                    //        //        cInfo.setAPIC[k].querys = new List<string>();
                    //        //if (cInfo.setAPIC[k].answers != null)
                    //        //    if (cInfo.setAPIC[k].answers.Count != 0)
                    //        //    cInfo.setAPIC[k].answers = new List<bool>();
                    //    }

                    GUILayout.EndHorizontal();
                }
                #endregion
            }
        }
    }

}