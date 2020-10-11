using EnhancedEditor;
using UnityEngine;


namespace GamePratic2020 {
    public class CrushMiniGame : MiniGame {
        #region Settings
        [Section("Mini Game Settings")]
        [SerializeField] private int collectedCokePoints = 100;
        [SerializeField] private AnimationCurve speedScaling = AnimationCurve.Linear(0f, 1f, 1f, 1f);
        #endregion

        #region References
        [SerializeField] private Travelator travelator = null;
        [SerializeField] private Piston piston = null;
        #endregion

        #region Minigame Callbacks
        public override void StartMiniGame() {
            base.StartMiniGame();
            travelator.spawn = true;
            piston.enableInputs = true;
        }

        public override void StopMiniGame() {
            base.StopMiniGame();
            travelator.spawn = false;
            travelator.SetMovementAmount(0f);
            piston.enableInputs = false;
        }

        public override void ResetMiniGame(int _iteration) {
            base.ResetMiniGame(_iteration);
            float ratio = (float)_iteration / (float)GameManager.Instance.MaxIteriation;
            travelator.SetMovementAmount(speedScaling.Evaluate(ratio));
        }
        #endregion

        #region Score
        public void CollectCoke() {
            score += collectedCokePoints;
            UIManager.Instance.UpdateScore(score);
        }
        #endregion
    }
}