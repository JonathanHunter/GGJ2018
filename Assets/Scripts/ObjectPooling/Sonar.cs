namespace GGJ2018.ObjectPooling
{
    using UnityEngine;

    public class Sonar : MonoBehaviour, IPoolable
    {
        //[SerializeField]
        //private Material[] sonarRings;

        private float maxScale;
        private float speed;
        public int referenceIndex;

        private void Update()
        {
            float s = this.transform.localScale.x;
            if (s < maxScale)
            {
                s += Time.deltaTime * this.speed;
                this.transform.localScale = Vector3.one * s;
                SetAlpha(1f - (s / maxScale));
            }
            else
                ReturnSonar();
        }

        public void SetGrowth(float speed, float size)
        {
            this.speed = speed;
            this.maxScale = size;
        }

        public IPoolable SpawnCopy(int reference)
        {
            Sonar sonar = Instantiate<Sonar>(this);
            sonar.referenceIndex = reference;
            return sonar;
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public int GetReferenceIndex()
        {
            return this.referenceIndex;
        }

        public void Initialize()
        {
            this.speed = 1;
            this.maxScale = 1;
        }

        public void ReInitialize()
        {
            SetAlpha(1f);
            this.transform.localScale = Vector3.zero;
            this.gameObject.SetActive(true);
        }

        public void Deallocate()
        {
            this.gameObject.SetActive(false);
        }

        public void Delete()
        {
            Destroy(this.gameObject);
        }

        protected void ReturnSonar()
        {
            SonarPool.Instance.ReturnSonar(this.gameObject);
        }

        private void SetAlpha(float a)
        {
            //foreach (Renderer r in sonarRings)
            //{
            //    Color c = r.;
            //    c.a = a;
            //    r.color = c;
            //}
        }
    }
}
