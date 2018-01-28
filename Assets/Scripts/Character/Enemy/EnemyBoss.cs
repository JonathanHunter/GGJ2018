namespace GGJ2018.Character.Enemy
{
    using UnityEngine;

    public class EnemyBoss : Enemy
    {
        public Transform[] RunLocations;
        public Collider melee;

        private enum State { Chase, Attack, Run, Stunned }

        private State prevState;
        private State currState;
        private int attackHash;
        private int stunHash;
        private bool attackDone;
        private bool doOnce;

        protected override void LocalInit()
        {
            this.currState = State.Chase;
            this.prevState = State.Chase;
            this.attackHash = Animator.StringToHash("Attack");
            this.stunHash = Animator.StringToHash("Stun");
            this.attackDone = false;
            this.doOnce = false;
        }

        protected override void LocalUpdate()
        {
            switch (this.currState)
            {
                case State.Chase: Chase(); break;
                case State.Attack: Attack(); break;
                case State.Run: Run(); break;
                case State.Stunned: Stunned(); break;
            }

            if(this.currState != this.prevState)
            {
                this.prevState = currState;
                this.attackDone = false;
                this.doOnce = false;
            }
        }

        protected override void TookDamage()
        {
        }

        protected override void InAgroRange()
        {
        }

        public void AttackDone()
        {
            this.attackDone = true;
        }

        public void StunDone()
        {

        }

        private void Chase()
        {

        }

        private void Attack()
        {
            if (!this.doOnce)
            {
                this.melee.enabled = true;
                this.doOnce = true;
                this.enemyAnimator.SetTrigger(this.attackHash);
            }

            if(this.attackDone)
            {
                this.melee.enabled = false;
                this.currState = State.Stunned;
            }
        }

        private void Run()
        {

        }

        private void Stunned()
        {

        }
    }
}