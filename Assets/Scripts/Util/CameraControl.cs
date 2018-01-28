namespace GGJ2018.Util
{
    using UnityEngine;

    public class CameraControl : MonoBehaviour
    {
        public bool ForceCursorUnLock;

        private bool shouldLockCursor;

        private void Start()
        {
            this.shouldLockCursor = true;
            CursorLock();
        }

        public float UpdateCamera()
        {
            if (CursorLock())
            {
                Vector2 offset = GetOffset();
                Quaternion rotation = this.transform.rotation * Quaternion.Euler(offset.x, 0f, 0f);
                if (offset.x != 0 && ((360 - rotation.eulerAngles.x < 80) || rotation.eulerAngles.x < 80))
                {
                    float z = this.transform.rotation.eulerAngles.z;
                    this.transform.Rotate(new Vector3(offset.x, 0f, 0f));
                    this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, z);
                }

                return offset.y;
            }

            return 0;
        }

        /// <summary> Locks or unlocks the cursor from the screen. </summary>
        /// <returns> The cursor state. </returns>
        private bool CursorLock()
        {
            if (this.ForceCursorUnLock || Input.GetKey(KeyCode.Escape))
                this.shouldLockCursor = false;
            else if (Input.GetKey(KeyCode.Mouse0))
                this.shouldLockCursor = true;

            if(this.shouldLockCursor && Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if(!this.shouldLockCursor && !Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            return !Cursor.visible;
        }

        /// <summary> Gets input from mouse and controller to get offset vector. </summary>
        /// <returns> The offset vector. </returns>
        private Vector2 GetOffset()
        {
            Vector2 mouse = new Vector2((Managers.GameState.Instance.Inverted ? 1 : -1) * CustomInput.MouseYRaw, CustomInput.MouseXRaw);
            Vector2 controller = new Vector2(
                (Managers.GameState.Instance.Inverted ? 1 : -1) * (CustomInput.Bool(CustomInput.UserInput.LookUp) ? CustomInput.Raw(CustomInput.UserInput.LookUp) : CustomInput.Raw(CustomInput.UserInput.LookDown)),
                CustomInput.Bool(CustomInput.UserInput.LookRight) ? CustomInput.Raw(CustomInput.UserInput.LookRight) : CustomInput.Raw(CustomInput.UserInput.LookLeft));

            return (mouse + controller) * Managers.GameState.Instance.Sensitivity;
        }
    }
}
