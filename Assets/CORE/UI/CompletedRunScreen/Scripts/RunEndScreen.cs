// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using DG.Tweening;
using EnhancedEditor;
using System.Collections;
using UnityEngine;

namespace GamePratic2020 {
    public class RunEndScreen : MonoBehaviour {
        #region Settings
        [Section("Settings")]
        [SerializeField] private float pointsPerSeconds = 200;
        [SerializeField] private float fillIntensity = 2f;
        [SerializeField] private float maxFillHeight = 2;
        [SerializeField] private float minimumFillHeight = 0;
        [SerializeField] private float initialFillHeight = 0;
        [SerializeField] private float cokeFallDuration = 0.5f;

        [Header("References")]
        [SerializeField] private Transform stackParent = null;
        [SerializeField] private ParticleSystem fallEffect = null;
        [SerializeField] private ParticleSystem fillEffect = null;

        //[Header("Text")]

        #endregion

        #region Current
        [Section("Read Only")]
        [SerializeField, ReadOnly] private int currentFilledPoints = 0;
        [SerializeField] private bool hasBeenInitialized = false; 
        #endregion

        #region Filling 
        public void Fill(int points) {
            currentFilledPoints = points;

            if (!hasBeenInitialized) {
                fillEffect.transform.position = Vector3.up * initialFillHeight;
                hasBeenInitialized = true;
            } else {
                if(fillEffect.transform.position.y > minimumFillHeight) {
                    stackParent.transform.position += Vector3.up * (minimumFillHeight - (fillEffect.transform.position.y));
                }
            }

            StartCoroutine(ProcessFillCoroutine());
        }

        private IEnumerator ProcessFillCoroutine() {
            fallEffect.Play();

            yield return new WaitForSeconds(cokeFallDuration);

            fillEffect.Play();

            while(currentFilledPoints > 0) {
                int pointsDecrement = Mathf.RoundToInt(pointsPerSeconds * Time.deltaTime);
                currentFilledPoints -= pointsDecrement;

                fillEffect.transform.position += Vector3.up * fillIntensity * Time.deltaTime;

                if(fillEffect.transform.position.y > maxFillHeight) {
                    stackParent.transform.position += Vector3.up * (maxFillHeight - (fillEffect.transform.position.y));
                }

                yield return null;
            }

            fallEffect.Stop();
            fillEffect.Stop();

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
