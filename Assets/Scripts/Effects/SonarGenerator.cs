namespace GGJ2018.Effects
{
    using UnityEngine;
    using ObjectPooling;

    public class SonarGenerator : MonoBehaviour
    {
        public Transform sonarPoint;
        public bool sonarEnabled;
        public float sonarRate;
        public float sonarSpeed;
        public float sonarSize;

        private float sonarTimer;

        private void Start()
        {
            this.sonarTimer = sonarRate;
        }

        private void Update()
        {
            if (Managers.GameState.Instance.CurrentState != Managers.GameState.State.Playing || !this.sonarEnabled)
                return;

            if((this.sonarTimer -= Time.deltaTime) < 0)
            {
                GameObject s = SonarPool.Instance.GetSonar(this.sonarSpeed, this.sonarSize);
                if (s != null)
                    s.transform.position = this.sonarPoint.position;

                this.sonarTimer = this.sonarRate;
            }
        }
    }
}
