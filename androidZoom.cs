using UnityEngine;
using System.Collections;

public class androidZoom : MonoBehaviour {

    public float perspectiveZoomSpeed = 0.5f;        
    public float orthoZoomSpeed = 0.5f;       
    public Camera normalCamera;


	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
         if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

             
             
            
             
                normalCamera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                
                normalCamera.fieldOfView = Mathf.Clamp(normalCamera.fieldOfView, 0.1f, 179.9f);
            }
        }
    
	
	}

