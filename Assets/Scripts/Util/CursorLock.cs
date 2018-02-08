namespace GGJ2018.Util
{
    using UnityEngine;

    public class CursorLock : MonoBehaviour
    {
        public bool ForceCursorUnLock;

        private bool shouldLockCursor;

        private void Update()
        {
            if (Managers.GameState.Instance.CurrentState != Managers.GameState.State.Playing || this.ForceCursorUnLock)
                UnLock();
            else
                Lock();
        }

        private void Lock()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void UnLock()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        /// <summary> Locks or unlocks the cursor from the screen. </summary>
        /// <returns> The cursor state. </returns>
        public void DynamicLock()
        {
            if (this.ForceCursorUnLock || Input.GetKey(KeyCode.Escape))
                this.shouldLockCursor = false;
            else if (Input.GetKey(KeyCode.Mouse0))
                this.shouldLockCursor = true;

            if (this.shouldLockCursor && Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!this.shouldLockCursor && !Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
