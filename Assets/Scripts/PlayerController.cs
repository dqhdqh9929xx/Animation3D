using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f; // Speed of the player movement
    [SerializeField] float rotationSpeed = 500f; // Speed of the player rotation

    CameraController cameraController; // Reference to the CameraController script

    Quaternion targetRotation; // Target rotation for the player

    private void Awake()
    {
        cameraController =  Camera.main.GetComponent<CameraController>();
    }

    // Update is called once per frame
    private void Update()
    {
        float h = Input.GetAxis("Horizontal"); // Get horizontal input (A/D or Left/Right arrow keys)
        float v = Input.GetAxis("Vertical"); // Get vertical input (W/S or Up/Down arrow keys)

        float moveAmount = Mathf.Abs(h) + Mathf.Abs(v); // Calculate the total movement amount

        var moveInput = (new Vector3(h, 0, v)).normalized;

        var moveDir = cameraController.PlanarRottation * moveInput; // lấy vị trí và hướng của camera để xác định hướng di chuyển
        // Hàm PlanarRottation chỉ trả về góc xoay ngang của camera, không ảnh hưởng đến góc xoay dọc, nhân vật ko thể bay lên được
        if (moveAmount > 0.1f) // If there is significant input
        {
            targetRotation = Quaternion.LookRotation(moveDir); // Quay nhân vật theo hướng camera
            transform.position += moveDir * moveSpeed * Time.deltaTime; // Di chuyển nhân vật theo hướng camera
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            targetRotation,rotationSpeed * Time.deltaTime); // Quay nhân vật về hướng di chuyển theo bàn phím với tốc độ quay mượt mà
    }
}
