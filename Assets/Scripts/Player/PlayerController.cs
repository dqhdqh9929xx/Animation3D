using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f; // Speed of the player movement
    [SerializeField] float rotationSpeed = 500f; // Speed of the player rotation
    CameraController cameraController; // Reference to the CameraController script

    Quaternion targetRotation; // Target rotation for the player
    Animator animator; // Reference to the Animator component
    [Header("Ground Check Settings")]
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer; // Layer mask to check for ground
    bool isGrounded; // Check if the player is grounded
    float ySpeed; // Vertical speed of the player, used for jumping or falling


    CharacterController characterController; // Reference to the CharacterController component
    // CharacterController là một component trong Unity cho phép bạn di chuyển nhân vật mà không cần phải sử dụng Rigidbody
    // , nó sẽ tự động xử lý va chạm và di chuyển theo hướng của nhân vật

    MeleeFighter meleeFighter; // Reference to the MeleeFighter script, if you have melee combat

    private void Awake()
    {
        cameraController =  Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>(); // Get the Animator component attached to the
        characterController = GetComponent<CharacterController>(); // Get the CharacterController component attached to the player
        meleeFighter = GetComponent<MeleeFighter>(); // Get the MeleeFighter component attached to the player, if you have melee combat
    }

    // Update is called once per frame
    private void Update()
    {
        if (meleeFighter.inAction)
        {
            animator.SetFloat("moveAmount", 0f); // nếu nhân vật thực hiện hành động tấn công thì dampTime animation bằng 0
                                                 // để tránh việc nhân vật đang tấn công mà vẫn có thể di chuyển, làm cho animation bị lỗi
                                                 // khác với Locomotion , Combat sẽ không có dampTime, không có thời gian delay
            return; // nếu nhân vật đang thực hiện hành động tấn công thì không cho phép di chuyển
                    // hay còn hiểu là nếu nhân vật đang tấn công thì bỏ qua tất cả code bên dưới
        }

    
        


        float h = Input.GetAxis("Horizontal"); // Get horizontal input (A/D or Left/Right arrow keys)
        float v = Input.GetAxis("Vertical"); // Get vertical input (W/S or Up/Down arrow keys)

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v)); // Calculate the total movement amount
        // hàm Mathf.Clamp01 sẽ giới hạn giá trị trong khoảng từ 0 đến 1, nếu lớn hơn 1 thì sẽ trả về 1, nếu nhỏ hơn 0 thì sẽ trả về 0
        // Điều này giúp tránh việc nhân quá lớn với moveSpeed, làm cho nhân vật di chuyển quá nhanh không đúng với tham số moveSpeed lớn nhất bằng 1

        var moveInput = (new Vector3(h, 0, v)).normalized;

        var moveDir = cameraController.PlanarRottation * moveInput; // lấy vị trí và hướng của camera để xác định hướng di chuyển
        // Hàm PlanarRottation chỉ trả về góc xoay ngang của camera, không ảnh hưởng đến góc xoay dọc, nhân vật ko thể bay lên được

        GroundCheck();
        if (isGrounded)
        {
            ySpeed = -0.5f;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime; // Apply gravity if not grounded
        }

        var velocity = moveDir * moveSpeed;
        velocity.y = ySpeed; // Set the vertical speed
        characterController.Move(velocity * Time.deltaTime); // Move the player using CharacterController

        if (moveAmount > 0.1f) // If there is significant input
        {
            targetRotation = Quaternion.LookRotation(moveDir); // Quay nhân vật theo hướng camera
            //transform.position += moveDir * moveSpeed * Time.deltaTime; // Di chuyển nhân vật theo hướng camera
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            targetRotation,rotationSpeed * Time.deltaTime); // Quay nhân vật về hướng di chuyển theo bàn phím với tốc độ quay mượt mà
        animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime); // Set the MoveAmount parameter in the Animator to control animations
        // thêm dampTime để làm cho chuyển động mượt mà hơn, tránh việc thay đổi quá nhanh giữa các trạng thái animation
        // dampTime là thời gian để chuyển đổi giữa các trạng thái animation, nếu không có thì sẽ chuyển đổi ngay lập tức
        // dampTime ở đây là 0.2 giây, có nghĩa là sẽ mất 0.2 giây để chuyển đổi giữa các trạng thái animation
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f); // Set the color of the Gizmos to red with some transparency
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius); // Draw a wire sphere at the ground check position
        // Gizmos là một công cụ trong Unity để vẽ các hình ảnh trong Scene view, giúp bạn dễ dàng kiểm tra vị trí và kích thước của các đối tượng trong game
    }
}
