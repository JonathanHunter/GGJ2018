namespace GGJ2018.Managers
{
    using UnityEngine;
    using ObjectPooling;

    public class SceneManager : MonoBehaviour
    {
        public ObjectPool[] pools;
        public PlayerManager pm;

        private void Start()
        {
            pm.Init();
            foreach (ObjectPool p in pools)
                p.Init();
        }
    }
}
