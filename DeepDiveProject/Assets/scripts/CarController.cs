using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    const string Horizontal = "Horizontal";
    const string Vertical = "Vertical";

    float horizontalInput;
    float verticalInput;
    float currentSteerAngle;
    float currentBreakForce;
    bool isBreaking;

    [SerializeField] float motorForce;
    [SerializeField] float breakForce;
    [SerializeField] float maxSteerAngle;



    [SerializeField] WheelCollider FrontLeftWheelCollider;
    [SerializeField] WheelCollider FrontRightWheelCollider;
    [SerializeField] WheelCollider BackLeftWheelCollider;
    [SerializeField] WheelCollider BackRightWheelCollider;

    [SerializeField] Transform FrontLeftWheelTransform;
    [SerializeField] Transform FrontRightWheelTransform;
    [SerializeField] Transform BackLeftWheelTransform;
    [SerializeField] Transform BackRightWheelTransform;

    void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }


    private void HandleMotor()
    {
        FrontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        FrontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentBreakForce = isBreaking ? breakForce : 0f;
        if (isBreaking)
        {
            ApplyBreaking();
        }

    }
    private void GetInput()
    {
        horizontalInput = Input.GetAxis(Horizontal);
        verticalInput = Input.GetAxis(Vertical);
        isBreaking = Input.GetKey(KeyCode.Space);

    }

    private void ApplyBreaking()
    {
       FrontRightWheelCollider.brakeTorque = currentBreakForce;
       FrontLeftWheelCollider.brakeTorque = currentBreakForce;
       BackRightWheelCollider.brakeTorque = currentBreakForce;
       BackLeftWheelCollider.brakeTorque = currentBreakForce;

    }

    private void HandleSteering()
    {
       currentSteerAngle = maxSteerAngle * horizontalInput;
       FrontLeftWheelCollider.steerAngle = currentSteerAngle;
       FrontRightWheelCollider.steerAngle = currentSteerAngle;
    }
    private void UpdateWheels()
    {
        UpdateSingleWheel(FrontLeftWheelCollider, FrontLeftWheelTransform);
        UpdateSingleWheel(BackRightWheelCollider, BackRightWheelTransform);
        UpdateSingleWheel(BackLeftWheelCollider, BackLeftWheelTransform);
        UpdateSingleWheel(FrontRightWheelCollider, FrontRightWheelTransform);
    }
    
    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = new Quaternion(rot.x, rot.y+90, rot.z + 90, rot.w);
        wheelTransform.position = pos;
    }
}
