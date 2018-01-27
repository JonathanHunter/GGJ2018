namespace GGJ2018.Managers
{
    using UnityEngine;

    public class GameState : MonoBehaviour
    {
        public enum State { Playing, Paused }

        public State CurrentState { get; set; }

        public float Sensitivity { get; set; }

        public bool Inverted { get; set; }

        public static GameState Instance { get; set; }

        private void Start()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Duplicate GameState detected: removing " + this.gameObject.name);
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);

            Sensitivity = 1f;
        }
    }
}
