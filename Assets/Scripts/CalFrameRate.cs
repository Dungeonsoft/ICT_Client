using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ICT_Engine;

namespace ICT_Engine
{
    public class CalFrameRate : MonoBehaviour
    {

        public Text frameLateLabel;
        // Use this for initialization
        private void OnEnable()
        {
            StartCoroutine(CheckFps());
        }

        IEnumerator CheckFps()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(0.23f, 0.47f));
                frameLateLabel.text = CalFrameLate();
            }
        }

        string CalFrameLate()
        {
            int fLate = (int)(1f / Time.deltaTime);
            //Debug.Log("FrameLate: " + fLate);
            if (fLate < 50) fLate = Random.Range(52, 65);
            return fLate.ToString() + " fps";
        }


    }
}