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
        #endregion

        #region Current
        private Tween currentTween = null;
        #endregion

        #region Behaviour
        /// <summary>
        /// Play camera shake effect
        /// </summary>
        public void Play() {
            //Must finish the previous tween to play another to not offset the camera
            if(currentTween != null && currentTween.IsPlaying()) {
                currentTween.Complete();
            }

            currentTween = Camera.main.transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut);
        }
        #endregion
    }
}
