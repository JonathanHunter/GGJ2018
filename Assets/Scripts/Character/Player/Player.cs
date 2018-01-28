namespace GGJ2018.Character.Player
{
    using UnityEngine;
    using Audio;
    using Effects;
    using ObjectPooling;
    using UI;
    using Util;

    public class Player : MonoBehaviour
    {
        public Animator anim;
        public Rigidbody rgdb;
        public float moveSpeed;
        public CameraControl cameraControl;
        public Collider meleeCollider;
        public PlayerSFXManager sfx;
        public Transform foot;
        public Transform tip;
        public SphereCollider agro;
        public int maxBullets;
        public GunUI reloadUI;
        public ScreechUI screechUI;
        public CameraEffects camEffects;
        public int maxHealth;
        public HealthUI healthUI;
        public Transform gunPos;

        private int moveHash;
        private int meleeHash;
        private int shootHash;
        private int bullets;
        private bool meleeing;
        private bool shooting;
        private bool reloading;
        private bool recharging;
        private float originalRadius;
        private bool dead;

        public int Health { get; private set; }

        private bool Moving
        {
            get
            {
                return CustomInput.BoolHeld(CustomInput.UserInput.Left) ||
                    CustomInput.BoolHeld(CustomInput.UserInput.Right) ||
                    CustomInput.BoolHeld(CustomInput.UserInput.Up) ||
                    CustomInput.BoolHeld(CustomInput.UserInput.Down);
            }
        }

        private void Start()
        {
            this.moveHash = Animator.StringToHash("move");
            this.meleeHash = Animator.StringToHash("melee");
            this.shootHash = Animator.StringToHash("shoot");
            this.meleeing = false;
            this.originalRadius = this.agro.radius;
            this.bullets = this.maxBullets;
            this.reloading = false;
            this.recharging = false;
            this.Health = this.maxHealth;
            this.healthUI.SetMaxHealth(this.maxHealth);
            this.dead = false;
        }

        private void Update()
        {
            if (Managers.GameState.Instance.CurrentState != Managers.GameState.State.Playing)
                return;

            if(this.Health <= 0 && !this.dead)
            {
                GameOver.Instance.Show();
                this.sfx.PlayPlayerDieSFX();
                this.dead = true;
            }

            float yRot = cameraControl.UpdateCamera();
            if (yRot != 0)
            {
                float z = this.transform.rotation.eulerAngles.z;
                this.transform.Rotate(new Vector3(0f, yRot, 0f));
                this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, z);
            }

            if (this.Moving)
                Move();

            this.anim.SetBool(this.moveHash, this.Moving);

            if (!this.meleeing && CustomInput.BoolFreshPress(CustomInput.UserInput.Melee))
            {
                this.meleeCollider.enabled = true;
                this.meleeing = true;
                this.anim.SetBool(this.meleeHash, true);
                this.sfx.PlayPlayerMeleeSFX();
            }
            else if (this.meleeing)
                this.rgdb.velocity *= .5f;

            if (!reloading && !shooting && CustomInput.BoolFreshPress(CustomInput.UserInput.Shoot))
            {
                this.shooting = true;
                GameObject g = BulletPool.Instance.GetBullet(BulletPool.BulletTypes.Player);
                g.transform.position = gunPos.position;
                g.transform.rotation = this.transform.rotation;
                this.anim.SetBool(this.shootHash, true);
                this.agro.radius += 1f;
                this.sfx.PlayPlayerGunfireSFX();
                this.reloadUI.Fire(this.bullets);
            }
            else if (this.shooting)
                this.rgdb.velocity *= .75f;

            if(!reloading && (bullets <= 0 || CustomInput.BoolFreshPress(CustomInput.UserInput.Reload)))
            {
                this.reloading = true;
                this.reloadUI.StartReload();
            }
            else if(this.reloading && !this.reloadUI.IsReloading)
            {
                this.bullets = this.maxBullets;
                this.reloading = false;
            }

            if (!this.recharging && CustomInput.BoolFreshPress(CustomInput.UserInput.Screech))
            {
                this.recharging = true;
                this.agro.radius *= 2f;
                this.screechUI.StartRecharge();
                this.camEffects.Screech();
                GameObject s = SonarPool.Instance.GetSonar(1f, 10f);
                s.transform.position = this.foot.position;
                s = SonarPool.Instance.GetSonar(8f, 10f);
                s.transform.position = this.foot.position;
                this.sfx.PlayPlayerSonarSFX();
            }
            else if (this.recharging && !this.screechUI.IsRecharging)
                this.recharging = false;

            if (this.agro.radius > this.originalRadius)
                this.agro.radius -= Time.deltaTime * .75f;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == "Enemy")
            {
                int damage = collision.gameObject.GetComponent<Enemy.Enemy>().GetDamage();
                this.Health -= damage;
                this.healthUI.TakeDamage(damage);
                this.sfx.PlayPlayerGetHitSFX();
                this.camEffects.TakeDamage();
            }
            else if(collision.gameObject.tag == "EnemyWeapon")
            {
                int damage = collision.gameObject.GetComponent<ObjectPooling.Bullets.Bullet>().GetDamage();
                this.Health -= damage;
                this.healthUI.TakeDamage(damage);
                this.sfx.PlayPlayerGetHitSFX();
                this.camEffects.TakeDamage();
            }
        }

        private void Move()
        {
            int x;
            int y;
            if (CustomInput.BoolHeld(CustomInput.UserInput.Up))
                y = 1;
            else if (CustomInput.BoolHeld(CustomInput.UserInput.Down))
                y = -1;
            else
                y = 0;

            if (CustomInput.BoolHeld(CustomInput.UserInput.Left))
                x = -1;
            else if (CustomInput.BoolHeld(CustomInput.UserInput.Right))
                x = 1;
            else
                x = 0;

            Vector2 dir = new Vector2(x, y);
            this.rgdb.velocity = (this.transform.right * dir.x + this.transform.forward * dir.y) * this.moveSpeed;
        }

        public void StepEvent()
        {
            GameObject s = SonarPool.Instance.GetSonar(2f, .5f);
            s.transform.position = this.foot.position;
            this.sfx.PlayPlayerStepSFX();
        }

        public void MeleeDone()
        {
            this.meleeCollider.enabled = true;
            this.meleeing = false;
            this.anim.SetBool(this.meleeHash, false);
        }

        public void ShootDone()
        {
            this.shooting = false;
            this.anim.SetBool(this.shootHash, false);
            this.bullets--;
        }

        public void Tap()
        {
            GameObject s = SonarPool.Instance.GetSonar(1f, 2f);
            s.transform.position = this.tip.position;
            this.sfx.PlayPlayerStickTapSFX();
        }
    }
}
