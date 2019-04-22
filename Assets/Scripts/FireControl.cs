using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICT_Engine;

namespace ICT_Engine
{
    public class FireControl : MonoBehaviour
    {
        Del upDel;
        List<ParticleSystem> par = new List<ParticleSystem>();

        public float delayTime = 10f;

        void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                par.Add(transform.GetChild(i).GetComponent<ParticleSystem>());
            }
            foreach (var p in par)
            {
                var pm = p.main;
                pm.startLifetime = 0;
                p.gameObject.SetActive(false);
            }

            StartCoroutine(SStartFire());

        }

        IEnumerator SStartFire()
        {
            yield return new WaitForSeconds(delayTime);

            foreach (var p in par)
            {
                var pm = p.main;
                pm.startLifetime = 0;
                p.gameObject.SetActive(true);
            }

            upDel += StartFire;
        }
        float spendTime;

        void StartFire()
        {
            foreach (var p in par)
            {
                var pm = p.main;
                pm.startLifetime = Mathf.Lerp(0f, 1f, spendTime);
            }
            spendTime += Time.deltaTime * 0.5f;

            if (spendTime > 1)
            {
                upDel -= StartFire;
            }

        }
        void EndFire()
        {
            foreach (var p in par)
            {
                var pm = p.main;
                pm.startLifetime = Mathf.Lerp(1f, 0.3f, spendTime);
            }
            spendTime += Time.deltaTime * 0.5f;

            if (spendTime > 1)
            {
                upDel -= EndFire;

                //transform.gameObject.SetActive(false);
            }
        }
        public void StartEndFire()
        {
            spendTime = 0;
            upDel += EndFire;
        }

        private void Update()
        {
            if (upDel != null)
            {
                upDel();
            }
        }
    }
}