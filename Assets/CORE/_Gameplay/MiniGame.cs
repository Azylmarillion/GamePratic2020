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
		[SerializeField] protected float timer = 10.0f;
		[SerializeField] protected int score = 0;
		[SerializeField, ReadOnly] protected bool isActivated = false;
		public bool IsActivated => isActivated; 
		#endregion

		#region Methods
		public abstract void ResetMiniGame(int _iteration);

		public virtual void StartMiniGame() 
		{
			gameObject.SetActive(true); 
			isActivated = true; 
		}

		public virtual void StopMiniGame()
		{
			isActivated = false; 			
			GameManager.Instance.ProceedToNextMG(score); 
		}

		protected virtual void Update()
		{
			if(isActivated)
			{
				timer -= Time.deltaTime;
				if (timer < 0)
					StopMiniGame(); 
			}
		}
		#endregion
	}
}
