// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GamePratic2020
{
	[CreateAssetMenu(fileName ="FunFactDB", menuName = "FunFacts/DB", order = 30)]
	public class FunFactDatabase : ScriptableObject
    {
		#region Fields / Properties
		[SerializeField, TextArea] private string[] funFacts = new string[] { };
		#endregion

		#region Methods
		public string GetRandomFact() => funFacts[Random.Range(0, funFacts.Length)]; 

		#endregion
    }
}
