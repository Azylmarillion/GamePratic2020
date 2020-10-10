// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GamePratic2020
{ 
    public class Thermometer : MiniGame
    {
        #region Fields and Properties
        [HorizontalLine(1, order = 0), Section("Thermometer", order = 1)]
        [SerializeField] private float initialValue = .5f;
        [SerializeField, MinMax(0.0f, 1.0f)] private Vector2 heatingLimit = new Vector2(.25f, .75f);
        [Tooltip("This value is used to slow the decrase of the value. The greater this value is, the slower the decreasing will be.")]
        [SerializeField, Range(1.0f, 100.0f)] private float decreasingRatio = 1.0f;
        [Tooltip("This value is used to slow the increase of the value. The greater this value is, the slower the increasing will be.")]
        [SerializeField, Range(1.0f, 100.0f)] private float increasingRatio = 1.0f;

        [HorizontalLine(1, order = 0), Section("Points", order = 1)]
        [SerializeField, Range(1,10)] private int increasingScoreValue = 1;
        [SerializeField, Range(.1f, 10.0f)] private float decreasingScoreTime = 1.0f;
        private float scoreTimer = 0; 

        [HorizontalLine(1, order = 0), Section("Read values", order = 1)]
        [SerializeField, ReadOnly] private float currentValue;
        private float targetValue = 0; 
        [SerializeField, ReadOnly] private bool hasBeenInitialized = false;
        public bool HasBeenInitialized => hasBeenInitialized;

        [HorizontalLine(1, order = 0), Section("UI", order = 1)]
        [SerializeField] private UnityEngine.UI.Image filledImage = null;
        [SerializeField] private Gradient gradientColor = new Gradient(); 
        #endregion

        #region Methods
        public void IncreaseRatio(float _increasingValue)
        {
            if (!isActivated) return;
            targetValue += _increasingValue / increasingRatio;
            targetValue = Mathf.Clamp(targetValue, 0, 1);
        }

        #region MiniGame
        public override void ResetMiniGame(int _iteration)
        {
            scoreTimer = 0; 
            score = 0; 
            currentValue = initialValue;
            targetValue = currentValue; 
            hasBeenInitialized = false; 
        }

        public override void StartMiniGame()
        {
            base.StartMiniGame();
            hasBeenInitialized = true; 
        }

        public override void StopMiniGame()
        {
            base.StopMiniGame();
        }
        #endregion

        #region Unity
        private void Start() => ResetMiniGame(0);


        protected override void Update()
        {
            base.Update(); 
            if (isActivated)
            {
                targetValue -= (Time.deltaTime / decreasingRatio);
                targetValue = Mathf.Clamp(targetValue, 0, 1);

                currentValue = Mathf.MoveTowards(currentValue, targetValue, Time.deltaTime); 
                filledImage.fillAmount = currentValue;
                filledImage.color = gradientColor.Evaluate(currentValue);

                if (currentValue >= heatingLimit.x && currentValue <= heatingLimit.y)
                {
                    // Decrease Score
                    scoreTimer += Time.deltaTime; 
                    if(scoreTimer > decreasingScoreTime)
                    {
                        score += increasingScoreValue;
                        scoreTimer = 0;
                    }
                }
                else 
                    scoreTimer = 0;
            }
        }
        #endregion

        #endregion
    }
}