using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;

namespace ICT_Engine
{
    public class AddClass : MonoBehaviour
    {

        public Transform ch;

        public bool isWater;

        public GameObject fireWater;

        public Transform LeftController;

        //public EllipsoidParticleEmitter em;
        private void Awake()
        {
            ch = transform.Find("CharacterAllSet");
            if (transform.Find("FireWaterPa") != null)
            {
                fireWater = transform.Find("FireWaterPa").gameObject;
                //em = transform.Find("Font").GetComponent<EllipsoidParticleEmitter>();
                //em.emit = false;
                fireWater.SetActive(false);
            }
        }

        public virtual void MeA(bool isActionCharacter)
        {

        }
        public virtual void MeB(bool isActionCharacter)
        {

        }

        public virtual void MeC(bool isActionCharacter)
        {
            if (isActionCharacter == true)
            {
                //BG와 End Ui 두개를 찾아서 켜준다//
                GameObject bgUI =
                    GameObject.FindWithTag("MainCam").transform.GetChild(0).GetChild(1).Find("BG").gameObject;
                GameObject endUI =
                    GameObject.FindWithTag("MainCam").transform.GetChild(0).GetChild(1).Find("EndPanel").gameObject;
                bgUI.SetActive(true);
                endUI.SetActive(true);

                GetComponent<MoveOnPathScript>().SendCompleteDataToServer();

            }
        }

        public virtual void ClickIn()
        {
            isWater = true;
            Debug.Log("물나옴 " + isWater);
            fireWater.SetActive(true);
            GetLeftController();
        }

        public virtual void ClickOut()
        {
            isWater = false;
            Debug.Log("물그만 " + isWater);
            fireWater.SetActive(false);
        }

        void GetLeftController()
        {
            LeftController = GameObject.Find("LeftController").transform;
            Debug.Log("Left Con Angles::: "+ LeftController.eulerAngles);
        }

        private void Update()
        {
            if (isWater == true)
            {
                fireWater.transform.eulerAngles = new Vector3(0, LeftController.eulerAngles.y, 0);

                Debug.Log("fireWater Angles::: " + fireWater.transform.eulerAngles);
                Debug.Log("Left Con Angles::: " + LeftController.eulerAngles);

            }
        }
    }
}