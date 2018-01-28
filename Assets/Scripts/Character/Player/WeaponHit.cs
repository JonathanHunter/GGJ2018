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
                GameObject s = SonarPool.Instance.GetSonar(1.5f, 1.5f);
                if(s != null)
                    s.transform.position = this.tip.position;
            }
        }
    }
}
