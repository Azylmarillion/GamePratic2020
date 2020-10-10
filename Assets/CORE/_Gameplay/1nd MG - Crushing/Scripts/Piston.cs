using UnityEngine;
using EnhancedEditor;
using GamePratic2020.Tools;
using System.Collections;
using UnityEngine.Events;

namespace GamePratic2020 {
    [SelectionBase]
    public class Piston : MonoBehaviour {
        #region Settings
        [Section("Settings")]
        [SerializeField] private float minHeight = 2f;
        [SerializeField] private float maxHeight = 10f;
        [SerializeField] private int steps = 5;
        [SerializeField] private AnimationCurve heightOverSteps = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [Header("Step")]
        [SerializeField, Min(0f)] private float stepMovementDuration = 0.2f;
        [SerializeField] private AnimationCurve stepMovementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Header("Crush")]
        [SerializeField, Min(0f)] private float crushMovementDuration = 0.2f;
        [SerializeField] private AnimationCurve crushMovementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Header("References")]
        [SerializeField] private Transform pistonHeadTransform = null;
        [SerializeField] private CameraShake stepUpCameraShake = null;
        [SerializeField] private CameraShake crushCameraShake = null;

        [Section("Callbacks")]
        [SerializeField] private UnityEvent onCrush = new UnityEvent();
        #endregion

        #region Currents
        private int currentStep = 0;
        private bool isMoving = false;
        #endregion

        #region Callbacks
        private void OnEnable() {
            ResetPiston();
        } 

        public void ResetPiston() { 
            currentStep = 0;
            isMoving = false;
            MovePiston(0f);
        }

        private void Update() {
            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
                UpdatePistonState();
            }
        }
        #endregion

        #region Movement
        private void UpdatePistonState() {
            currentStep++;

            if(currentStep > steps) {
                currentStep = 0;
                Crush();
            } else {
                if (!isMoving) {
                    StartCoroutine(StepMovementCoroutine());
                }
            }
        }

        private void MovePiston(float _heightRatio) {
            float t = heightOverSteps.Evaluate(_heightRatio);
            pistonHeadTransform.localPosition = Vector3.Lerp(Vector3.up * minHeight, Vector3.up * maxHeight, t);
        }

        private void Crush() {
            if (!isMoving) {
                StartCoroutine(CrushMovementCoroutine());
            }
        }
        #endregion

        #region Coroutines
        private IEnumerator StepMovementCoroutine() {
            stepUpCameraShake.Play();

            isMoving = true;
            float from = ((float)currentStep - 1f) / (float)steps;
            float increment = (1f / (float)steps);

            for (float f = 0; f < 1f; f+= Time.deltaTime / stepMovementDuration) {
                float t = stepMovementCurve.Evaluate(f);
                float height = from + (increment * t);
                MovePiston(height);
                yield return null;
            }

            MovePiston(from + increment);

            isMoving = false;

            yield return null;
        }

        private IEnumerator CrushMovementCoroutine() {
            isMoving = true;

            Vector3 fromPos = pistonHeadTransform.localPosition;

            for (float f = 0; f < 1f; f += Time.deltaTime / crushMovementDuration) {
                float t = crushMovementCurve.Evaluate(f);
                pistonHeadTransform.localPosition = Vector3.Lerp(fromPos, Vector3.zero, t);
                yield return null;
            }

            crushCameraShake.Play();
            Vector3 initialPos = Vector3.up * minHeight;

            for (float f = 0; f < 1f; f+= Time.deltaTime / stepMovementDuration) {
                pistonHeadTransform.localPosition = Vector3.Lerp(Vector3.zero, initialPos, f);
                yield return null;
            }

            isMoving = false;

            yield return null;
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
