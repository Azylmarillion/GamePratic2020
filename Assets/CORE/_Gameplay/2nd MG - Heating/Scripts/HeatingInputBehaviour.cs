// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GamePratic2020
{ 
    public class HeatingInputBehaviour : MonoBehaviour
    {
        #region Fields and Properties
        [HorizontalLine(1, order = 0), Section("HeatingInputBehaviour", order = 1)]
        [SerializeField] private Camera currentCamera;
        [SerializeField] private Thermometer thermometer;

        [SerializeField, Required] private AudioSource crankSource = null; 

        private Vector2 previousPosition = Vector2.zero;
        private Vector2 currentPosition;
        private bool isTurningCrank = false;
        #endregion

        #region Methods
        private void Start()
        {
            crankSource.clip = GameManager.Instance.SoundDataBase.CrankLoop; 
        }

        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                currentPosition = currentCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                float _angle = (Mathf.Atan2(currentPosition.y, currentPosition.x) - Mathf.Atan2(previousPosition.y, previousPosition.x)) * Mathf.Rad2Deg;
                previousPosition = currentPosition;
                if (!isTurningCrank)
                {
                    isTurningCrank = true;
                    crankSource.Play(); 
                    return;
                }
                transform.eulerAngles += new Vector3(0, 0, _angle);
                thermometer.IncreaseRatio(Mathf.Abs(_angle * Time.deltaTime));
            }
            else
            {
                if(isTurningCrank)
                {
                    crankSource.Stop(); 
                    previousPosition.Set(0, 0);
                    isTurningCrank = false;
                }

            }
#else
            if (Input.touchCount == 1)
            {
                currentPosition = currentCamera.ScreenToWorldPoint(Input.GetTouch(0).position) - transform.position;
                float _angle = (Mathf.Atan2(currentPosition.y, currentPosition.x) - Mathf.Atan2(previousPosition.y, previousPosition.x)) * Mathf.Rad2Deg;
                previousPosition = currentPosition;
                if (!isTurningCrank)
                {
                    isTurningCrank = true;
                    crankSource.Play(); 
                    return;
                }
                transform.eulerAngles += new Vector3(0, 0, _angle);
                thermometer.IncreaseRatio(Mathf.Abs(_angle * Time.deltaTime));
            }
            else
            {
                if(isTurningCrank)
                {
                    crankSource.Stop(); 
                    previousPosition.Set(0, 0);
                    isTurningCrank = false;
                }

            }
#endif
        }
        #endregion
    }
}