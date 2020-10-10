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
		[SerializeField] private float initalTimer = 10.0f;
		[SerializeField] private int score = 100;
		#endregion

		#region Methods
		public virtual void ResetMiniGame(int _iteration)
		{

		}

		public virtual void StartMiniGame() 
		{ 
		}

		public virtual void StopMiniGame()
		{ 
			// GameManager.GoToNextMiniGame(score); 
		}
		#endregion
    }
}
