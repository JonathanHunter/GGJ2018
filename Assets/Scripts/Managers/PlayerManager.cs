namespace GGJ2018.Managers
{
    using UnityEngine;

    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject playerPrefab;

        public Transform player;

        public static PlayerManager Instance { get; set; }

        public void Init()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Duplicate PlayerManager detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            GameObject player = Instantiate(playerPrefab);
            player.transform.position = this.transform.position;
            this.player = player.transform;
        }
    }
}
