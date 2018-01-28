namespace GGJ2018.Effects.ExplodingDesk
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using ObjectPooling;

    public class ShardDissolve : MonoBehaviour
    {

        public Renderer ren;


        private Material[] mats;

        public float timerDown = 3f;

        public float sliceAmount = 0;

        // Use this for initialization
        void Start()
        {
            ren = GetComponent<Renderer>();
            mats = ren.materials;
            //mat = GetComponent<Renderer>().material;
            GameObject s = SonarPool.Instance.GetSonar(7f, 5f);
            s.transform.position = transform.position;
        }

        // Update is called once per frame
        void Update()
        {


            if (timerDown > 0)
            {
                timerDown -= Time.deltaTime;
            }
            else
            {
                if (sliceAmount >= .98f)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    sliceAmount = 0.5f + Mathf.Sin(Time.time) * 0.5f;
                    foreach (Material m in mats)
                    {
                        m.SetFloat("_SliceAmount", sliceAmount);
                    }
                }

            }


        }
        void OnCollisionEnter(Collision collision)
        {
            GameObject s = SonarPool.Instance.GetSonar(3f, 3f);
            if (s != null)
            {
                s.transform.position = transform.position;
            }
        }
        
    }
}