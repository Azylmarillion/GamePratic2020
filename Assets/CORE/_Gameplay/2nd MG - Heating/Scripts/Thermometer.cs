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
        [HorizontalLine(1, order = 0), Section("Thermometer", order = 1)]
        [SerializeField] private float initialValue = .5f;
        [SerializeField, MinMax(0.0f, 1.0f)] private Vector2 heatingLimit = new Vector2(.25f, .75f);
        [Tooltip("This value is used to slow the decrase of the value. The greater this value is, the slower the decreasing will be.")]
        [SerializeField, Range(1.0f, 100.0f)] private float decreasingRatio = 1.0f;
        [Tooltip("This value is used to slow the increase of the value. The greater this value is, the slower the increasing will be.")]
        [SerializeField, Range(1.0f, 100.0f)] private float increasingRatio = 1.0f;

        [HorizontalLine(1, order = 0), Section("Read values", order = 1)]
        [SerializeField, ReadOnly] private float currentValue;
        [SerializeField, ReadOnly] private bool gameIsOver = false; 

        public void IncreaseRatio(float _increasingValue)
        {
            if (gameIsOver) return; 
            currentValue = Mathf.Clamp(currentValue + _increasingValue / increasingRatio, 0, 1);
            if(currentValue > heatingLimit.y)
            {
                gameIsOver = true; 
            }
        }

        private void ResetRatio()
        {
            currentValue = initialValue;
            gameIsOver = false; 
        }
    
        private void Start() => ResetRatio();

        private void Update()
        {
            if (gameIsOver) return; 
            if (currentValue < heatingLimit.x)
            {
                gameIsOver = true; 
                return;
            }
            currentValue -= (Time.deltaTime / decreasingRatio);
            currentValue = Mathf.Clamp(currentValue, 0, 1); 
        }
    }
}