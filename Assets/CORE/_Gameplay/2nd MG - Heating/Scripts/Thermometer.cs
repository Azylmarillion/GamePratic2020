// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GamePratic2020
{ 
    public class Thermometer : MonoBehaviour
    {
        [HorizontalLine(1, order = 0), Section("Thermometer", order = 1)]
        [SerializeField] private float initialRatio = .5f;
        [SerializeField, MinMax(0.0f, 1.0f)] private Vector2 heatingLimit = new Vector2(.25f, .75f);
        [SerializeField, Range(.1f, 1.0f)] private float decreasingMultiplier = 1.0f;
        [SerializeField, Range(.1f, 1.0f)] private float increasingMultiplier = 1.0f;
        [SerializeField, ReadOnly] private float currentRatio;

        public void IncreaseRatio(float _increasingValue)
        {
            currentRatio = Mathf.Clamp(currentRatio + _increasingValue * increasingMultiplier, 0, 1);
            if(currentRatio > heatingLimit.y)
            {
                Debug.Log("Loose"); 
            }
        }

        private void ResetRatio() => currentRatio = initialRatio;    
    
        private void Start() => ResetRatio();

        private void Update()
        {
            if (currentRatio < heatingLimit.x)
            {
                Debug.Log("Loose"); 
                return;
            }
            currentRatio -= (Time.deltaTime * decreasingMultiplier);
            currentRatio = Mathf.Clamp(currentRatio, 0, 1); 
        }
    }
}