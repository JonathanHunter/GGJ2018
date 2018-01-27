namespace GGJ2018.Character.Player
{
    using UnityEngine;
    using Util;

    public class Player : MonoBehaviour
    {
        public Animator anim;
        public Rigidbody rgdb;
        public float moveSpeed;
        public CameraControl cameraControl;

        private int moveHash;
        private int meleeHash;
        private int shootHash;

        private bool Moving
        {
            get
            {
                return CustomInput.BoolHeld(CustomInput.UserInput.Left) ||
                    CustomInput.BoolHeld(CustomInput.UserInput.Right) ||
                    CustomInput.BoolHeld(CustomInput.UserInput.Up) ||
                    CustomInput.BoolHeld(CustomInput.UserInput.Down);
            }
        }

        private void Start()
        {
            this.moveHash = Animator.StringToHash("move");
            this.meleeHash = Animator.StringToHash("melee");
            this.shootHash = Animator.StringToHash("shoot");
        }

        private void Update()
        {
            float yRot = cameraControl.UpdateCamera();
            if (yRot != 0)
            {
                float z = this.transform.rotation.eulerAngles.z;
                this.transform.Rotate(new Vector3(0f, yRot, 0f));
                this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, z);
            }
            
            if (this.Moving)
            {
                Move();
            }

            //UpdateAnimator();
        }

        private void Move()
        {
            int x;
            int y;
            if (CustomInput.BoolHeld(CustomInput.UserInput.Up))
                y = 1;
            else if (CustomInput.BoolHeld(CustomInput.UserInput.Down))
                y = -1;
            else
                y = 0;

            if (CustomInput.BoolHeld(CustomInput.UserInput.Left))
                x = -1;
            else if (CustomInput.BoolHeld(CustomInput.UserInput.Right))
                x = 1;
            else
                x = 0;

            Vector2 dir = new Vector2(x, y);
            this.rgdb.velocity = (this.transform.right * dir.x + this.transform.forward * dir.y) * this.moveSpeed;
        }

        private void UpdateAnimator()
        {
            anim.SetBool(this.moveHash, this.Moving);
        }
    }
}
