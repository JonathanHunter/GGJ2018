namespace GGJ2018.Effects.ExplodingDesk
{
    using UnityEngine;
    using ObjectPooling;

    public class ShardDissolve : MonoBehaviour
    {
        public Renderer ren;
        public float timerDown = 3f;
        public float sliceAmount = 0;

        private Material[] mats;
        private bool sonar;

        // Use this for initialization
        void Start()
        {
            ren = GetComponent<Renderer>();
            mats = ren.materials;
            this.sonar = false;
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
    }
}
