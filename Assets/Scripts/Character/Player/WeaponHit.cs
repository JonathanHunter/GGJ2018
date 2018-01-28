namespace GGJ2018.Character.Player
{
    using UnityEngine;
    using ObjectPooling;

    public class WeaponHit : MonoBehaviour
    {
        public Transform tip;

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag != "Player")
            {
                GameObject s = SonarPool.Instance.GetSonar(5f, 5f);
                s.transform.position = this.tip.position;
            }
        }
    }
}
