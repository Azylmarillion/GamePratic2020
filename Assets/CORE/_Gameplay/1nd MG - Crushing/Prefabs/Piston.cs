using UnityEngine;
using EnhancedEditor;
using GamePratic2020.Tools;

namespace GamePratic2020 {
    public class Piston : MonoBehaviour {
        #region Settings
        [Section("Settings")]
        [SerializeField] private float minHeight = 2f;
        [SerializeField] private float maxHeight = 10f;
        [SerializeField] private int steps = 5;
        [SerializeField] private AnimationCurve heightOverSteps = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [Header("References")]
        [SerializeField] private Transform pistonHeadTransform = null;
        [SerializeField] private CameraShake crushCameraShake = null;
        #endregion

        #region Currents
        private int currentStep = 0;
        #endregion

        #region Callbacks
        private void Start() {
            MovePiston();    
        }

        private void Update() {
            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
                UpdatePistonPos();
            }
        }
        #endregion

        #region Movement
        private void UpdatePistonPos() {
            currentStep++;

            if(currentStep > steps) {
                currentStep = 0;
                Crush();
            }

            MovePiston();

            Debug.Log($"Move to step {currentStep}");
        }

        private void MovePiston() {
            float t = (float)currentStep / (float)steps;
            t = heightOverSteps.Evaluate(t);
            pistonHeadTransform.position = Vector3.Lerp(transform.position + Vector3.up * minHeight, transform.position + Vector3.up * maxHeight, t);
        }

        private void Crush() {
            Debug.Log("Crush");
            crushCameraShake.Play();
        }
        #endregion

        #region Debug
#if UNITY_EDITOR
        private void OnDrawGizmos() {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position + Vector3.up * minHeight, transform.position + Vector3.up * maxHeight);
            Gizmos.DrawWireSphere(transform.position + Vector3.up * minHeight, 0.3f);
            Gizmos.DrawWireSphere(transform.position + Vector3.up * maxHeight, 0.3f);
        }
#endif
        #endregion
    }
}
