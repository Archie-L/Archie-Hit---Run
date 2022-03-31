using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentBreakForce;
    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteeringAngle;

    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider rearLeft;
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider rearRight;

    [SerializeField] private Transform frontLeftT;
    [SerializeField] private Transform rearLeftT;
    [SerializeField] private Transform frontRightT;
    [SerializeField] private Transform rearRightT;

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void HandleMotor()
    {
        rearLeft.motorTorque = -verticalInput * motorForce;
        rearRight.motorTorque = -verticalInput * motorForce;
        currentBreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontLeft.brakeTorque = currentBreakForce;
        frontRight.brakeTorque = currentBreakForce;
        rearLeft.brakeTorque = currentBreakForce;
        rearRight.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteeringAngle * horizontalInput;
        frontLeft.steerAngle = currentSteerAngle;
        frontRight.steerAngle = currentSteerAngle;
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeft, frontLeftT);
        UpdateSingleWheel(frontRight, frontRightT);
        UpdateSingleWheel(rearLeft, rearLeftT);
        UpdateSingleWheel(rearRight, rearRightT);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}