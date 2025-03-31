using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public bool canMove = true;
    public float bobbingSpeed = 6f;  
    public float bobbingAmount = 0.1f;
    public float sprintMultiplier = 1.6f;
    public float sprintBobbingMultiplier = 1.3f; // Усиление покачивания при беге

    private Vector3 velocity;
    private float bobbingTimer = 0f;
    private float defaultCameraY;

    void Start() {
        if (cameraTransform != null) {
            defaultCameraY = cameraTransform.localPosition.y;
        }
    }

    void Update() {
        if (!canMove) {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        bool isMoving = move.magnitude > 0.1f;
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        float currentSpeed = isSprinting ? speed * sprintMultiplier : speed;
        float currentBobbingSpeed = isSprinting ? bobbingSpeed * sprintBobbingMultiplier : bobbingSpeed;
        float currentBobbingAmount = isSprinting ? bobbingAmount * sprintBobbingMultiplier : bobbingAmount;

        controller.Move(move * currentSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        if (controller.isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        controller.Move(velocity * Time.deltaTime);

        // Покачивание камеры
        if (isMoving && controller.isGrounded) {
            bobbingTimer += currentBobbingSpeed * Time.deltaTime;
            float bobbingOffset = Mathf.Sin(bobbingTimer) * currentBobbingAmount;
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, defaultCameraY + bobbingOffset, cameraTransform.localPosition.z);
        }
        else {
            bobbingTimer = 0f;
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, defaultCameraY, cameraTransform.localPosition.z);
        }
    }

    public void SetCanMove(bool value) {
        canMove = value;
    }

    public bool GetCanMove() {
        return canMove;
    }
}