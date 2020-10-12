using UnityEngine;
using EnhancedEditor;
using GamePratic2020.Tools;
using System.Collections;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

namespace GamePratic2020 {
    [SelectionBase]
    public class Piston : MonoBehaviour {
        #region Settings
        [Section("Settings")]
        public bool enableInputs = true;
        [Space(10f)]
        [SerializeField] private float minHeight = 2f;
        [SerializeField] private float maxHeight = 10f;
        [SerializeField] private int steps = 5;
        [SerializeField] private AnimationCurve heightOverSteps = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [Header("Step")]
        [SerializeField, Min(0f)] private float stepMovementDuration = 0.2f;
        [SerializeField] private AnimationCurve stepMovementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField] private float stepUpSFXMaxPitch = 0.5f;
        [SerializeField] private AudioSource stepUpSFXSource = null;

        [Header("Crush")]
        [SerializeField, Min(0f)] private float crushMovementDuration = 0.2f;
        [SerializeField] private AnimationCurve crushMovementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField] private Color readyToCrushButtonColor = Color.white;

        [Header("References")]
        [SerializeField] private Transform pistonHeadTransform = null;
        [SerializeField] private CameraShake stepUpCameraShake = null;
        [SerializeField] private CameraShake crushCameraShake = null;
        [SerializeField] private Animator pistonAnimator = null;
        [SerializeField] private RectTransform buttonRectTransform = null;
        [SerializeField] private TextMeshProUGUI counterText = null;
        [SerializeField] private Image buttonImage = null;
        [SerializeField] private JitterMovement textJitterMovement = null;

        [Section("Callbacks")]
        [SerializeField] private UnityEvent onCrushBegins = new UnityEvent();
        [SerializeField] private UnityEvent onCrushImpact = new UnityEvent();
        #endregion

        #region Currents
        private int currentStep = 0;
        private bool isMoving = false;

        private Tween buttonTween = null;

        private static readonly int stepUpAnim = Animator.StringToHash("StepUp");
        private static readonly int fallAnim = Animator.StringToHash("Fall");
        private static readonly int crushAnim = Animator.StringToHash("Crush");
        #endregion

        #region Callbacks
        private void OnEnable() {
            ResetPiston();
        } 

        public void ResetPiston() { 
            currentStep = 0;
            isMoving = false;
            MovePiston(0f);
            counterText.text = (steps - currentStep).ToString();
            textJitterMovement.SetActiveJitter(false);
        }

        private void Update() {
            if(enableInputs && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !isMoving) {
                UpdatePistonState();
            }
        }
        #endregion

        #region Movement
        private void UpdatePistonState() {
            currentStep++;

            if (buttonTween != null && buttonTween.IsPlaying()) {
                buttonTween.Complete();
            }

            buttonTween = buttonRectTransform.transform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 20);

            if(currentStep > steps) {
                Crush();
                textJitterMovement.SetActiveJitter(false);
            } else {
                if(currentStep >= steps) {
                    counterText.text = "GO";
                    textJitterMovement.SetActiveJitter(true);
                    buttonImage.color = readyToCrushButtonColor;
                } else {
                    counterText.text = (steps - currentStep).ToString();
                }

                StartCoroutine(StepMovementCoroutine());
            }
        }

        private void MovePiston(float _heightRatio) {
            float t = heightOverSteps.Evaluate(_heightRatio);
            pistonHeadTransform.localPosition = Vector3.Lerp(Vector3.up * minHeight, Vector3.up * maxHeight, t);
        }

        private void Crush() {
            counterText.text = steps.ToString();
            currentStep = 0;
            buttonImage.color = Color.white;
            StartCoroutine(CrushMovementCoroutine());
        }
        #endregion

        #region Coroutines
        private IEnumerator StepMovementCoroutine() {
            stepUpCameraShake.Play();

            stepUpSFXSource.Stop();
            stepUpSFXSource.pitch = 1f + ((float)(currentStep - 1) / ((float)steps - 1));
            stepUpSFXSource.Play();

            pistonAnimator.SetTrigger(stepUpAnim);

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
            onCrushBegins?.Invoke();
            Vector3 fromPos = pistonHeadTransform.localPosition;
            pistonAnimator.SetTrigger(fallAnim);


            for (float f = 0; f < 1f; f += Time.deltaTime / crushMovementDuration) {
                float t = crushMovementCurve.Evaluate(f);
                pistonHeadTransform.localPosition = Vector3.Lerp(fromPos, Vector3.zero, t);
                yield return null;
            }

            pistonAnimator.SetTrigger(crushAnim);

            onCrushImpact?.Invoke();
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
