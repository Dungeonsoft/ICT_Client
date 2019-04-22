using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using ICT_Engine;

namespace ICT_Engine
{
    public class KeyTestScript : MonoBehaviour
    {


        public InteractionEvent onInteraction = new InteractionEvent();

        public KeyCode kCode = KeyCode.Space;

        public void Interact()
        {
            Debug.Log("Interact");
            onInteraction.Invoke();
        }



        private void Update()
        {
            if (Input.GetKeyDown(kCode))
            {
                Interact();
            }
        }
    }
}