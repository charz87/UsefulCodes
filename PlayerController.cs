using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(Animator))]

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;

    [SerializeField]
    private float thrusterForce = 1000f;

    [SerializeField]
    private float thrusterFuelBurnSpeed = 1f;
    [SerializeField]
    private float thrusterFuelRegenSpeed = 0.3f;
    private float thrusterFuelAmount = 1f;

    public float GetThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    }

    [SerializeField]
    private LayerMask environmentMask;

    [Header("Joint Settings")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    private PlayerMotor motor;

    private ConfigurableJoint joint;

    private Animator anim;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        anim = GetComponent<Animator>();

        SetJointSettings(jointSpring);
    }

    private void Update()
    {
        if (PauseMenu.isOn)
            return;
        // Setting Target position for Spring
        // this makes physics act right when it comes to 
        // applying gravity when flying over objects
        RaycastHit hit;
        if(Physics.Raycast(transform.position,Vector3.down,out hit,100f,environmentMask))
        {
            joint.targetPosition = new Vector3(0f,-hit.point.y, 0f);
        }
        else
        {
            joint.targetPosition = new Vector3(0f, 0f, 0f);
        }


        //Calculate velocity as 3d Vector
        float xMov = Input.GetAxis("Horizontal");
        float zMov = Input.GetAxis("Vertical");

        Vector3 movHorizontal = transform.right * xMov;
        Vector3 movVertical = transform.forward * zMov;

        //Final movement Vector
        Vector3 velocity = (movHorizontal + movVertical) * speed;

        //Animate movement
        anim.SetFloat("ForwardVelocity", zMov);

        //Apply movement
        motor.Move(velocity);

        //Calculate rotation as a 3d Vector (turning Around)
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 rot = new Vector3(0,yRot,0) * lookSensitivity;

        //Apply rotation
        motor.Rotate(rot);

        //Calculate camera rotation as a 3d Vector (turning Around)
        float xRot = Input.GetAxisRaw("Mouse Y");

        float cameraRotX = xRot * lookSensitivity;

        //Apply rotation
        motor.RotateCamera(cameraRotX);

        //Calculate thruster force based on player input
        Vector3 _thrusterForce = Vector3.zero;
        
        if(Input.GetButton("Jump") && thrusterFuelAmount > 0f)
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

            if(thrusterFuelAmount >= 0.01f)
            {
                _thrusterForce = Vector3.up * thrusterForce;
                SetJointSettings(0f);
            }
            
        }else
        {
            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
            SetJointSettings(jointSpring);
        }

        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0, 1);
        //Apply the thruster force
        motor.ApplyThruster(_thrusterForce);


    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }

}
