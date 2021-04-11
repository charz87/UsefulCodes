using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePinchZoom : MonoBehaviour {

    private float minFov = 15;
    private float maxFov = 60;

    [SerializeField]
    private float speedZoom = 10;

    private float fov;

    // Update is called once per frame
    void Update() {

        fov = Camera.main.fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * speedZoom;
        fov = Mathf.Clamp(fov, minFov, maxFov);

        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
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

            fov -= deltaMagnitudeDiff * (speedZoom - 9f);
            fov = Mathf.Clamp(fov, minFov, maxFov);
        }

        Camera.main.fieldOfView = fov;



    }
}
