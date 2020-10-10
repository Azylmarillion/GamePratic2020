// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GamePratic2020
{
	public class WagonController : MonoBehaviour
    {
        #region Fields / Properties
        [HorizontalLine(1, order = 0), Section("WAGON CONTROLLER", order = 1)]

        [SerializeField, Required] private new Collider2D collider = null;
        [SerializeField, Required] private Transform anchor = null;

        [HorizontalLine(1)]

        [SerializeField] private float maxDistance = 1;
        [SerializeField] private float speed = 5;

        [HorizontalLine(1)]

        [SerializeField] private float maxRotation = 45;
        #endregion

        #region Methods
        private void Update()
        {
            Vector3 _position = transform.position;
            float _target = anchor.transform.position.x;
            float _difference = _target - _position.x;
            if (_difference != 0)
            {
                if (Mathf.Abs(_difference) > maxDistance)
                {
                    _position.x = _target - (maxDistance * Mathf.Sign(_difference));
                }
                else
                {
                    _position.x = Mathf.MoveTowards(_position.x, _target, Time.deltaTime * speed);
                }

                transform.position = _position;
            }
        }
        #endregion
    }
}
