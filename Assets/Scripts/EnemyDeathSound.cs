using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathSound : MonoBehaviour {

    public AudioSource sfx1, sfx2;

    public void Start()
    {
        int ran = Random.Range(0, 2);
        if (ran == 0)
        {
            sfx1.Play();
        }
        else
        {
            sfx2.Play();
        }
        Destroy(gameObject, 5.0f);
    }
}
