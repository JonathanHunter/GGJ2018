namespace GGJ2018
{
	using UnityEngine;
	using ObjectPooling;

	public class WaterDrop : MonoBehaviour {

		public float time = 4f;
		float timer;

		void Start() 
		{
			time += Random.Range(-time / 2 + 1, time / 2);
			timer = time;
		}
		
		// Update is called once per frame
		void Update () 
		{
			if ((timer -= Time.deltaTime) <= 0) 
			{
				Drop();
				timer = time;
			}
		}

		void Drop() 
		{
			GameObject s = SonarPool.Instance.GetSonar(3f, 1f);
			s.transform.position = this.transform.position;
		}
	}
}
