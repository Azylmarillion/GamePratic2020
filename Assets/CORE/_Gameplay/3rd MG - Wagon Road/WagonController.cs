// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GamePratic2020
{
	public class WagonController : MiniGame
    {
        #region Fields / Properties
        private const float MIN_DISTANCE = .02f;

        // -----------------------

        [HorizontalLine(1, order = 0), Section("WAGON CONTROLLER", order = 1)]

        [SerializeField, Required] private new Camera camera = null;
        [SerializeField, Required] private Transform anchor = null;
        [SerializeField, Required] private Collider2D controlArea = null;
        [SerializeField, Required] private Transform controlStick = null;

        [HorizontalLine(1)]

        [SerializeField] private float controlSpeed = 5;

        [HorizontalLine(1)]

        [SerializeField, MinMax(-3, 3)] private Vector2 controlBounds = new Vector2();
        [SerializeField, MinMax(-3, 3)] private Vector2 deadZone = new Vector2();

        [SerializeField, MinMax(-3, 3)] private Vector2 bounds = new Vector2();

        [HorizontalLine(1)]

        [SerializeField] private float trackOneLimit = 5;
        [SerializeField] private float trackTwoLimit = 5;
        [SerializeField] private float trackThreeLimit = 5;

        [HorizontalLine(1)]

        [SerializeField] private float trackTwoHeight = 5;
        [SerializeField] private float trackThreeHeight = 5;

        [HorizontalLine(2, SuperColor.Crimson)]

        [SerializeField] private float distanceMagnitude = 2;
        [SerializeField] private float horizontalDistance = 1.75f;

        [HorizontalLine(1)]

        [SerializeField] private float maxSpeed = 5;

        [SerializeField] private float speedMovementIncrease = 1;
        [SerializeField] private float speedFallIncrease = 1;
        [SerializeField] private float speedIncrease = 1;
        [SerializeField] private float speedDecrease = 1;
        [SerializeField] private float fallSpeed = 5;

        [HorizontalLine(1)]

        [SerializeField, ReadOnly] private bool isMoving = false;
        [SerializeField, ReadOnly] private bool isFalling = false;
        [SerializeField, ReadOnly] private int railIndex = 1;
        [SerializeField, ReadOnly] private int direction = 0;

        [HorizontalLine(1)]

        [SerializeField, ReadOnly] private float speedVar = 0;
        [SerializeField, ReadOnly] private float fallSpeedVar = 0;
        [SerializeField, ReadOnly] private float startFallSpeed = 0;

        [HorizontalLine(1)]

        [SerializeField, ReadOnly] private bool isTouch = false;
        [SerializeField, ReadOnly] private Vector3 previousAnchor = new Vector3();

        // -----------------------

        private Vector3 originalPosition = new Vector3();
        #endregion

        #region Methods

        #region Mini Game
        public override void StartMiniGame()
        {
            base.StartMiniGame();
        }

        public override void StopMiniGame()
        {
            base.StopMiniGame();
        }

        // -----------------------

        public override void ResetMiniGame(int _iteration)
        {
            base.ResetMiniGame(_iteration);

            anchor.position = previousAnchor = originalPosition;
            transform.position = new Vector3(originalPosition.x, originalPosition.y - distanceMagnitude, originalPosition.z);
            transform.rotation = Quaternion.identity;

            railIndex = 1;
            isTouch = isMoving = isFalling = false;
            speedVar = fallSpeedVar = startFallSpeed;
        }
        #endregion

        #region Monobehaviour
        private void Awake()
        {
            if (!camera)
                camera = Camera.main;

            originalPosition = anchor.position;
        }

        private void OnEnable()
        {
            ResetMiniGame(0);
        }

        private void Update()
        {
            // Get anchor position.
            Vector3 _anchorPosition = anchor.transform.position;

            // Move the wagon on screen touch.
#if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                Vector2 _contact = camera.ScreenToWorldPoint(Input.mousePosition);
                if (isTouch)
                {
                    Vector3 _controlPos = controlStick.position;
                    _controlPos.x = Mathf.Clamp(_contact.x, controlBounds.x, controlBounds.y);
                    controlStick.position = _controlPos;

                    // Move wagon anchor if touch point outside dead zone.
                    if ((_contact.x < deadZone.x) || (_contact.x > deadZone.y))
                        _anchorPosition.x += ((_controlPos.x - deadZone.y * Mathf.Sign(_controlPos.x)) / (controlBounds.y - deadZone.y)) * Time.deltaTime * controlSpeed;
                }
                else if (controlArea.OverlapPoint(_contact))
                {
                    isTouch = true;
                }
            }
            else if (isTouch)
            {
                isTouch = false;
                controlStick.localPosition = Vector3.zero;
            }
#endif

            // ----------

            // Falling state and horizontal position clamp.
            _anchorPosition.x = Mathf.Clamp(_anchorPosition.x, bounds.x, bounds.y);
            switch (railIndex)
            {
                case 1:
                    if (_anchorPosition.x > trackOneLimit)
                    {
                        isFalling = true;
                        startFallSpeed = speedVar;
                        railIndex++;
                    }
                    break;

                case 2:
                    if (_anchorPosition.x < trackTwoLimit)
                    {
                        isFalling = true;
                        startFallSpeed = speedVar;
                        railIndex++;
                    }
                    break;

                case 3:
                    _anchorPosition.x = Mathf.Min(_anchorPosition.x, trackThreeLimit);
                    break;
            }

            // Falling height update.
            if (isFalling)
            {
                float _height = railIndex == 2 ? trackTwoHeight : trackThreeHeight;
                fallSpeedVar += Time.deltaTime * fallSpeed;

                _anchorPosition.y -= fallSpeedVar * Time.deltaTime;
                if (_anchorPosition.y < _height)
                {
                    isFalling = false;
                    fallSpeedVar = 0;

                    _anchorPosition.y = _height;
                }
            }

            // Get anchor movement and wagon position.
            anchor.transform.position = _anchorPosition;
            Vector3 _movement = _anchorPosition - previousAnchor;
            previousAnchor = _anchorPosition;

            Vector3 _position = transform.position;
            float _difference = _anchorPosition.x - _position.x;
            if (Mathf.Abs(_difference) > horizontalDistance)
                _difference = horizontalDistance * Mathf.Sign(_difference);

            // Get movement inertia.
            if (_movement.x != 0)
            {
                if (!isMoving)
                {
                    isMoving = true;
                    direction = _movement.x > 0 ? -1 : 1;

                    speedVar = Mathf.MoveTowards(speedVar, maxSpeed, Time.deltaTime * speedIncrease * speedMovementIncrease);
                }
                else if (direction != Mathf.Sign(_movement.x))
                {
                    speedVar = Mathf.MoveTowards(speedVar, maxSpeed, Time.deltaTime * speedIncrease * speedMovementIncrease);
                }
                else
                {
                    speedVar = Mathf.MoveTowards(speedVar, 0, Time.deltaTime * speedDecrease);
                }
            }

            // Move and rotate wagon around anchor.
            if (isMoving)
            {
                _position.x = _anchorPosition.x - _difference;
                _position.x = Mathf.MoveTowards(_position.x, _anchorPosition.x + (horizontalDistance * direction), Time.deltaTime * speedVar);
                _difference = _anchorPosition.x - _position.x;

                // Falling speed variation.
                if (isFalling)
                {
                    speedVar = Mathf.MoveTowards(speedVar, maxSpeed, Time.deltaTime * speedFallIncrease * startFallSpeed);
                }
                // Straight movement speed variation.
                else
                {
                    if (Mathf.Abs(_anchorPosition.x + (horizontalDistance * direction) - _position.x) < MIN_DISTANCE)
                        direction *= -1;

                    if (Mathf.Sign(_difference) == direction)
                    {
                        if (Mathf.Abs(_anchorPosition.x + (horizontalDistance * direction) - _position.x) > MIN_DISTANCE)
                            speedVar = Mathf.MoveTowards(speedVar, maxSpeed, Time.deltaTime * speedIncrease);
                    }
                    else
                    {
                        speedVar = Mathf.MoveTowards(speedVar, 0, Time.deltaTime * speedIncrease * speedDecrease);
                        if (speedVar == 0)
                        {
                            if (Mathf.Abs(_difference) > MIN_DISTANCE)
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
                float _pos = Mathf.Sqrt(Mathf.Pow(distanceMagnitude, 2) - Mathf.Pow(_difference, 2));
                _position.y = _anchorPosition.y - _pos;

                transform.position = _position;
                transform.rotation = Quaternion.LookRotation(Vector3.forward, _anchorPosition - _position);
            }
        }
        #endregion

        #endregion
    }
}
