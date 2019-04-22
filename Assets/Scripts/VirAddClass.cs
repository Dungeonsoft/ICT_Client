using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;

namespace ICT_Engine
{
    public class VirAddClass : AddClass
    {
        public Face face;
        public HandL handL;
        public HandR handR;
        public Helmet helmet;
        public Mask mask;
        public Dress dress;
        public ShoesAll shoes;


        // 보통 이부분은 옷 갈아입기를 하는 함수를 연결하는데.
        // 옷 갈아입기 함수는 그냥 하는게 아니라 먼저 박스를 선택하는 동작을 한다.
        // 그 동작이 이루어지지 않으면 당연히 옷 갈아입기는 되질 않는다.
        public override void MeA(bool isActionCharacter)
        {
            // 우선 현재 캐릭터가 플레이어 캐릭터인지 확인한다.
            // 플레이어 캐릭터이면 터치인풋스크립트를 찾아서 그곳 델리게이트에.
            // 차후에 실행되어야 할 스크립트를 연결하여 준다.
            if (isActionCharacter == true)
            {
                ControllerKeyInputTest tInput = GameObject.Find("TouchInput").transform.GetComponent<ControllerKeyInputTest>();

                Debug.Log("::::::::::::::::::::::::::::");
                Debug.Log("::::::::::::::::::::::::::::");
                Debug.Log("::::::::::::::::::::::::::::");
                Debug.Log("Action Number ::: "+this.GetComponent<MoveOnPathScript>().actionNum);
                Debug.Log("::::::::::::::::::::::::::::");
                Debug.Log("Add Action ::: Change Cloths");
                Debug.Log("::::::::::::::::::::::::::::");
                Debug.Log("::::::::::::::::::::::::::::");
                Debug.Log("::::::::::::::::::::::::::::");
                tInput.AddNewAction(ChangeCloths);
            }
            else
            {
                ChangeCloths();
            }
        }

        public override void MeB(bool isActionCharacter)
        {
            CharacterShapeSetting chsetting = ch.GetComponent<CharacterShapeSetting>();
            chsetting.ChOri();
        }

        public void ChangeCloths()
        {
            CharacterShapeSetting chsetting = ch.GetComponent<CharacterShapeSetting>();
            chsetting.ChangeCh(face, handL, handR, helmet, mask, dress, shoes);
        }
    }
}