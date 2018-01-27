namespace GGJ2018
{
    using UnityEngine;
    using ObjectPooling;

    public class Test : MonoBehaviour
    {
        public SonarPool pool;
        public BulletPool bpool;

        private void Start()
        {
            pool.Init();
            bpool.Init();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.E))
            {
                GameObject s = pool.GetSonar(Random.Range(1f, 6f), Random.Range(1f, 6f));
                if (s != null)
                    s.transform.position = new Vector3(Random.Range(-13f, 12f), 0f, Random.Range(-7f, 12f));
                else
                    Debug.Log("AAAA");
            }
        }
    }
}
