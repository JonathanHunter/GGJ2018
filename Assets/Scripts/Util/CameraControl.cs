﻿namespace GGJ2018.Util
{
    using UnityEngine;

    public class CameraControl : MonoBehaviour
    {
        public bool ForceCursorUnLock;
        public bool inverted;
        public float sensitivity;

        private bool shouldLockCursor;

        private void Start()
        {
            this.shouldLockCursor = true;
            CursorLock();
        }

        private void Update()
        {
            if (CursorLock())
            {
                Vector2 offset = GetOffset();
                this.transform.rotation = this.transform.rotation * Quaternion.Euler(offset.x, offset.y, 0f);
                this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            }
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
            Vector2 mouse = new Vector2((this.inverted ? 1 : -1) * CustomInput.MouseYRaw, CustomInput.MouseXRaw);
            Vector2 controller = new Vector2(
                (this.inverted ? 1 : -1) * (CustomInput.Bool(CustomInput.UserInput.LookUp) ? CustomInput.Raw(CustomInput.UserInput.LookUp) : CustomInput.Raw(CustomInput.UserInput.LookDown)),
                CustomInput.Bool(CustomInput.UserInput.LookRight) ? CustomInput.Raw(CustomInput.UserInput.LookRight) : CustomInput.Raw(CustomInput.UserInput.LookLeft));

            return (mouse + controller) * this.sensitivity;
        }
    }
}