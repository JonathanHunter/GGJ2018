namespace GGJ2018.ObjectPooling.Bullets
{
    using UnityEngine;

    public class BasicBullet : Bullet
    {
        public float speed;
        public Rigidbody rgbdy;

        protected override void LocalInitialize()
        {
        }

        protected override void LocalReInitialize()
        {
        }

        protected override void LocalUpdate()
        {
            this.rgbdy.velocity = this.transform.right * speed;
        }

        protected override void LocalDeallocate()
        {
        }

        protected override void LocalDelete()
        {
        }
    }
}
