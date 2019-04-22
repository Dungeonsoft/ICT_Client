using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;

namespace ICT_Engine
{
    public class RoleEvent : MonoBehaviour
    {

        #region Variables

        public bool isMainRole = false;

        //캐릭터 더미 표시용
        public List<PointerProperty> pList;
        //캐릭터 더미 표시용

        public List<SetActionPointerInfoForCharacter> setAPIC;

        #endregion
    }
}