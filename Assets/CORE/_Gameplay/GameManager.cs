// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//      • Reset Score --> Reset EndGame
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
        public int Iteration => iteration;
        public int MaxIteriation => maxIteration;

		[HorizontalLine(1, order = 0), Section("Sounds", order = 1)]
		[SerializeField] private SoundDatabase soundDatabase = null;
		public SoundDatabase SoundDataBase => soundDatabase;

		[SerializeField, Required] private AudioSource ambiantSource = null;
        public AudioSource AmbiantSource => ambiantSource;

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

        private bool isWaiting = false;
        private float waitingVar = 0;

        public void StartNextMiniGame()
        {
            currentMGIndex++;
            if (currentMGIndex == miniGames.Length)
            {
                CompleteRun();
                UIManager.ShowEndScreen();
                return;
            }
            else if (iteration == maxIteration)
            {
                uiManager.ShowMainMenu(true);
                return;
            }

            miniGames[currentMGIndex].ResetMiniGame(iteration);

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
                isWaiting = true;
                waitingVar = .5f;
            }
        }

        public void HideMiniGame() => miniGames[currentMGIndex].HideMiniGame();

        public void StartGame()
        {
            UIManager.Instance.ShowMainMenu(false);
            isWaiting = false;
            currentMGIndex = -1;
            globalScore = currentRunScore = iteration = 0;
            StartNextMiniGame();
        }

		private void StartRun() {
            currentRunScore = 0;
		}

        public void ResetScore() => currentRunScore = 0;

        private bool CompleteRun() {
            currentMGIndex = -1;
            iteration++;

            globalScore += currentRunScore;

            if (iteration == maxIteration) {
                // Do some things.
                Debug.Log("C'est la fin.");
                return true;
            }
            return false;
        }

        public void QuitGame() => Application.Quit();

        public void PlayClickSound()
        {
            ambiantSource.PlayOneShot(soundDatabase.ClickClip); 
        }

        public void PlayFootStepSound()
        {
            ambiantSource.PlayOneShot(soundDatabase.GetRandomFootStep(), .25f); 
        }

        private void Awake()
		{
			if (Instance == null)
				Instance = this;
			else Destroy(this);

            uiManager.ShowMainMenu(true);
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

            // Start mini game delay.
            if (isWaiting)
            {
                waitingVar -= Time.deltaTime;
                if (waitingVar < 0)
                {
                    isWaiting = false;
                    miniGames[currentMGIndex].StartMiniGame();
                }
            }
        }
        #endregion
    }
}
