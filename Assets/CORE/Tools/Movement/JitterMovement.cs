using UnityEngine;

namespace GamePratic2020 {
    public class JitterMovement : MonoBehaviour {
        #region Settings
        [SerializeField] private float amplitude = 1f;
        [SerializeField] private float frequency = 1f;
        [Space(10f)]
        [SerializeField] private float xOffset = 0;
        [SerializeField] private float yOffset = 5;

        [Space(20f)]
        [SerializeField] private bool playOnStart = false;
        #endregion

        #region Current 
        private Vector3 basePosition;
        private bool playing = false;
        #endregion

        #region Callbacks
        private void Start() {
            if (playOnStart) {
                SetActiveJitter(true);
            }
        }

        public void SetActiveJitter(bool active) {
            if (active) {
                basePosition = transform.position;
            } else {
                transform.position = basePosition;
            }

            playing = active;
        }

        void Update() {
            if (playing) {
                Vector2 newPos = basePosition;

                newPos.x += ((Mathf.PerlinNoise(Time.time * frequency + xOffset, 0) * 2f) -1f) * amplitude;
                newPos.y += ((Mathf.PerlinNoise(0, Time.time * frequency + yOffset) * 2f) -1f) * amplitude;

                transform.position = newPos;
            }
        }
        #endregion
    } 
}
