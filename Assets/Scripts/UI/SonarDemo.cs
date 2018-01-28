namespace GGJ2018.UI
{
    using UnityEngine;
    using ObjectPooling;

    public class SonarDemo : MonoBehaviour
    {
        private float time = .05f;

        private void Update()
        {
            if ((time -= Time.deltaTime) < 0)
            {
                GameObject s = SonarPool.Instance.GetSonar(Random.Range(1f, 3f), Random.Range(1f, 6f));
                if (s != null)
                    s.transform.position = new Vector3(Random.Range(-1f, 12f), 0f, Random.Range(-6f, 4f));

                time = .05f;
            }
        }
    }
}
