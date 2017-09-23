using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	float minX, maxX, minY, maxY;
	public float por = .3f;
	
	bool isActiveTouch = false;
	
	public GameObject rotationPivot;
	public float speed = 10;
	Vector3 rot = Vector3.zero;
	float valZ = 0;
	public Camera raCamera;
	public Camera normalCamera;
	bool switchCamera = true;
    public GameObject instructionsText;
   

    

	public bool isArCamera = false;

    public void shutDownCamera ()
    {
        isArCamera = true;
        switchCamera = true;
        raCamera.enabled = true;
        normalCamera.enabled = false;
    }
	public void toggleCamera()
	{
		if (switchCamera) 
		{
			isArCamera = false;
			switchCamera = false;
			raCamera.enabled = false;
			normalCamera.enabled = true;
            instructionsText.SetActive(true);
		} 
		else
		{
			isArCamera = true;
			switchCamera = true;
			raCamera.enabled = true;
			normalCamera.enabled = false;
            instructionsText.SetActive(false);
		}
	}



	// Use this for initialization
	void Start () {
		rot = rotationPivot.gameObject.transform.rotation.eulerAngles;
		minX = Screen.width * por;
		maxX = Screen.width * (1 - por);
		minY = Screen.height * por;
		maxY = Screen.height * (1 - por);
		raCamera.enabled = true;
		normalCamera.enabled = false;
		isArCamera = true;
	}
	

	// Update is called once per frame
	void Update () {

		if(!switchCamera)
		{
            /*if (Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(1).phase == TouchPhase.Began)
            {


            }
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                Vector2 Position1 = Input.GetTouch(0).position;
                Vector2 Position2 = Input.GetTouch(1).position;
                Vector2 substract = Position1 - Position2;
                float delta = substract.magnitude;

            }*/

			if(validMousePosition())
			{
				isActiveTouch = true;
			}
			
			if(Input.GetMouseButtonUp(0))
			{
				isActiveTouch = false;
			}
			if(Input.GetMouseButton(0) && isActiveTouch)
			{
				float xAxis = Mathf.Clamp(Input.GetAxis("Mouse X"),-1,1);
				float yAxis = Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1);
                yAxis *= -1;
				
				rot.y += (Time.deltaTime*speed) * xAxis;
				valZ += (Time.deltaTime * speed) * yAxis;
				if(valZ > 60)
				{
					valZ = 60;
				}

				if(valZ < 5)
				{
					valZ =5;
				}

				if(valZ > 5 && valZ < 60)
				{
					rot.z = valZ; 
				}

				
				rot.x = 0;
				rotationPivot.transform.rotation = Quaternion.Euler(new Vector3(rot.z,rot.y,0));
			}
		}
	}

	bool validMousePosition()
	{
		if(Input.mousePosition.x > minX && Input.mousePosition.x < maxX && Input.mousePosition.y > minY && Input.mousePosition.y < maxY )
		{
			return true;
		}
		return false;
	}
}
