﻿// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
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
        [SerializeField, MinMax(0.0f, 1.0f)] private Vector2[] heatingLimits = new Vector2[3];
        [SerializeField] private Gradient[] gradientColors = new Gradient[3];
        [SerializeField] private ParticleSystem smokeSystem = null;
        private ParticleSystem.EmissionModule emission;
        private ParticleSystem.MainModule main;


        [Tooltip("This value is used to fasten the decrease of the value.")]
        [SerializeField, Range(.1f, 10.0f)] private float decreasingMultiplier = 1.0f;
        [Tooltip("This value is used to fasten the increase of the value.")]
        [SerializeField, Range(.1f, 10.0f)] private float increasingMultiplier = 1.0f;

        [HorizontalLine(1, order = 0), Section("Points", order = 1)]
        [SerializeField, Range(1,10000)] private int increasingScoreValue = 1;
        [SerializeField, Range(.1f, 10.0f)] private float increasingScoreTime = 1.0f;
        private float scoreTimer = 0; 

        [HorizontalLine(1, order = 0), Section("Read values", order = 1)]
        [SerializeField, ReadOnly] private float currentValue;
        [SerializeField, ReadOnly] private Vector2 heatingLimit = new Vector2(.25f, .75f);
        [SerializeField, ReadOnly] private Gradient currentGradientColor = new Gradient();
        private float targetValue = 0; 

        [HorizontalLine(1, order = 0), Section("UI", order = 1)]
        [SerializeField] private UnityEngine.UI.Image filledImage = null;
        [SerializeField] private RectTransform topCursorTransform;
        [SerializeField] private RectTransform botCursorTransform;
        [SerializeField] private GameObject warningIcon;

        private bool isOutOfLimits = false; 
        #endregion

        #region Methods
        public void IncreaseRatio(float _increasingValue)
        {
            if (!isActivated) return;
            targetValue += _increasingValue * increasingMultiplier;
            targetValue = Mathf.Clamp(targetValue, 0, 1);
        }

        #region MiniGame
        public override void ResetMiniGame(int _iteration)
        {
            base.ResetMiniGame(_iteration);
            heatingLimit = heatingLimits[_iteration];
            topCursorTransform.anchoredPosition = new Vector3(topCursorTransform.anchoredPosition.x, filledImage.rectTransform.rect.height * heatingLimit.y);
            botCursorTransform.anchoredPosition = new Vector3(botCursorTransform.anchoredPosition.x, filledImage.rectTransform.rect.height * heatingLimit.x);

            scoreTimer = 0; 
            currentValue = .5f;
            targetValue = currentValue;
            filledImage.fillAmount = currentValue;
            currentGradientColor = gradientColors[_iteration]; 
            filledImage.color = currentGradientColor.Evaluate(currentValue);

            emission = smokeSystem.emission;
            emission.rateOverTime = 40 * currentValue;

            main = smokeSystem.main;
            main.startLifetime = 1.5f * currentValue; 
        }

        public override void StartMiniGame()
        {
            base.StartMiniGame();
        }

        public override void StopMiniGame()
        {
            miniGameSource.PlayOneShot(GameManager.Instance.SoundDataBase.EndAlarm);
            base.StopMiniGame();
        }
        #endregion

        #region Unity
        protected override void Update()
        {
            base.Update(); 
            if (isActivated)
            {
                targetValue -= (Time.deltaTime * decreasingMultiplier);
                targetValue = Mathf.Clamp(targetValue, 0, 1);

                currentValue = Mathf.MoveTowards(currentValue, targetValue, Time.deltaTime); 
                filledImage.fillAmount = currentValue;
                filledImage.color = currentGradientColor.Evaluate(currentValue);
                emission.rateOverTime = 40 * Mathf.Clamp(currentValue, .1f, 1.0f);
                main.startLifetime = 1.5f * Mathf.Clamp(currentValue, .25f, 1.0f);

                if (currentValue >= heatingLimit.x && currentValue <= heatingLimit.y)
                {
                    if (isOutOfLimits)
                    {
                        isOutOfLimits = false;
                        warningIcon.SetActive(false);
                    }
                    // Decrease Score
                    scoreTimer += Time.deltaTime; 
                    if(scoreTimer > increasingScoreTime)
                    {
                        score += increasingScoreValue;
                        scoreTimer = 0;

                        UIManager.Instance.UpdateScore(score);
                    }
                }
                else
                {
                    if(!isOutOfLimits)
                    {
                        isOutOfLimits = true;
                        scoreTimer = 0;
                        warningIcon.SetActive(true);

                        miniGameSource.PlayOneShot(GameManager.Instance.SoundDataBase.GaugeAlarm, .5f);
                    }

                }
            }
        }
        #endregion

        #endregion
    }
}