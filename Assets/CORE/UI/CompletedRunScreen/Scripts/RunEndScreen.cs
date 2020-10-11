// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using DG.Tweening;
using EnhancedEditor;
using GamePratic2020.Tools;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GamePratic2020 {
    public class RunEndScreen : MonoBehaviour {
        #region Settings
        [Section("Settings")]
        [SerializeField, Min(0f)] private float textAppearDuration = 0.8f;

        [Header("Fill")]
        [SerializeField] private float pointsPerSeconds = 200;
        [SerializeField] private float fillIntensity = 2f;
        [SerializeField] private float maxFillHeight = 2;
        [SerializeField] private float minimumFillHeight = 0;
        [SerializeField] private float initialFillHeight = 0;
        [SerializeField] private float cokeFallDuration = 0.5f;

        [Header("Screen")]
        [SerializeField] private float endWaitDuration = 0.6f;

        [Header("References")]
        [SerializeField] private Transform stackParent = null;
        [SerializeField] private ParticleSystem fallEffect = null;
        [SerializeField] private ParticleSystem fillEffect = null;
        [SerializeField] private CameraShake fillEndCameraShake = null;
        [SerializeField] private AudioSource cokeFallSource = null;
        [SerializeField] private AudioSource audioSource = null;

        [Header("Text")]
        [SerializeField] private CanvasGroup textCanvasGroup = null;
        [SerializeField] private TextMeshProUGUI totalScoreText = null;
        [SerializeField] private TextMeshProUGUI runScoreText = null;
        [SerializeField] private TextMeshProUGUI goesHarderText = null;
        [SerializeField] private TextMeshProUGUI dayOverText = null;
        [SerializeField] private TextMeshProUGUI daysCounterText = null;
        [SerializeField] private TextMeshProUGUI workOverText = null;
        [SerializeField] private Image fillBar = null;
        [SerializeField] private RectTransform daysPanelGroup = null;
        [SerializeField] private CanvasGroup uiGroup = null;

        [Section("Callbacks")]
        [SerializeField] private UnityEvent onFillComplete = null;
        #endregion

        #region Current
        [Section("Read Only")]
        [SerializeField, ReadOnly] private int remainingPointsToFill = 0;
        private bool hasBeenInitialized = false;
        #endregion

        #region Reset
        [ContextMenu("Reset All")]
        public void ResetAll() {
            hasBeenInitialized = false;
            fillEffect.Clear();
        }
        #endregion

        #region Callbacks
        private void Awake() {
            CleanDisplay();
        }
        #endregion

        #region Filling
        public void PlayAnimation() {
            remainingPointsToFill = GameManager.Instance.CurrentRunScore;
            runScoreText.text = remainingPointsToFill.ToString();
            totalScoreText.text = (GameManager.Instance.GlobalScore - remainingPointsToFill).ToString();

            if (!hasBeenInitialized) {
                fillEffect.transform.position = Vector3.up * initialFillHeight;
                hasBeenInitialized = true;
            } else {
                if (fillEffect.transform.position.y > minimumFillHeight) {
                    stackParent.transform.position += Vector3.up * (minimumFillHeight - (fillEffect.transform.position.y));
                }
            }

            textCanvasGroup.alpha = 0f;

            StartCoroutine(ProcessAnimation());
        }

        public void CleanDisplay() {
            textCanvasGroup.alpha = 0;
            goesHarderText.gameObject.SetActive(false);
            dayOverText.gameObject.SetActive(false);
            workOverText.gameObject.SetActive(false);
            daysPanelGroup.gameObject.SetActive(false);
        }

        private IEnumerator ProcessAnimation() {

            uiGroup.alpha = 1f;

            dayOverText.gameObject.SetActive(true);
            dayOverText.transform.localScale = Vector3.zero;
            dayOverText.transform.DOScale(1f, 0.4f);

            yield return new WaitForSeconds(0.7f);

            textCanvasGroup.DOFade(1f, textAppearDuration);

            Vector3 totalScoreTextPos = totalScoreText.transform.position;
            Vector3 runScoreTextPos = runScoreText.transform.position;

            totalScoreText.transform.position = totalScoreTextPos + Vector3.right * 300f;
            runScoreText.transform.position = runScoreTextPos + Vector3.right * -300f;

            totalScoreText.transform.DOMove(totalScoreTextPos, textAppearDuration, true);
            runScoreText.transform.DOMove(runScoreTextPos, textAppearDuration, true);

            yield return new WaitForSeconds(0.2f);

            yield return ProcessFillCoroutine();
            yield return DaysFillCoroutine();

            yield return new WaitForSeconds(0.4f);

            if (GameManager.Instance.Iteration >= GameManager.Instance.MaxIteriation) {
                workOverText.gameObject.SetActive(true);
                workOverText.transform.localScale = Vector3.zero;
                workOverText.transform.DOScale(1f, 0.3f);
                audioSource.PlayOneShot(GameManager.Instance.SoundDataBase.WinJingle);
            } else {
                goesHarderText.gameObject.SetActive(true);
                goesHarderText.transform.localScale = Vector3.zero;
                goesHarderText.transform.DOScale(1f, 0.3f);
            }

            yield return new WaitForSeconds(endWaitDuration);

            uiGroup.DOFade(0f, 0.3f);
            yield return new WaitForSeconds(0.3f);


            onFillComplete?.Invoke();
        }

        private IEnumerator ProcessFillCoroutine() {
            fallEffect.Play();
            cokeFallSource.Play();
            int globalScore = GameManager.Instance.GlobalScore;

            yield return new WaitForSeconds(cokeFallDuration);

            fillEffect.Play();

            while (remainingPointsToFill > 0) {
                int pointsDecrement = Mathf.RoundToInt(pointsPerSeconds * Time.deltaTime);
                remainingPointsToFill -= pointsDecrement;
                if (remainingPointsToFill < 0) {
                    remainingPointsToFill = 0;
                }

                fillEffect.transform.position += Vector3.up * fillIntensity * Time.deltaTime;

                //Update Text
                runScoreText.text = remainingPointsToFill.ToString();
                totalScoreText.text = (globalScore - remainingPointsToFill).ToString();

                //Move down the stack
                if (fillEffect.transform.position.y > maxFillHeight) {
                    stackParent.transform.position += Vector3.up * (maxFillHeight - (fillEffect.transform.position.y));
                }

                yield return null;
            }

            fillEndCameraShake.Play();
            cokeFallSource.Stop();

            runScoreText.text = "0";
            totalScoreText.text = GameManager.Instance.GlobalScore.ToString();

            fallEffect.Stop();
            fillEffect.Stop();

            yield return null;
        }

        private IEnumerator DaysFillCoroutine() {
            yield return new WaitForSeconds(0.5f);

            daysPanelGroup.gameObject.SetActive(true);
            daysPanelGroup.transform.localScale = Vector3.zero;
            daysPanelGroup.transform.DOScale(1f, 0.4f);

            GameManager gm = GameManager.Instance;

            daysCounterText.text = $"Jour {gm.Iteration}/{gm.MaxIteriation}";

            float fromFillVal = ((float)gm.Iteration - 1)/ (float)gm.MaxIteriation;
            float toFillVal = ((float)gm.Iteration) / (float)gm.MaxIteriation;
            fillBar.fillAmount = fromFillVal;

            yield return new WaitForSeconds(0.4f);

            for (float f = 0; f < 1f; f += Time.deltaTime / 0.6f) {
                float fill = Mathf.SmoothStep(fromFillVal, toFillVal, f);
                fillBar.fillAmount = fill;
                yield return null;
            }

            fillBar.fillAmount = toFillVal;

            yield return null;
        }
        #endregion

        #region Debug
#if UNITY_EDITOR
        private void OnDrawGizmosSelected() {
            Vector3 heightPos = Vector3.up * maxFillHeight;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(heightPos + Vector3.right * 100f, heightPos - Vector3.right * 100f);

            Vector3 initialFillPos = Vector3.up * initialFillHeight;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(initialFillPos + Vector3.right * 100f, initialFillPos - Vector3.right * 100f);

            Vector3 minHeightPos = Vector3.up * minimumFillHeight;
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(minHeightPos + Vector3.right * 100f, minHeightPos - Vector3.right * 100f);
        }
#endif
        #endregion
    }
}
