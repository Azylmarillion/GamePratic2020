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
	[RequireComponent(typeof(AudioSource))] 
	public class GPButton : Button
    {
		#region Fields / Properties
		[HorizontalLine(1, order = 0), Section("GPButton", order = 1)]
		[SerializeField, Required] private AudioSource source; 
		#endregion

		#region Methods
		public override void OnPointerClick(PointerEventData eventData)
		{
			base.OnPointerClick(eventData);
			//source.PlayOneShot()
		}

		protected override void Start()
		{
			base.Start();
			source = GetComponent<AudioSource>(); 
		}
		#endregion
	}
}
