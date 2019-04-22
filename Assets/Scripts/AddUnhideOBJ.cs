using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;

    public class AddUnhideOBJ : AddClass
{

        [Header("언하이드 시킬 오브젝트")]
        //언하이드 시킬 오브젝트를 넣습니다. 해당 오브젝트는 시작시 자동 하이드 됩니다.
        [SerializeField]
        private GameObject[] hideObj = null;

        public override void MeA(bool isActionCharacter)
        {
            //base.MeA();
            Debug.Log("shit");
            foreach (GameObject Obj in hideObj)
            {
            Obj.SetActive(true);

        }
        }

        private void Start()
        {
        try
        {
            //hideObj 마다 순회하며 하이드
            foreach (GameObject Obj in hideObj)
            {
                Obj.SetActive(false);

            }
        }catch{ Debug.Log("특수동작용 오브젝트가 없습니다.");
        }
        }

    }

