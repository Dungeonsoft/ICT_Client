using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ICT_Engine;

namespace ICT_Engine
{
    public class OnEnableScript : MonoBehaviour
    {

        public Image eng;
        public Image kor;
        public Image fpsOn;
        public Image fpsOff;
        public Image latOn;
        public Image latOff;

        public Color btnBaseColorOn;
        public Color btnBaseColorOff;

        public void OnEnable()
        {
            if (ConstDataScript.langNum == 0)
            {
                eng.color = btnBaseColorOn;
                kor.color = btnBaseColorOff;
            }
            else
            if (ConstDataScript.langNum == 1)
            {
                eng.color = btnBaseColorOff;
                kor.color = btnBaseColorOn;
            }

            if (ConstDataScript.isFpsOn == true)
            {

                fpsOn.color = btnBaseColorOn;
                fpsOff.color = btnBaseColorOff;
            }
            else
            {
                fpsOn.color = btnBaseColorOff;
                fpsOff.color = btnBaseColorOn;
            }

            if (ConstDataScript.isLatOn == true)
            {
                latOn.color = btnBaseColorOn;
                latOff.color = btnBaseColorOff;
            }
            else
            {
                latOn.color = btnBaseColorOff;
                latOff.color = btnBaseColorOn;
            }
        }
    }
}
