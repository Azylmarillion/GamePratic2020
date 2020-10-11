using UnityEngine;
using DG.Tweening;

namespace GamePratic2020 {
	/// <summary>
	/// This class is called on runtime begins
	/// Userd to setup base game settings and behaviours like default framerate
	/// </summary>
	public class GameInitializer {
		[RuntimeInitializeOnLoadMethod]
		private static void InitializeGame() {
			Application.targetFrameRate = 60;
			DOTween.Init(true, true, LogBehaviour.ErrorsOnly).SetCapacity(200, 50);
		}
	}
}