namespace GGJ2018
{
	using UnityEngine;
	using UnityEngine.SceneManagement;
    using UnityEngine.Audio;

	public class TransitionManager : MonoBehaviour {

		public static TransitionManager Instance { get; private set; }
        
		[SerializeField]
        private CanvasGroup overlay = null;
        
        [SerializeField]
        private AudioMixer mixer;
        
        [SerializeField]
        private float speed = 2;
        private AsyncOperation loading;

        private float alpha;
        private bool decreasing;

        private string nextScene;


		void Awake()
		{
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Duplicate TransitionManager detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
			
			this.alpha = 0;
            this.decreasing = true;
            FadeOut();
        }

        private void Update()
        {
            if (this.alpha > 0 && this.decreasing)
            {
                this.alpha -= Time.deltaTime * this.speed;
                if (this.alpha < 0)
                    this.alpha = 0;

                this.overlay.alpha = this.alpha;
            }

            if (loading != null && !loading.isDone)
            {
                if (this.alpha < 1 && !this.decreasing)
                {
                    this.alpha += Time.deltaTime * this.speed;
                    if (this.alpha < 0)
                        this.alpha = 0;

                    this.overlay.alpha = this.alpha;
                }
            }
            else if (loading != null && loading.isDone)
            {
                loading.allowSceneActivation = true;
                loading = null;
            }
        }

		public void LoadScene(string scene) {
            FadeIn();
			loading = SceneManager.LoadSceneAsync(scene);
		}
        public void FadeIn()
        {
            this.overlay.alpha = 0f;
            this.alpha = 0f;
            this.decreasing = false;
        }

        public void Show()
        {
            this.overlay.alpha = 1f;
            this.alpha = 1f;
            this.decreasing = false;
        }

        public void FadeOut()
        {
            this.overlay.alpha = 1f;
            this.alpha = 1f;
            this.decreasing = true;
        }
	}
}
