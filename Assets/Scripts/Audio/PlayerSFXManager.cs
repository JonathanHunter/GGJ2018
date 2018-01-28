namespace GGJ2018.Audio
{
    using UnityEngine;

    /// <summary>
    /// Manager that handles playing player sounds if they are available.
    /// </summary>
    public class PlayerSFXManager : MonoBehaviour
    {

        /// <summary>
        /// Player step sounds array
        /// </summary>
        public AudioSource[] steps;

        /// <summary>
        /// Player melee attack sounds array
        /// </summary>
        public AudioSource[] meleeAttacks;

        /// <summary>
        /// Player get hit sounds array
        /// </summary>
        public AudioSource[] getHit;

        /// <summary>
        /// Player die sounds array
        /// </summary>
        public AudioSource[] die;

        /// <summary>
        /// Player gunfire sound
        /// </summary>
        public AudioSource[] gunfire;

        /// <summary>
        /// The player's sonar sound
        /// </summary>
        public AudioSource sonar;

        /// <summary>
        /// The player's stick tap sound
        /// </summary>
        public AudioSource stickTap;

        /*void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)){
                PlayPlayerStepSFX();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                PlayPlayerMeleeSFX();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                PlayPlayerGetHitSFX();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                PlayPlayerDieSFX();
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                PlayPlayerGunfireSFX();
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                PlayPlayerSonarSFX();
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                PlayPlayerStickTapSFX();
            }
        }*/

        /// <summary>
        /// Randomly selects and plays a sound out of the player step sfx array.
        /// </summary>
        public void PlayPlayerStepSFX()
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
        /// Randomly selects and plays a sound out of the player melee sfx array.
        /// </summary>
        public void PlayPlayerMeleeSFX()
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
        /// Randomly selects and plays a sound out of the player get hit sfx array.
        /// </summary>
        public void PlayPlayerGetHitSFX()
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
        /// Randomly selects and plays a sound out of the player die sfx array.
        /// </summary>
        public void PlayPlayerDieSFX()
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
        /// Randomly selects and plays a sound out of the player gunfire sfx array.
        /// </summary>
        public void PlayPlayerGunfireSFX()
        {
            Shuffle(gunfire);
            foreach (AudioSource fire in gunfire)
            {
                if (!fire.isPlaying)
                {
                    fire.Play();
                    return;
                }
            }
            gunfire[Random.Range(0, gunfire.Length)].Play();
        }

        /// <summary>
        /// Plays the player sonar sfx.
        /// </summary>
        public void PlayPlayerSonarSFX()
        {
            sonar.Play();
        }

        /// <summary>
        /// Plays the player sticktap sfx. Slightly randomizes volume and pitch
        /// </summary>
        public void PlayPlayerStickTapSFX()
        {
            stickTap.pitch = Random.Range(0.9f, 1.1f);
            stickTap.volume = Random.Range(0.05f, 0.1f);
            stickTap.Play();
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
