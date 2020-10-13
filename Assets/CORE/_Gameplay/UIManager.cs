// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using DG.Tweening;
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
        [SerializeField, Required] private GameObject pressToPlayScreen = null;

        [HorizontalLine(1)]

        [SerializeField, Required] private RunEndScreen finalScreen = null;
        [SerializeField, Required] private RectTransform endGameScreen = null;
        [SerializeField, Required] private FunFactDatabase database = null;

        [SerializeField, Required] private TextMeshProUGUI funfactText = null;
        [SerializeField, Required] private TextMeshProUGUI endGameText = null;

        [HorizontalLine(1)]

        [SerializeField, Required] private Image timerPanel = null;
        [SerializeField, Required] private TextMeshProUGUI timer = null;
        [SerializeField, Required] private TextMeshProUGUI score = null;

        [HorizontalLine(1)]

        [SerializeField, Required] private Animator progressAnimator = null;

        [HorizontalLine(1)]

        [SerializeField] private float transitionInDuration = .5f;
        [SerializeField] private float transitionOutDuration = .5f;

        [HorizontalLine(1)]

        [SerializeField] private Vector3 finalScreenPos = new Vector3();
        [SerializeField] private float finalTransitionInDuration = .5f;
        [SerializeField] private float finalTransitionOutDuration = .75f;

        private readonly int progress_Hash = Animator.StringToHash("State");
        #endregion

        #region Methods

        #region Menus
        public void ShowMainMenu(bool _isActive) => mainMenu.SetActive(_isActive);

        // ---------

        private bool isShowingEndScreen = false;
        private bool isHidingEndScreen = false;

        private float endScreenVar = 0;

        public void ShowEndScreen()
        {
            isShowingEndScreen = true;
            isHidingEndScreen = false;
            endScreenVar = 0;

            finalScreen.gameObject.SetActive(true);
            finalScreen.CleanDisplay();
        }

        public void HideEndScreen()
        {
            isShowingEndScreen = false;
            isHidingEndScreen = true;
            endScreenVar = 0;

            GameManager.Instance.ResetScore();
            if (GameManager.Instance.Iteration == GameManager.Instance.MaxIteriation)
            {
                isEndTransition = true;
                endTransitionVar = 0;
                endTransitionState = 0;

                endGameScreen.gameObject.SetActive(true);
                endGameScreen.anchoredPosition = Vector3.zero;

                endGameText.text =  "Félicitations !\n\n" +
                                    "Vous avez produit " +
                                    (GameManager.Instance.GlobalScore / 1000).ToString() +
                                    " tonnes de coke pour alimenter une usine sidérurgique locale !";
            }
            else
                GameManager.Instance.StartNextMiniGame();
        }
        #endregion

        #region In Game UI
        public void DisplayGameScore(bool _doDisplay) => score.gameObject.SetActive(_doDisplay);

        public void UpdateTimer(float _timer, float _percent)
        {
            timer.text = _timer.ToString("00");
            timerPanel.fillAmount = _percent;
        }

        Tweener twin = null;

        public void UpdateScore(int _score)
        {
            if ((twin != null) && twin.IsPlaying())
                twin.Complete();

            twin = score.transform.DOPunchScale(new Vector3(1.1f, 1.1f, 1.1f), .5f);
            score.text = _score.ToString("### ### 000");

            //if (_score > 0)
            //    GameManager.Instance.AmbiantSource.PlayOneShot(GameManager.Instance.SoundDataBase.ScoreIncrement);
        }

        public void DisplayPressToPlay(bool _doDisplay) => pressToPlayScreen.SetActive(_doDisplay);

        // ----------

        private bool isInTransition = false;
        private bool isTransitStart = false;
        private bool isTransitIn = false;
        private bool isTransitOut = false;
        private float transitionVar = 0;

        private bool isEndTransition = false;
        private int endTransitionState = 0;
        private float endTransitionVar = 0;

        public void DoMiniGameTransition()
        {
            // Call hide mini game after animation.
            isInTransition = true;
            isTransitStart = true;
            isTransitIn = false;
            isTransitOut = false;
            transitionVar = 0;
            endMiniGame.SetActive(true);
            GameManager.Instance.AmbiantSource.PlayOneShot(GameManager.Instance.SoundDataBase.WinJingle);

            funfactText.text = database.GetRandomFact();
            transitionScreen.anchoredPosition = new Vector2(1250, 0);
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

            // Final screen transitions.
            if (isShowingEndScreen)
            {
                endScreenVar += Time.deltaTime;
                float _lerp = endScreenVar / finalTransitionInDuration;
                if (_lerp > 1)
                {
                    _lerp = 1;
                    isShowingEndScreen = false;

                    finalScreen.PlayAnimation();
                }

                finalScreen.transform.position = Vector3.Lerp(finalScreenPos, Vector3.zero, _lerp);
            }
            else if (isHidingEndScreen)
            {
                endScreenVar += Time.deltaTime;
                float _lerp = endScreenVar / finalTransitionOutDuration;
                if (_lerp > 1)
                {
                    _lerp = 1;
                    isHidingEndScreen = false;

                    if (GameManager.Instance.Iteration < GameManager.Instance.MaxIteriation)
                        finalScreen.gameObject.SetActive(false);
                }

                finalScreen.transform.position = Vector3.Lerp(Vector3.zero, -finalScreenPos, _lerp);
            }

            // End game transition.
            if (isEndTransition)
            {
                endTransitionVar += Time.deltaTime;
                switch (endTransitionState)
                {
                    case 0:
                        if (endTransitionVar > 5)
                        {
                            endTransitionState++;

                            finalScreen.gameObject.SetActive(false);
                            GameManager.Instance.StartNextMiniGame();
                        }
                        break;

                    case 1:
                        if (endTransitionVar < 6)
                        {
                            endGameScreen.anchoredPosition = Vector3.Lerp(Vector3.zero, new Vector3(0, -2000, 0), endTransitionVar - 5);
                        }
                        else
                        {
                            isEndTransition = false;
                            endGameScreen.gameObject.SetActive(false);
                        }
                        break;
                }
            }
        }

        #endregion
    }
}
