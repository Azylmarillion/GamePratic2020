// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GamePratic2020
{
	public class GameManager : MonoBehaviour
    {
		#region Fields / Properties
		public static GameManager Instance = null; 

		[HorizontalLine(1, order = 0), Section("GameManager", order = 1)]
		[SerializeField] private MiniGame[] miniGames = new MiniGame[] { };
		private int currentMGIndex = 0;
		[SerializeField] private int globalScore = 0; 
		#endregion

		#region Methods
		public void ProceedToNextMG(int _score)
		{
			globalScore += _score;
			// ADD TRANSITION HERE
			miniGames[currentMGIndex].gameObject.SetActive(false);
			currentMGIndex++;
			currentMGIndex = currentMGIndex >= miniGames.Length ? 0 : currentMGIndex;
			miniGames[currentMGIndex].StartMiniGame(); 
		}

		private void Awake()
		{
			if (Instance == null)
				Instance = this;
			else Destroy(this); 
		}
		#endregion
	}
}
