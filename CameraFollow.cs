using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float cameraSpeed;
    public float cameraRotationSpeed;
    public Transform player;

    private Vector3 cameraOffset;
    private Vector3 goalPosition;
    private Quaternion goalRotation;

    private void Start()
    {
        cameraOffset = player.position - transform.position;
    }
    private void FixedUpdate()
    {
        goalPosition = player.position;
        goalRotation = Quaternion.LookRotation(player.forward, player.up);

        goalPosition -= (transform.rotation * cameraOffset);

        Vector3 goalDirection = (goalPosition - transform.position);
        Vector3 goalOffset = goalDirection * cameraSpeed * Time.fixedDeltaTime;

        if(Vector3.Dot((goalPosition - transform.position),goalPosition - (transform.position + goalOffset)) > 0.0f)
        {
            transform.position += goalOffset;
        }
        else
        {
            transform.position = goalPosition;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation,goalRotation, cameraRotationSpeed * Time.fixedDeltaTime);
    }

}
