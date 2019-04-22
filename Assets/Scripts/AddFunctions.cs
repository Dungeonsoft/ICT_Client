using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;

namespace ICT_Engine
{
    public enum PointerStyle
    {
        GuidePointer = 0,
        ItemPointer,
        CharacterPointer,
        NotiPointer
    }

    /// <summary>
    /// 모션 총46종
    /// </summary>
    public enum NextCharcterMotion
    {
        Batterycharge_01,
        Batterycharge_02,
        Batterycharge_03,
        Batterycharge_04,
        CPR_01,
        Damper_01,
        DoorHeatCheck_01,
        FaintAway_01,
        GasCheck_01,
        Idle_01,
        Lashing_01,
        Lashing_02,
        Lashing_03,
        LifeBoatEngineTest_01,
        Lifeline_01,
        Lifeline_02,
        Lifeline_03,
        Lifeline_04,
        Lookout_01,
        Lying_01,
        ManholeClosing_01,
        MemberCount_01,
        MushroomVent_01,
        Paper_01,
        PatientCheck_01,
        Sitting_01,
        Stair_01,
        Stair_02,
        Standup_01,
        Telephone_A_01,
        Telephone_A_02,
        Telephone_A_03,
        Telephone_B_01,
        Telephone_B_02,
        Telephone_B_03,
        Telephone_B_04,
        Tranceiver_01,
        Tripod_01,
        Tripod_02,
        VerticalLadder_01,
        VerticalLadder_02,
        Walking_01,
        Walking_02,
        Walking_03,
        Walking_04,
        Walking_05,
        Writing_01
    }



    [System.Serializable]
    public class GetDataFromServer
    {
        public string StructuresName = "Structures";
        public string sctionPointersName = "ActionPointers";
        public string CharctersName = "Characters";
    }

    [System.Serializable]
    public class SetActionPointerInfoForCharacter
    {
        //Text Bubble는 대화창//
        //항상 대화는 나(선택된 롤)부터//
        //순서가 바뀌었다면 첫번째는 공란으로 놔둔다//
        [System.NonSerialized]
        public string pointerName;
        [Header("--------------------------------------------")]
        [Header("타 캐릭터에게 영향을 받을지 여부를 결정하는 bool변수 True:영향")]
        //MoveOnPathScript.inputActionBool의 영향을 받을지를 여기서 결정한다//
        //true면 영향을 받는 것// false면 영향을 받지 않는것//
        public bool isInputActionB;

        public MoveOnPathScript connectedRole;
        public int setActionNum;

        [Header("질답전에 이뤄지는 동작이 있을경우 지정하세요")]
        public NextCharcterMotion fChMotion = NextCharcterMotion.Idle_01;
        public List<NextCharcterMotion> fChMotionNew;

        [Header("첫 대기시간")]
        public float waitTime01 = 10f;
        [Header("-무전기나 음성으로 말풍선이 나올경우 아래에 적으세요")]
        [Header("_화자")]
        public string[] roleName1;
        [Header("_내용")]
        public string[] textBubble1;
        [Header("말풍선의 음성을 지정합니다.")]
        public AudioClip[] textBubble1Audio;
        [Header("-아래에 질문을 적으세요")]
        public string question;
        [Header("-아래에 선택지를 적으세요")]
        public List<string> answers;
        [Header("-최소 하나 이상을 정답으로 지정하세요")]
        public List<bool> correct;
        [Header("-질답이 끝난 후 대화가 있을 경우 아래에 적으세요")]
        [Header("_화자")]
        public string[] roleName2;
        [Header("_내용")]
        public string[] textBubble2;
        [Header("말풍선의 음성을 지정합니다.")]
        public AudioClip[] textBubble2Audio;
        [Header("질답이 끝나고 특정동작을 수행시 동작을 지정하세요")]
        public NextCharcterMotion nChMotion = NextCharcterMotion.Idle_01;
        public List<NextCharcterMotion> nChMotionNew;

        [Header("처음 서있는 위치를 지정하세요")]
        public Transform standingPosition;
        [Header("움직일 경우 path를 지정하세요")]
        public EditorPathScript movePath;

        [Header("다른캐릭터에무언가를 전달할때 이부분은 위에 isInputActionB와 연결된다")]
        [Header("첫번째 대화시")]
        public InputAction[] inAction1;//첫번째 대화시
        [Header("두번째 질답후(문제)")]
        public InputAction[] inAction2;//두번째 질답후(문제

        [Header("질답다음에 독특한 기능을 실행하고 싶을때")]
        [Header("AddClass MeA 실행")]
        public bool isMeA;
        [Header("AddClass MeB 실행")]
        public bool isMeB;
        [Header("AddClass MeC 실행")]
        public bool isMeC;

        [Header("소화기물작동_ON")]
        public bool isWaterOn;
        [Header("소화기물작동_OFF")]
        public bool isWaterOff;

        [Header("액션이 가지고 있는 고유 Enum")]
        public ActionEnum aEnum;
    }

    [System.Serializable]
    public class InputAction
    {
        public MoveOnPathScript otherSA;
        public int inputActionNum;
        // 타 캐릭터와 소통하는 부분을 위의 액션 넘버를 연결하는 것이 아닌.
        // 각 액션에 이넘 변수를 찾아서 연결하는 방식으로 변경하려고.
        // 아래와 같이 새롭게 변수를 생성한다.
        public ActionEnum inputActionEnum;
    }


    [System.Serializable]
    public class InteractionEvent : UnityEngine.Events.UnityEvent { }

    public delegate void Del();

    public class AskingControl
    {
        public string Question;
        public string[] answers;
    }

    public enum CharacterSpeed { slow, normal, high }

    public enum ActionEnum
    {
        a00, a01, a02, a03, a04, a05, a06, a07, a08, a09,
        a10, a11, a12, a13, a14, a15, a16, a17, a18, a19,
        a20, a21, a22, a23, a24, a25, a26, a27, a28, a29,
        a30, a31, a32, a33, a34, a35, a36, a37, a38, a39,
        a40, a41, a42, a43, a44, a45, a46, a47, a48, a49,
        a50, a51, a52, a53, a54, a55, a56, a57, a58, a59,
        a60, a61, a62, a63, a64, a65, a66, a67, a68, a69,
        a70, a71, a72, a73, a74, a75, a76, a77, a78, a79,
        a80, a81, a82, a83, a84, a85, a86, a87, a88, a89,
        a90, a91, a92, a93, a94, a95, a96, a97, a98, a99,
        b00, b01, b02, b03, b04, b05, b06, b07, b08, b09,
        b10, b11, b12, b13, b14, b15, b16, b17, b18, b19,
        b20, b21, b22, b23, b24, b25, b26, b27, b28, b29,
        b30, b31, b32, b33, b34, b35, b36, b37, b38, b39,
        b40, b41, b42, b43, b44, b45, b46, b47, b48, b49,
        b50, b51, b52, b53, b54, b55, b56, b57, b58, b59,
        b60, b61, b62, b63, b64, b65, b66, b67, b68, b69,
        b70, b71, b72, b73, b74, b75, b76, b77, b78, b79,
        b80, b81, b82, b83, b84, b85, b86, b87, b88, b89,
        b90, b91, b92, b93, b94, b95, b96, b97, b98, b99,
        c00, c01, c02, c03, c04, c05, c06, c07, c08, c09,
        c10, c11, c12, c13, c14, c15, c16, c17, c18, c19,
        c20, c21, c22, c23, c24, c25, c26, c27, c28, c29,
        c30, c31, c32, c33, c34, c35, c36, c37, c38, c39,
        c40, c41, c42, c43, c44, c45, c46, c47, c48, c49,
        c50, c51, c52, c53, c54, c55, c56, c57, c58, c59,
        c60, c61, c62, c63, c64, c65, c66, c67, c68, c69,
        c70, c71, c72, c73, c74, c75, c76, c77, c78, c79,
        c80, c81, c82, c83, c84, c85, c86, c87, c88, c89,
        c90, c91, c92, c93, c94, c95, c96, c97, c98, c99,
    }
}