using UnityEngine;

public class HeatingInputBehaviour : MonoBehaviour
{
    [SerializeField] private Camera currentCamera; 
    private Vector2 previousPosition = Vector2.zero;

    private Vector2 currentPosition;
    private bool isInitialized = false;
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            currentPosition = currentCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position; 
            float _angle = (Mathf.Atan2(currentPosition.y, currentPosition.x) - Mathf.Atan2(previousPosition.y, previousPosition.x)) * Mathf.Rad2Deg;
            previousPosition = currentPosition; 
            if(!isInitialized)
            {
                isInitialized = true;
                return; 
            }
            transform.eulerAngles += new Vector3(0,0,_angle); 
        }        
        else
        {
            previousPosition.Set(0, 0);
            isInitialized = false; 
        }

        if(Input.touchCount == 1)
        {
            currentPosition = currentCamera.ScreenToWorldPoint(Input.GetTouch(0).position) - transform.position;
            float _angle = (Mathf.Atan2(currentPosition.y, currentPosition.x) - Mathf.Atan2(previousPosition.y, previousPosition.x)) * Mathf.Rad2Deg;
            previousPosition = currentPosition;
            if (!isInitialized)
            {
                isInitialized = true;
                return;
            }
            transform.eulerAngles += new Vector3(0, 0, _angle);
        }
        else
        {
            previousPosition.Set(0,0);
            isInitialized = false; 
        }

    }
}
