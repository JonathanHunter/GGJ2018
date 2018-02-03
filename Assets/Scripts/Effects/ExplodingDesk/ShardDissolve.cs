namespace GGJ2018.Effects.ExplodingDesk
{
    using UnityEngine;
    using ObjectPooling;

    public class ShardDissolve : MonoBehaviour
    {
        public Renderer ren;
        public float timerDown = 3f;
        public float sliceAmount = 0;
        //public float explosionTime = .5f;

        private Material[] mats;
        //private float explosionTimer;
        private bool sonar;

        // Use this for initialization
        void Start()
        {
            ren = GetComponent<Renderer>();
            mats = ren.materials;
            //mat = GetComponent<Renderer>().material;
            //GameObject s = SonarPool.Instance.GetSonar(7f, 5f);
            //s.transform.position = transform.position;
            //this.explosionTimer = this.explosionTime;
            this.sonar = false;
        }

        // Update is called once per frame
        void Update()
        {
            //this.explosionTimer -= Time.deltaTime;

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
            //if (!this.sonar && this.explosionTimer <= 0)
            //{
            //    GameObject s = SonarPool.Instance.GetSonar(3f, 3f);
            //    if (s != null)
            //    {
            //        s.transform.position = transform.position;
            //    }

            //    this.explosionTimer = this.explosionTime;
            //    this.sonar = true;
            //}
        }        
    }
}