using UnityEngine;

public class MouseMovement : MonoBehaviour {
    public float mouseSensitivity = 100f;
    float xRotation = 0f;
    float YRotation = 0f;
    public PlayerMovement playerController;

    void Start() {
      Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
      if (playerController.GetCanMove() == false) {
        return;
      }

      float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
      float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

      xRotation -= mouseY;

      xRotation = Mathf.Clamp(xRotation, -30f, 30f);

      YRotation += mouseX;

      transform.localRotation = Quaternion.Euler(xRotation, YRotation, 0f);
    }
}
