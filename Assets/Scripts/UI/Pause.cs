namespace GGJ2018.UI
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class Pause : MonoBehaviour
    {
        [SerializeField]
        private GameObject canvas;
        [SerializeField]
        private GameObject resume;
        [SerializeField]
        private GameObject quit;

        private GameObject currentSelected;

        void Start()
        {
            EventSystem.current.SetSelectedGameObject(resume);
        }

        void Update()
        {
            if (Managers.GameState.Instance.CurrentState == Managers.GameState.State.Paused)
            {
                if (EventSystem.current.currentSelectedGameObject == null)
                    EventSystem.current.SetSelectedGameObject(resume);

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
                    EventSystem.current.SetSelectedGameObject(resume);
                    Navigator.CallSubmit();
                }
            }
            else if(Managers.GameState.Instance.CurrentState != Managers.GameState.State.Paused)
            {
                if (Util.CustomInput.BoolFreshPress(Util.CustomInput.UserInput.Pause))
                {
                    Managers.GameState.Instance.CurrentState = Managers.GameState.State.Paused;
                    EventSystem.current.SetSelectedGameObject(resume);
                    canvas.SetActive(true);
                }
            }
        }

        public void Resume()
        {
            Managers.GameState.Instance.CurrentState = Managers.GameState.State.Playing;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canvas.SetActive(false);
        }

        public void Quit()
        {
            Managers.GameState.Instance.CurrentState = Managers.GameState.State.Playing;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
        }
    }
}