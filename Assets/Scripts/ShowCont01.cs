using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;

namespace ICT_Engine
{
    public class ShowCont01 : MonoBehaviour
    {
        public GameObject showObj;
        public GameObject hideObj;

        // Use this for initialization
        void OnEnable()
        {
            showObj.SetActive(true);
            hideObj.SetActive(false);
        }

    }
}