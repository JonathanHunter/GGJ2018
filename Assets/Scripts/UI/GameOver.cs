namespace GGJ2018.UI
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class GameOver : MonoBehaviour
    {
        [SerializeField]
        private GameObject canvas;
        [SerializeField]
        private GameObject retry;
        [SerializeField]
        private GameObject quit;
        [SerializeField]
        private string scene;

        private GameObject currentSelected;

        /// <summary> Singleton instance for this object pool. </summary>
        public static GameOver Instance { get; private set; }

        void Start()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Duplicate Game Over detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            EventSystem.current.SetSelectedGameObject(retry);
        }

        void Update()
        {   
            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(retry);

            currentSelected = EventSystem.current.currentSelectedGameObject;

            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Up))
                Navigator.Navigate(Util.CustomInput.UserInput.Up, currentSelected);
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Down))
                Navigator.Navigate(Util.CustomInput.UserInput.Down, currentSelected);
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Accept))
                Navigator.CallSubmit();
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Cancel) ||
                Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Pause))
            {
                EventSystem.current.SetSelectedGameObject(retry);
                Navigator.CallSubmit();
            }
        }

        public void Retry()
        {
            Managers.GameState.Instance.CurrentState = Managers.GameState.State.Playing;
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }

        public void Quit()
        {
            Managers.GameState.Instance.CurrentState = Managers.GameState.State.Playing;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
        }

        public void Show()
        {
            canvas.SetActive(true);
            Managers.GameState.Instance.CurrentState = Managers.GameState.State.Paused;
        }
    }
}