namespace GGJ2018.ObjectPooling
{
    using UnityEngine;

    public class SonarPool : ObjectPool
    {
        public Sonar sonarPrefab;
        public int sonarCount;

        /// <summary> Singleton instance for this object pool. </summary>
        public static SonarPool Instance { get; private set; }

        protected override void PreInit()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Duplicate Sonar Pool detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
        }
        
        protected override IPoolable[] GetTemplets()
        {
            return new IPoolable[] { sonarPrefab };
        }

        protected override int[] GetPoolSizes()
        {
            return new int[] { sonarCount };
        }

        public GameObject GetSonar(float speed, float scale)
        {
            IPoolable s = AllocateEntity(sonarPrefab);
            if (s == null)
                return null;

            s.GetGameObject().GetComponent<Sonar>().SetGrowth(speed, scale);
            return s.GetGameObject();
        }

        public void ReturnSonar(GameObject sonar)
        {
            Debug.Log("a");
            IPoolable s = sonar.GetComponent<IPoolable>();
            DeallocateEntity(sonarPrefab, s);
        }
    }
}
