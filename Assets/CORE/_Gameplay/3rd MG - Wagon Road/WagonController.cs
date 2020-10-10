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

        [SerializeField] private Vector2 bounds = new Vector2();
        [SerializeField] private float fallMovement = 5;

        [HorizontalLine(1)]

        [SerializeField] private float trackOneLimit = 5;
        [SerializeField] private float trackTwoLimit = 5;
        [SerializeField] private float trackThreeLimit = 5;

        [HorizontalLine(1)]

        [SerializeField] private float trackTwoHeight = 5;
        [SerializeField] private float trackThreeHeight = 5;

        [HorizontalLine(2, SuperColor.Crimson)]

        [SerializeField] private float radius = 2;
        [SerializeField] private float maxDistance = 1;
        [SerializeField] private float speed = 5;
        [SerializeField] private float speedMovement = 1;
        [SerializeField] private float speedDecreaseCoef = 1;

        [HorizontalLine(1)]

        [SerializeField] private float minDistance = .02f;
        [SerializeField] private float movementSensibility = 1;
        [SerializeField] private float speedFall = 1;
        [SerializeField] private float rotationSpeed = .1f;

        [HorizontalLine(1)]

        [SerializeField, ReadOnly] private float speedVar = 0;
        [SerializeField, ReadOnly] private Vector3 previousAnchor = new Vector3();

        [HorizontalLine(1)]

        [SerializeField, ReadOnly] private bool isFalling = false;
        [SerializeField, ReadOnly] private int railIndex = 1;
        [SerializeField, ReadOnly] private float fallMovementVar = 0;

        [HorizontalLine(1)]

        [SerializeField, ReadOnly] private bool isMoving = false;
        [SerializeField, ReadOnly] private int direction = 0;

        [HorizontalLine(1)]

        [SerializeField, ReadOnly] private float speedStartFall = 0;
        #endregion

        #region Methods
        private void Update()
        {
            Vector3 _anchorPosition = anchor.transform.position;
            _anchorPosition.x = Mathf.Clamp(_anchorPosition.x, bounds.x, bounds.y);
            switch (railIndex)
            {
                case 1:
                    if (_anchorPosition.x > trackOneLimit)
                    {
                        isFalling = true;
                        speedStartFall = speedVar;
                        railIndex++;
                    }
                    break;

                case 2:
                    if (_anchorPosition.x < trackTwoLimit)
                    {
                        isFalling = true;
                        speedStartFall = speedVar;
                        railIndex++;
                    }
                    break;

                case 3:
                    _anchorPosition.x = Mathf.Min(_anchorPosition.x, trackThreeLimit);
                    break;
            }

            if (isFalling)
            {
                float _height = railIndex == 2 ? trackTwoHeight : trackThreeHeight;
                fallMovementVar += Time.deltaTime * fallMovement;

                _anchorPosition.y -= fallMovementVar * Time.deltaTime;
                if (_anchorPosition.y < _height)
                {
                    _anchorPosition.y = _height;
                    fallMovementVar = 0;
                    isFalling = false;
                }
            }

            anchor.transform.position = _anchorPosition;
            Vector3 _movement = _anchorPosition - previousAnchor;
            previousAnchor = _anchorPosition;

            Vector3 _position = transform.position;
            float _difference = _anchorPosition.x - _position.x;
            if (Mathf.Abs(_difference) > maxDistance)
                _difference = maxDistance * Mathf.Sign(_difference);

            // Get movement inertia.
            if (_movement.x != 0)
            {
                if (!isMoving)
                {
                    isMoving = true;
                    direction = _movement.x > 0 ? -1 : 1;

                    speedVar = Mathf.MoveTowards(speedVar, speed, Time.deltaTime * speedMovement * movementSensibility);
                }
                else if (direction != Mathf.Sign(_movement.x))
                {
                    speedVar = Mathf.MoveTowards(speedVar, speed, Time.deltaTime * speedMovement * movementSensibility);
                }
                else
                {
                    speedVar = Mathf.MoveTowards(speedVar, 0, Time.deltaTime * speedMovement * speedDecreaseCoef);
                }
            }

            // Move around anchor.
            if (isMoving)
            {
                _position.x = _anchorPosition.x - _difference;
                _position.x = Mathf.MoveTowards(_position.x, _anchorPosition.x + (maxDistance * direction), Time.deltaTime * speedVar);
                _difference = _anchorPosition.x - _position.x;

                // Falling.
                if (isFalling)
                {
                    speedVar = Mathf.MoveTowards(speedVar, speed, Time.deltaTime * speedFall * speedStartFall);
                }
                // Straight movement
                else
                {
                    if (Mathf.Abs(_anchorPosition.x + (maxDistance * direction) - _position.x) < minDistance)
                        direction *= -1;

                    if (Mathf.Sign(_difference) == direction)
                    {
                        if (Mathf.Abs(_anchorPosition.x + (maxDistance * direction) - _position.x) > minDistance)
                            speedVar = Mathf.MoveTowards(speedVar, speed, Time.deltaTime * speedMovement);
                    }
                    else
                    {
                        speedVar = Mathf.MoveTowards(speedVar, 0, Time.deltaTime * speedMovement * speedDecreaseCoef);
                        if (speedVar == 0)
                        {
                            if (Mathf.Abs(_difference) > minDistance)
                            {
                                direction *= -1;
                            }
                            else
                            {
                                isMoving = false;
                            }
                        }
                    }
                }

                // Move object.
                float _pos = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(_difference, 2));
                _position.y = _anchorPosition.y - _pos;

                transform.position = _position;
                transform.rotation = Quaternion.LookRotation(Vector3.forward, _anchorPosition - _position);
            }
        }
        #endregion
    }
}
