using UnityEngine;

namespace GamePratic2020 {
	/// <summary>
	/// This class is called on runtime begins
	/// Userd to setup base game settings and behaviours like default framerate
	/// </summary>
	public class GameInitializer {
		[RuntimeInitializeOnLoadMethod]
		private static void InitializeGame() {
			Debug.Log("Initialize game");
			Application.targetFrameRate = 60;
		}
	}
}