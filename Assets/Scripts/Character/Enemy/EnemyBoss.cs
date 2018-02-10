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
        private int currentRunIndex;
        private bool stunOver;

        protected override void LocalInit()
        {
            this.currState = State.Chase;
            this.prevState = State.Chase;
            this.attackHash = Animator.StringToHash("Attack");
            this.stunHash = Animator.StringToHash("Stun");
            this.attackDone = false;
            this.doOnce = false;
            this.stunOver = false;
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
                this.stunOver = false;
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
            this.stunOver = true;
        }

        private void Chase()
        {
            this.agent.isStopped = false;
            this.agent.SetDestination(Managers.PlayerManager.Instance.player.position);

            if (Vector3.Distance(Managers.PlayerManager.Instance.player.position, this.transform.position) < 2.5f)
            {
                this.agent.isStopped = true;
                this.currState = State.Attack;
            }
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
                this.enemyAnimator.SetTrigger(this.stunHash);
            }
        }

        private void Run()
        {
            if(!this.doOnce)
            {
                this.currentRunIndex = Random.Range(0, 3);
                this.doOnce = true;
            }
            
            this.agent.isStopped = false;
            this.agent.destination = this.RunLocations[this.currentRunIndex].position;

            if (Vector3.Distance(this.transform.position, this.RunLocations[this.currentRunIndex].position) < 0.5f)
            {
                this.agent.isStopped = true;
                this.currState = State.Chase;
            }
        }

        private void Stunned()
        {
            if (this.stunOver)
            {
                this.currState = State.Run;
            }
        }

        public void playPunchSound()
        {
            sfx.PlayBoss1PunchSFX();
        }

    }
}