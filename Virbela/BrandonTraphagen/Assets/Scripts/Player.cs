using UnityEngine;

/// <summary>
/// Main player game object will be looking to interact with other objects.
/// </summary>
public class Player : MonoBehaviour
{
    public GameObject CameraObject;
    private CharacterController controller;

    public float PlayerSpeed = 20.0f;

    private Vector3 playerMoveDirection = Vector3.zero;
    private Vector2 cameraRotation = Vector2.zero;
    private float cameraLookSpeed = 2.0f;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        cameraRotation.x = transform.eulerAngles.x;
        cameraRotation.y = transform.eulerAngles.y;
    }

    /// <summary>
    /// Update the cameraObject rotation and view around the Player.
    /// </summary>
    private void UpdateCameraObject()
    {
        cameraRotation.y += Input.GetAxis("Mouse X") * cameraLookSpeed;
        cameraRotation.x += -Input.GetAxis("Mouse Y") * cameraLookSpeed;
        CameraObject.transform.localRotation = Quaternion.Euler(cameraRotation.x, 0, 0);
        transform.eulerAngles = new Vector2(cameraRotation.x, cameraRotation.y);
    }

    /// <summary>
    /// Update the Player by speed and input.
    /// </summary>
    private void UpdatePlayer()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        float curSpeedX = PlayerSpeed * Input.GetAxis("Vertical");
        float curSpeedY = PlayerSpeed * Input.GetAxis("Horizontal");
        playerMoveDirection = (forward * curSpeedX) + (right * curSpeedY);
        controller.Move(playerMoveDirection * Time.deltaTime);
    }

    /// <summary>
    /// Used to update the Players position and the cameraObject position around the Player.
    /// </summary>
    private void Update()
    {
        UpdatePlayer();
        UpdateCameraObject();
    }
}
