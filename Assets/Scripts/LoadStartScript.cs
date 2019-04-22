using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;

namespace ICT_Engine
{

    public class LoadStartScript : MonoBehaviour {

        public void CBackToMain()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            // 1번: 화재//
            // 2번: 퇴선//
            // 3번: 밀폐구역//
        }
    }
}