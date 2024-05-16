using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using Random = System.Random;

public enum GearState {
    Neutral,
    Running,
    CheckingChange,
    Changing
};

public class CarController : MonoBehaviour {
    private float horizontalInput, verticalInput;
    private float currentSteerAngle;
    private bool isBreaking;

    // Settings
    [Header("Settings")]
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
    [SerializeField] private CinemachineVirtualCamera CineCamera;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private bool ControllerEnabled;

    [SerializeField] private float increaseGearRPM;
    [SerializeField] private float decreaseGearRPM;
    [SerializeField] private float changeGearTime = 0.5f;

    Random random = new Random();

    [Header("Effects")]
    [SerializeField] private GameObject SteeringWheel;
    [SerializeField] private Rigidbody VehicleRigidBody;

    // Wheel Colliders
    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    // Wheels
    [Header("Wheels")]
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

   // UI
   [Header("UI")]
    [SerializeField] private GameObject RPMBar;
    [SerializeField] private Slider RPMSlider;
    [SerializeField] private TMP_Text SpeedIndicator;
    [SerializeField] private TMP_Text GearIndicator;

    // Car State


    // Inputs
    float GetForward() => playerInput.actions["Accelerate"].ReadValue<float>();
    float GetBrake() => playerInput.actions["Brake"].ReadValue<float>();
    float GearUp() => playerInput.actions["GearUp"].ReadValue<float>();
    float GearDown() => playerInput.actions["GearDown"].ReadValue<float>();
    float GetClutch() => playerInput.actions["Clutch"].ReadValue<float>();
    float GetLookRight() => playerInput.actions["CameraRight"].ReadValue<float>();
    float GetLookLeft() => playerInput.actions["CameraLeft"].ReadValue<float>();

    private void Update() {
        UpdateUIElements();
    }

    private void FixedUpdate() {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        clutch = 1 - GetClutch();
        if (GetLookRight() > 0)
            CineCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset =
                new Vector3(0.1f, 0, -0.24f);
        else if (GetLookLeft() > 0)
            CineCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset =
                new Vector3(-0.1f, 0, -0.24f);
        else 
            CineCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset =
            new Vector3(0f, 0, -0.24f);
    }

    private void GetInput() {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        if (ControllerEnabled) {
            verticalInput = GetForward();
        } else {
            verticalInput = Input.GetAxis("Vertical");
        }
    }

    float CalculateTorque(float gasInput) {
        float torque = 0;
        //if (RPM < idleRPM + 200 && gasInput == 0 && currentGear == 0) {
        //    gearState = GearState.Neutral;
        //}

        if (gearState == GearState.Running && clutch > 0) {
            if (Input.GetKey(KeyCode.E) || GearUp() == 1) { // Get Key E | RPM > increaseGearRPM
                StartCoroutine(ChangeGear(1));
            }
            else if (Input.GetKey(KeyCode.Q) || GearDown() == 1) { // GetKey Q | RPM < decreaseGearRPM
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
        if (verticalInput < 0 || GetBrake() > 0) {
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
        float appliedBrakePower = isBreaking ? brakePower * GetBrake() : 0f;
        float applySlowBrake = (verticalInput < .05) ? 200 : 0.25f;

        frontRightWheelCollider.wheelDampingRate = applySlowBrake;
        frontLeftWheelCollider.wheelDampingRate = applySlowBrake;
        rearLeftWheelCollider.wheelDampingRate = applySlowBrake;
        rearRightWheelCollider.wheelDampingRate = applySlowBrake;

        frontRightWheelCollider.brakeTorque = appliedBrakePower;
        frontLeftWheelCollider.brakeTorque = appliedBrakePower;
        rearLeftWheelCollider.brakeTorque = appliedBrakePower;
        rearRightWheelCollider.brakeTorque = appliedBrakePower;
    }

    private void HandleSteering() {
        currentSteerAngle = maxSteerAngle * horizontalInput;

        SteeringWheel.transform.localRotation = Quaternion.Euler(new Vector3(-15.957f, -180,
            (currentSteerAngle / maxSteerAngle * 360)));

        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels() {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateUIElements() {
        RPMSlider.value = RPM;

        if (RPM / redLine > .95) {
            RPMBar.GetComponent<Image>().color = Color.red;
        } else RPMBar.GetComponent<Image>().color = Color.white;

        SpeedIndicator.text = $"Speed: {MathF.Floor(VehicleRigidBody.velocity.magnitude * 3.6f)}";
        GearIndicator.text = $"Gear: {currentGear}";
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}