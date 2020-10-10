// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using System.Collections;
using UnityEngine;

namespace GamePratic2020 {
    public class RunEndScreen : MonoBehaviour {
        #region Settings
        [Header("Fill")]
        [SerializeField] private float fillTimePerPoints = 0.5f;
        [SerializeField] private float fillIntensity = 2f;
        [SerializeField] private float maxFillHeight = 150f;
        #endregion

        #region Current
        private int currentFilledPoints = 0;
        #endregion

        #region Filling 

        [ContextMenu("Fill Debug")]
        public void FillDebug() {
            Fill(1000);
        }

        public void Fill(int points) {
            currentFilledPoints = points;
        }

        private IEnumerator ProcessFillCoroutine() {

            while(currentFilledPoints > 0) {
                yield return null;
            }

            yield return null;
        }
        #endregion

        #region Debug
#if UNITY_EDITOR
        private void OnDrawGizmosSelected() {
            Vector3 heightPos = transform.position + Vector3.up * maxFillHeight;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(heightPos + Vector3.right * 100f, heightPos - Vector3.right * 100f);
        }
#endif
        #endregion
    }
}
