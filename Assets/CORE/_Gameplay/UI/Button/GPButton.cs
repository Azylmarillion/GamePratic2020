// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using EnhancedEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

namespace GamePratic2020
{
	public class GPButton : Button
    {
		#region Fields / Properties
		#endregion

		#region Methods
		public override void OnPointerClick(PointerEventData eventData)
		{
			base.OnPointerClick(eventData);
			GameManager.Instance.PlayClickSound(); 
		}

		protected override void Start()
		{
			base.Start();
			source = GetComponent<AudioSource>(); 
		}
		#endregion
	}
}
