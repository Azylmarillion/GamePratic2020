using UnityEngine;
using DG.Tweening;

namespace GamePratic2020.Tools {
	[CreateAssetMenu(fileName = "NewCameraShake", menuName = "Utils/Tweening/Camera Shake", order = 0)]
	public class CameraShake : ScriptableObject {
        #region Settings
        [SerializeField, Min(0f)] private float duration = 1f;
        [SerializeField, Min(0f)] private float strength = 1f;
        [SerializeField, Min(0f)] private int vibrato = 10;
        [SerializeField, Min(0f)] private float randomness = 90f;
        [SerializeField] private bool snapping = false;
        [SerializeField] private bool fadeOut = true;
        [SerializeField, Min(0)] private int priority = 0;
        #endregion

        #region Current
        private Tween currentTween = null;
        private static int currentTweenPriority = -1;
        #endregion

        #region Initialize
        [RuntimeInitializeOnLoadMethod]
        private void Reset() {
            currentTweenPriority = -1;
        }
        #endregion

        #region Behaviour
        /// <summary>
        /// Play camera shake effect
        /// </summary>
        public void Play() {
            if (priority < currentTweenPriority) return;

            //Must finish the previous tween to play another to not offset the camera
            if(currentTween != null && currentTween.IsPlaying()) {
                currentTween.Complete();
            }

            currentTweenPriority = priority;
            currentTween = Camera.main.transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut).OnComplete(() => currentTweenPriority = -1);
        }
        #endregion
    }
}
