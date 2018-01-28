
namespace GGJ2018
{
	using UnityEngine;
	using UnityEngine.SceneManagement;

	public class SceneTransition : MonoBehaviour 
	{
		public string nextScene;
		void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				TransitionManager.Instance.LoadScene(nextScene);
			}
		}
	}
}
