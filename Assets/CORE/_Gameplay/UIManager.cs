// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using EnhancedEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GamePratic2020
{
	public class UIManager : MonoBehaviour
    {
        #region Fields / Properties
        public static UIManager Instance => GameManager.Instance.UIManager;

        [HorizontalLine(1, order = 0), Section("UI MANAGER", order = 1)]

        [SerializeField, Required] private GameObject mainMenu = null;
        [SerializeField, Required] private GameObject endMiniGame = null;
        [SerializeField, Required] private RectTransform transitionScreen = null;
        [SerializeField, Required] private GameObject finalScreen = null;
        [SerializeField, Required] private GameObject pressToPlayScreen = null;

        [HorizontalLine(1)]

        [SerializeField, Required] private TextMeshProUGUI timer = null;
        [SerializeField, Required] private TextMeshProUGUI score = null;

        [HorizontalLine(1)]

        [SerializeField, Required] private Animator progressAnimator = null;

        [HorizontalLine(1)]

        [SerializeField] private float transitionInDuration = .75f;
        [SerializeField] private float transitionOutDuration = .5f;

        private readonly int progress_Hash = Animator.StringToHash("State");
        #endregion

        #region Methods

        #region Main Menu
        public void ShowMainMenu(bool _isActive) => mainMenu.SetActive(_isActive);
        #endregion

        #region In Game UI
        public void DisplayGameScore(bool _doDisplay) => score.gameObject.SetActive(_doDisplay);

        public void UpdateTimer(float _timer) => timer.text = _timer.ToString("00.00");

        public void DisplayPressToPlay(bool _doDisplay) => pressToPlayScreen.SetActive(_doDisplay);

        // ----------

        private bool isInTransition = false;
        private bool isTransitStart = false;
        private bool isTransitIn = false;
        private bool isTransitOut = false;
        private float transitionVar = 0;

        public void DoMiniGameTransition()
        {
            // Call hide mini game after animation.
            isInTransition = true;
            isTransitStart = true;
            isTransitIn = false;
            isTransitOut = false;
            transitionVar = 0;
            endMiniGame.SetActive(true);
        }

        public void UpdateProgressBar(int _amount) => progressAnimator.SetInteger(progress_Hash, _amount);
        #endregion

        private void Update()
        {
            // UI transitions.
            if (isInTransition)
            {
                transitionVar += Time.deltaTime;
                if (isTransitStart)
                {
                    if (transitionVar > 2.5f)
                    {
                        transitionVar = 0;
                        isTransitStart = false;
                        isTransitIn = true;

                        transitionScreen.gameObject.SetActive(true);
                        UpdateProgressBar(GameManager.Instance.CurrentMGIndex + 1);
                    }
                }
                else if (isTransitIn)
                {
                    float _lerp = transitionVar / transitionInDuration;
                    if (_lerp > 1)
                    {
                        _lerp = 1;
                        isTransitIn = false;
                        transitionVar = 0;

                        endMiniGame.SetActive(false);
                        GameManager.Instance.HideMiniGame();
                    }
                    transitionScreen.anchoredPosition = Vector2.Lerp(new Vector2(1250, 0), Vector2.zero, _lerp);
                }
                else if (transitionVar > 5)
                {
                    if (!isTransitOut)
                    {
                        isTransitOut = true;
                        GameManager.Instance.StartNextMiniGame();
                    }

                    float _lerp = (transitionVar - 5) / transitionInDuration;
                    if (_lerp > 1)
                    {
                        _lerp = 1;
                        isInTransition = false;

                        transitionScreen.gameObject.SetActive(false);
                    }
                    transitionScreen.anchoredPosition = Vector2.Lerp(Vector2.zero, new Vector2(-1250, 0), _lerp);
                }
            }
        }

        #endregion
    }
}