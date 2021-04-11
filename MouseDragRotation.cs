using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDragRotation : MonoBehaviour {

    [SerializeField]
    private float speedRotation = 100f;

    private float minX, maxX, minY, maxY;
    private bool activeTouch = false;

    private Vector3 rot = Vector3.zero;

    // Use this for initialization
    void Start () {

        minX = Screen.width * 0.1f;
        maxX = Screen.width * (1 - 0.1f);
        minY = Screen.height * 0.1f;
        maxY = Screen.height * (1 - 0.1f);
        rot = transform.eulerAngles;

		
	}
	
	// Update is called once per frame
	void Update () {

        if(ValidMousePosition())
        {
            activeTouch = true;
        }
        if(Input.GetMouseButtonUp(0) && activeTouch)
        {
            activeTouch = false;
        }
        if(Input.GetMouseButton(0) && activeTouch)
        {
            float xAxis = Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1);
            float yAxis = Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1);

            rot.y += xAxis * (speedRotation * Time.deltaTime);
            rot.x += yAxis * (speedRotation * Time.deltaTime);

            transform.rotation = Quaternion.Euler(rot.x, rot.y, 0);

        }
		
	}

    private bool ValidMousePosition()
    {
        if(Input.mousePosition.x > minX && Input.mousePosition.x < maxX && Input.mousePosition.y > minY && Input.mousePosition.y < maxY)
        {
            return true;
        }
        return false;
    }
}
