namespace GGJ2018.Effects
{
    using UnityEngine;

    public class RecordPlayer : MonoBehaviour
    {
        public MeshRenderer mesh;
        public Collider col;
        public SonarGenerator sonar;
        public Util.SoundPlayer sfx;
        public int maxHealth;

        private int health;

        private void Start()
        {
            sonar.sonarEnabled = true;
            this.sfx.loop = true;
            this.sfx.PlaySong(0);
            this.health = this.maxHealth;
        }

        private void Update()
        {
            if (health <= 0)
            {
                this.mesh.enabled = false;
                this.col.enabled = false;
                this.sonar.sonarEnabled = false;
                this.sfx.PlaySong(1);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "PlayerWeapon")
            {
                this.health -= collision.gameObject.GetComponent<ObjectPooling.Bullets.Bullet>().GetDamage();

            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "PlayerWeapon") // if hit by player bullet
            {
                ObjectPooling.Bullets.Bullet temp = other.gameObject.GetComponent<ObjectPooling.Bullets.Bullet>();
                if (temp == null)
                {
                    this.health -= 2;
                }
                else
                {
                    this.health -= temp.GetDamage();
                }
            }
            if (other.gameObject.tag == "CaneWeaponHitBox" && other.GetComponent<BoxCollider>() != null) //hit by player cane
            {
                other.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
