using EnhancedEditor;
using UnityEngine;


namespace GamePratic2020 {
    public class CrushMiniGame : MiniGame {
        #region Settings
        [Section("Mini Game Settings")]
        [SerializeField] private int collectedCokePoints = 100;
        #endregion

        #region References
        [SerializeField] private Travelator travelator = null;
        #endregion

        #region Minigame Callbacks
        public override void StartMiniGame() {
            base.StartMiniGame();
            travelator.spawn = true;
        }

        public override void StopMiniGame() {
            base.StopMiniGame();
            travelator.spawn = false;
        }

        public override void ResetMiniGame(int _iteration) {
            score = 0;
        }
        #endregion

        #region Score
        public void CollectCoke() {
            score += collectedCokePoints;
        }
        #endregion
    }
}