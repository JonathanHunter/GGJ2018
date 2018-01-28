namespace GGJ2018.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField]
        private Toggle inverted;
        [SerializeField]
        private Slider sensitivy;
        [SerializeField]
        private GameObject backButton;

        private GameObject currentSelected;
        private bool inMain;
        private bool inCredits;

        void Start()
        {
            EventSystem.current.SetSelectedGameObject(backButton);
        }

        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(backButton);

            currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected == this.sensitivy.gameObject)
            {
                if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Left))
                {
                    this.sensitivy.value -= .2f;
                    Managers.GameState.Instance.Sensitivity = this.sensitivy.value;
                }

                if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Right))
                {
                    this.sensitivy.value += .2f;
                    Managers.GameState.Instance.Sensitivity = this.sensitivy.value;
                }
            }
            
            if (currentSelected == this.sensitivy.gameObject)
            {
                if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Accept))
                {
                    this.inverted.isOn = !this.inverted.isOn;
                    Managers.GameState.Instance.Inverted = this.inverted.isOn;
                }
            }

            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Up))
                Navigator.Navigate(Util.CustomInput.UserInput.Up, currentSelected);
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Down))
                Navigator.Navigate(Util.CustomInput.UserInput.Down, currentSelected);
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Left))
                Navigator.Navigate(Util.CustomInput.UserInput.Left, currentSelected);
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Right))
                Navigator.Navigate(Util.CustomInput.UserInput.Right, currentSelected);
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Accept))
                Navigator.CallSubmit();
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Cancel))
            {
                EventSystem.current.SetSelectedGameObject(backButton);
                Navigator.CallSubmit();
            }
        }

        public void ChangeSensitivity(float change)
        {
            Managers.GameState.Instance.Sensitivity = change;
        }
        
        public void ChangeToggle(bool val)
        {
            Managers.GameState.Instance.Inverted = val;
        }
    }
}