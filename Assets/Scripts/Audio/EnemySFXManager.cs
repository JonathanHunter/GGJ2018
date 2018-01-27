namespace GGJ2018.Audio
{
    using UnityEngine;

    /// <summary>
    /// Manager that handles playing player sounds if they are available.
    /// </summary>
    public class EnemySFXManager : MonoBehaviour
    {

        /// <summary>
        /// Enemy step sounds array
        /// </summary>
        public AudioSource[] steps;

        /// <summary>
        /// Enemy melee attack sounds array
        /// </summary>
        public AudioSource[] meleeAttacks;

        /// <summary>
        /// Enemy get hit sounds array
        /// </summary>
        public AudioSource[] getHit;

        /// <summary>
        /// Enemy die sounds array
        /// </summary>
        public AudioSource[] die;

        /// <summary>
        /// Enemy gunfire sound
        /// </summary>
        public AudioSource gunfire;

        /// <summary>
        /// The minimum distance from the enemy in which sounds still play at max volume.
        /// </summary>
        public float minDistance;

        /// <summary>
        /// The maximum distance from the enemy before the sounds fade completely.
        /// </summary>
        public float maxDistance;

        //Overides the min and max distance of each audiosource attached to this enemy
        void Start()
        {
            foreach (AudioSource audio in GetComponentsInChildren<AudioSource>())
            {
                audio.maxDistance = maxDistance;
                audio.minDistance = minDistance;
            }
        }

        /*void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                PlayEnemyStepSFX();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                PlayEnemyMeleeSFX();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                PlayEnemyGetHitSFX();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                PlayEnemyDieSFX();
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                PlayEnemyGunfireSFX();
            }
        }*/

        /// <summary>
        /// Randomly selects and plays a sound out of the enemy step sfx array.
        /// </summary>
        public void PlayEnemyStepSFX()
        {
            Shuffle(steps);
            foreach (AudioSource step in steps)
            {
                if (!step.isPlaying)
                {
                    step.Play();
                    return;
                }
            }
            steps[Random.Range(0, steps.Length)].Play();
        }

        /// <summary>
        /// Randomly selects and plays a sound out of the enemy melee sfx array.
        /// </summary>
        public void PlayEnemyMeleeSFX()
        {
            Shuffle(meleeAttacks);
            foreach (AudioSource meleeAttack in meleeAttacks)
            {
                if (!meleeAttack.isPlaying)
                {
                    meleeAttack.Play();
                    return;
                }
            }
            meleeAttacks[Random.Range(0, meleeAttacks.Length)].Play();
        }

        /// <summary>
        /// Randomly selects and plays a sound out of the enemy get hit sfx array.
        /// </summary>
        public void PlayEnemyGetHitSFX()
        {
            Shuffle(getHit);
            foreach (AudioSource hit in getHit)
            {
                if (!hit.isPlaying)
                {
                    hit.Play();
                    return;
                }
            }
            getHit[Random.Range(0, getHit.Length)].Play();
        }

        /// <summary>
        /// Randomly selects and plays a sound out of the enemy die sfx array.
        /// </summary>
        public void PlayEnemyDieSFX()
        {
            Shuffle(die);
            foreach (AudioSource death in die)
            {
                if (!death.isPlaying)
                {
                    death.Play();
                    return;
                }
            }
            die[Random.Range(0, die.Length)].Play();
        }

        /// <summary>
        /// Plays the enemy gunfire sfx.
        /// </summary>
        public void PlayEnemyGunfireSFX()
        {
            gunfire.Play();
        }

        /// <summary>
        /// Shuffles the array for the sake of randomized audio
        /// </summary>
        /// <param name="array"></param>
        private void Shuffle(AudioSource[] array)
        {
            // Knuth shuffle algorithm :: courtesy of Wikipedia :)
            for (int t = 0; t < array.Length; t++)
            {
                AudioSource tmp = array[t];
                int r = Random.Range(t, array.Length);
                array[t] = array[r];
                array[r] = tmp;
            }
        }
    }
}
