namespace GGJ2018.ObjectPooling.Bullets
{
    using ObjectPooling;
    using UnityEngine;

    public abstract class Bullet : MonoBehaviour, IPoolable
    {
        [SerializeField]
        private int damage = 1;
        [SerializeField]
        private float lifeTime = 1;
        [SerializeField]
        private BulletPool.BulletTypes type = BulletPool.BulletTypes.Player;

        public BulletPool.BulletTypes Type { get { return this.type; } }

        public MeleeWeaponTrail[] trails;

        private int referenceIndex = 0;
        private float currentLifeTime = 0;

        private void Update()
        {
            if (Managers.GameState.Instance.CurrentState != Managers.GameState.State.Playing)
            {
                Rigidbody r = this.GetComponent<Rigidbody>();
                if (r != null)
                    r.velocity = Vector3.zero;
                return;
            }

            LocalUpdate();
            if ((this.currentLifeTime -= Time.deltaTime) <= 0)
            {
                ReturnBullet();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (Managers.GameState.Instance.CurrentState != Managers.GameState.State.Playing)
                return;

            this.currentLifeTime = 0;
            GameObject g = SonarPool.Instance.GetSonar(1f, 1f);
            if(g != null)
                g.transform.position = this.transform.position;
        }

        public IPoolable SpawnCopy(int referenceIndex)
        {
            Bullet bullet = Instantiate<Bullet>(this);
            bullet.referenceIndex = referenceIndex;
            return bullet;
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
            LocalInitialize();
        }

        public void ReInitialize()
        {
            LocalReInitialize();
            this.currentLifeTime = this.lifeTime;
            this.gameObject.SetActive(true);
        }

        public void Deallocate()
        {
            LocalDeallocate();
            this.gameObject.SetActive(false);
        }

        public void Delete()
        {
            LocalDelete();
            Destroy(this.gameObject);
        }

        public int GetDamage()
        {
            return this.damage;
        }

        protected void ReturnBullet()
        {
            BulletPool.Instance.ReturnBullet(this.type, this.gameObject);
        }

        /// <summary> Local Update for subclasses. </summary>
        protected abstract void LocalUpdate();
        /// <summary> Local Initialize for subclasses. </summary>
        protected abstract void LocalInitialize();
        /// <summary> Local ReInitialize for subclasses. </summary>
        protected abstract void LocalReInitialize();
        /// <summary> Local Deallocate for subclasses. </summary>
        protected abstract void LocalDeallocate();
        /// <summary> Local Delete for subclasses. </summary>
        protected abstract void LocalDelete();
    }
}
