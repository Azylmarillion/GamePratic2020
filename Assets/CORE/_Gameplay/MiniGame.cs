// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GamePratic2020
{
	public abstract class MiniGame : MonoBehaviour
    {
        #region Fields / Properties
        [HorizontalLine(1, order = 0), Section("MiniGame", order = 1)]
        [SerializeField] protected float[] timer = new float[] { 10f, 10f, 10f };
		[SerializeField] protected int score = 0;

        [SerializeField, ReadOnly] protected float timerVar = 0;
        [SerializeField, ReadOnly] protected bool isActivated = false;
		public bool IsActivated => isActivated; 
		#endregion

		#region Methods
		public virtual void ResetMiniGame(int _iteration)
        {
            timerVar = timer[_iteration];
            score = 0;
        }

        public virtual void StartMiniGame() 
		{
			gameObject.SetActive(true); 
			isActivated = true; 
		}

		public virtual void StopMiniGame()
		{
			isActivated = false;

            // Delay before next mini game.
			GameManager.Instance.ProceedToNextMG(score);
		}

        public virtual void HideMiniGame()
        {
            gameObject.SetActive(false);
        }

		protected virtual void Update()
		{
			if (isActivated)
			{
				timerVar -= Time.deltaTime;
				if (timerVar < 0)
					StopMiniGame(); 
			}
		}
		#endregion
	}
}
