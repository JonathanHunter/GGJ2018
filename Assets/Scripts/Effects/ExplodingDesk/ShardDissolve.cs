using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardDissolve : MonoBehaviour {

    public float lifeTime = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.GetComponent<Collider>().enabled = false;
        lifeTime -= Time.deltaTime;

        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }
	}
}
