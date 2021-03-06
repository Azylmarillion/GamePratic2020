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
        public int Score => score;

        [SerializeField, ReadOnly] protected float timerVar = 0;
        [SerializeField, ReadOnly] protected bool isActivated = false;
		public bool IsActivated => isActivated;

		[HorizontalLine(1, order = 0), Section("Sound", order = 1)]
		[SerializeField, Required] protected AudioSource miniGameSource = null;
		#endregion

		#region Methods
		public virtual void ResetMiniGame(int _iteration)
        {
            timerVar = timer[_iteration];
            gameObject.SetActive(true);
            score = 0;

            UIManager.Instance.UpdateTimer(timerVar, 0);
            UIManager.Instance.UpdateScore(score);
        }

        public virtual void StartMiniGame() 
		{
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
                {
                    timerVar = 0;
                    StopMiniGame();
                }

                UIManager.Instance.UpdateTimer(timerVar, 1 - (timerVar / timer[GameManager.Instance.Iteration]));
            }
		}
		#endregion
	}
}
