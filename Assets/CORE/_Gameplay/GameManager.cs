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

        [SerializeField, Required] private UIManager uiManager = null;
        public UIManager UIManager => uiManager;

		[SerializeField] private MiniGame[] miniGames = new MiniGame[] { };
		private int currentMGIndex = 0;
        public int CurrentMGIndex => currentMGIndex;

        [SerializeField] private int globalScore = 0;
		public int GlobalScore => globalScore;

		[SerializeField] private int currentRunScore = 0;
		public int CurrentRunScore => currentRunScore;

        [SerializeField, Min(1)] private int maxIteration = 3;
        [SerializeField, ReadOnly] private int iteration = 0;

		[HorizontalLine(1, order = 0), Section("Sounds", order = 1)]
		[SerializeField] private SoundDatabase soundDatabase = null;
		public SoundDatabase SoundDataBase => soundDatabase;

		[SerializeField, Required] private AudioSource ambiantSource = null;

        [HorizontalLine(1)]

        [SerializeField, ReadOnly] private bool isWaitingToStart = false;
        private int waitingInputAmount = 0;
		#endregion

		#region Methods
		public void ProceedToNextMG(int _score)
		{
			currentRunScore += _score;

            // ADD TRANSITION HERE
            uiManager.DoMiniGameTransition();
		}

        public int GetMiniGameScore() => miniGames[currentMGIndex].Score;

        public void StartNextMiniGame()
        {
            currentMGIndex++;
            if (currentMGIndex >= miniGames.Length)
            {
                if (CompleteRun())
                {
                    // C'est la fin, afficher l'écran de fin.
                    return;
                }
                else
                {
                    // Afficher l'écran de fin d'itération.
                }
            }

            miniGames[currentMGIndex].ResetMiniGame(iteration);
            ambiantSource.clip = soundDatabase.AmbientClips[currentMGIndex];
            ambiantSource.Play();

            if (iteration == 0)
            {
                UIManager.Instance.DisplayPressToPlay(true);
                isWaitingToStart = true;
#if UNITY_EDITOR
                waitingInputAmount = Input.GetMouseButton(0) ? 1 : 0;
#else
            waitingInputAmount = Input.touchCount;
#endif
            }
            else
            {
                miniGames[currentMGIndex].StartMiniGame();
            }
        }

        public void HideMiniGame() => miniGames[currentMGIndex].HideMiniGame();

        private void StartGame()
        {
            currentMGIndex = -1;
            globalScore = currentRunScore = iteration = 0;
            StartNextMiniGame();
        }

		private void StartRun() {
            currentRunScore = 0;
		}

		private bool CompleteRun() {
            currentMGIndex = 0;
            iteration++;

            globalScore += currentRunScore;

            if (iteration == maxIteration) {
                // Do some things.
                Debug.LogError("C'est la fin.");
                return true;
            }
            return false;
        }

		private void Awake()
		{
			if (Instance == null)
				Instance = this;
			else Destroy(this);

            StartGame();
        }

        private void Update()
        {
            // Wait for input before mini game starts.
            if (isWaitingToStart)
            {
                int _touchCount;
#if UNITY_EDITOR
                _touchCount = Input.GetMouseButton(0) ? 1 : 0;
#else
                _touchCount = Input.touchCount;
#endif
                if (waitingInputAmount == 0)
                {
                    if (_touchCount > 0)
                    {
                        isWaitingToStart = false;
                        UIManager.DisplayPressToPlay(false);
                        miniGames[currentMGIndex].StartMiniGame();
                    }
                }
                else
                    waitingInputAmount = _touchCount;
            }
        }
        #endregion
    }
}
