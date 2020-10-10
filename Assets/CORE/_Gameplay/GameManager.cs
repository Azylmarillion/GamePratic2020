// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GamePratic2020
{
	public class GameManager : MonoBehaviour
    {
		#region Fields / Properties
		public static GameManager Instance = null; 

		[HorizontalLine(1, order = 0), Section("GameManager", order = 1)]
		[SerializeField] private MiniGame[] miniGames = new MiniGame[] { };
		private int currentMGIndex = 0;
		[SerializeField] private int globalScore = 0;
		public int GlobalScore => globalScore;

		[SerializeField] private int currentRunScore = 0;
		public int CurrentRunScore => currentRunScore;

        [SerializeField, Min(1)] private int maxIteration = 3;
        [SerializeField, ReadOnly] private int iteration = 0;

		[HorizontalLine(1, order = 0), Section("Sounds", order = 1)]
		[SerializeField] private SoundDatabase soundDatabase = null;
		public SoundDatabase SoundDataBase => soundDatabase;
		#endregion

		#region Methods
		public void ProceedToNextMG(int _score)
		{
			currentRunScore += _score;
			// ADD TRANSITION HERE
			miniGames[currentMGIndex].HideMiniGame();
			currentMGIndex++;

            if (currentMGIndex >= miniGames.Length)
            {
				CompleteRun();
            }

            miniGames[currentMGIndex].ResetMiniGame(iteration);
            miniGames[currentMGIndex].StartMiniGame(); 
		}

		private void StartRun() {
            currentRunScore = 0;
		}

		private void CompleteRun() {
            currentMGIndex = 0;
            iteration++;

            globalScore += currentRunScore;

            if (iteration == maxIteration) {
                // Do some things.
                Debug.LogError("C'est la fin.");
                return;
            }
        }

		private void Awake()
		{
			if (Instance == null)
				Instance = this;
			else Destroy(this);

            miniGames[0].ResetMiniGame(0);
            miniGames[0].StartMiniGame();
		}
		#endregion
	}
}
