using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public enum GearState {
    Neutral,
    Running,
    CheckingChange,
    Changing
};

public class CarController : MonoBehaviour {
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;

    // Settings
    [SerializeField] public int isEngineRunning;

    [SerializeField] private float maxSteerAngle;
    [SerializeField] private int currentGear = 0;
    [SerializeField] private float RPM;
    [SerializeField] private float idleRPM;
    [SerializeField] private float clutch;
    [SerializeField] private GearState gearState;
    [SerializeField] private float redLine;
    [SerializeField] private float wheelRPM;
    [SerializeField] private float[] gearRatios;
    [SerializeField] private float motorPower;
    [SerializeField] private float brakePower;
    [SerializeField] private float differentialRatio;
    [SerializeField] private AnimationCurve hpToRPMCurve;
    Random random = new Random();

    [SerializeField] private float increaseGearRPM;
    [SerializeField] private float decreaseGearRPM;
    [SerializeField] private float changeGearTime = 0.5f;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private void FixedUpdate() {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput() {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

    }

    float CalculateTorque(float gasInput) {
        float torque = 0;
        if (RPM < idleRPM + 200 && gasInput == 0 && currentGear == 0) {
            gearState = GearState.Neutral;
        }

        if (gearState == GearState.Running && clutch > 0) {
            if (RPM > increaseGearRPM) { // Get Key E
                StartCoroutine(ChangeGear(1));
            }
            else if (RPM < decreaseGearRPM) { // GetKey Q
                StartCoroutine(ChangeGear(-1));
            }
        }

        if (isEngineRunning > 0) {
            if (clutch < 0.1f) {
                RPM = Mathf.Lerp(RPM, Mathf.Max(idleRPM, redLine * gasInput) + random.Next(-50, 50), Time.deltaTime);
            } else {
                wheelRPM = Mathf.Abs((rearRightWheelCollider.rpm + rearLeftWheelCollider.rpm) / 2f) * gearRatios[currentGear] * differentialRatio;
                RPM = Mathf.Lerp(RPM, Mathf.Max(idleRPM - 100, wheelRPM), Time.deltaTime * 3f);
                torque = (hpToRPMCurve.Evaluate(RPM / redLine) * motorPower / RPM) * gearRatios[currentGear] * differentialRatio * 5252f * clutch;
            }
        }
        return torque;
    }

    IEnumerator ChangeGear(int gearChange) {
        gearState = GearState.CheckingChange;
        if (currentGear + gearChange >= 0) {
            if (gearChange > 0) {
                //increase the gear
                yield return new WaitForSeconds(0.7f);
                if (RPM < increaseGearRPM || currentGear >= gearRatios.Length - 1) {
                    gearState = GearState.Running;
                    yield break;
                }
            }
            if (gearChange < 0) {
                //decrease the gear
                yield return new WaitForSeconds(0.1f);

                if (RPM > decreaseGearRPM || currentGear <= 0) {
                    gearState = GearState.Running;
                    yield break;
                }
            }
            gearState = GearState.Changing;
            yield return new WaitForSeconds(changeGearTime);
            currentGear += gearChange;
        }

        if (gearState != GearState.Neutral)
            gearState = GearState.Running;
    }

    private void HandleMotor() {
        if (verticalInput < 0) {
            verticalInput = 0;
            isBreaking = true;
        }
        else isBreaking = false;

        rearLeftWheelCollider.motorTorque = CalculateTorque(verticalInput) * verticalInput;
        rearRightWheelCollider.motorTorque = CalculateTorque(verticalInput) * verticalInput;
        // rearLeftWheelCollider.motorTorque = MathF.Max(verticalInput, 0) * motorTorque;
        // rearRightWheelCollider.motorTorque = MathF.Max(verticalInput, 0) * motorTorque;
        // currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking() {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering() {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels() {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}