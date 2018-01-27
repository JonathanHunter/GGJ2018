namespace GGJ2018.Character.Player
{
    using UnityEngine;

    public class AnimRedirect : MonoBehaviour
    {
        public Player player;

        public void StepEvent()
        {
            player.StepEvent();
        }

        public void MeleeDone()
        {
            player.MeleeDone();
        }

        public void ShootDone()
        {
            player.ShootDone();
        }

        public void Tap()
        {
            player.Tap();
        }
    }
}
