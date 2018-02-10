namespace GGJ2018.Character.Enemy
{
    using UnityEngine;

    public class EnemyAnimRedirect : MonoBehaviour
    {
        public EnemyBoss boss;

        public void AttackDone()
        {
            this.boss.AttackDone();
        }

        public void StunDone()
        {
            this.boss.StunDone();
        }

        public void playSFX()
        {
            this.boss.playPunchSound();
        }
    }
}
