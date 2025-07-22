using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget; // The target to follow
    [SerializeField] float rotationSpeed = 2f; // Speed at which the camera follows the target
    [SerializeField] float distance = 5;

    [SerializeField] float minVerticalAngle = -45;
    [SerializeField] float maxVerticalAngle = 45;
    [SerializeField] Vector2 framingOffset;

    float rotationY;
    float rotationX;

    private void Start()
    {
        Cursor.visible = false; // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    private void Update()
    {
        rotationY += Input.GetAxis("Mouse X") * rotationSpeed;

        rotationX += Input.GetAxis("Mouse Y") * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);// giới hạn góc xoay dọc

        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

        // Tính toán vị trí Vector2 của camera sao cho camera ở độ cao hợp lý

        var forcusPosition = followTarget.position +  new Vector3(framingOffset.x, framingOffset.y);

        // Set camera luôn ở sau player 1 khoảng cách nhất định
        transform.position = forcusPosition - targetRotation * new Vector3(0, 0, distance);

        // Set camera xoay được lên xuống
        transform.rotation = targetRotation;
    }

    public Quaternion PlanarRottation => Quaternion.Euler(0, rotationY, 0); // Chỉ về góc xoay ngang của camera
    

}
