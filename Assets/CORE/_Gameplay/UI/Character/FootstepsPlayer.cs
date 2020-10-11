// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using UnityEngine;

namespace GamePratic2020
{
	public class FootstepsPlayer : MonoBehaviour
    {
        public void PlayFootstepts() => GameManager.Instance.AmbiantSource.PlayOneShot(GameManager.Instance.SoundDataBase.GetRandomFootStep());
    }
}
