using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    [Header("Camera Setup")]
    public Transform player;             
    public Transform cameraTarget;        
    public Vector3 shoulderOffset = new Vector3(0.3f, 1.6f, -3f);
    public float followSpeed = 10f;    
    public float rotationSpeed = 5f;
    public float mouseSensitivity = 0.5f;

    [Header("Orbita")]
    public float minPitch = -40f;
    public float maxPitch = 70f;

    private float yaw = 0f;
    private float pitch = 0f;

    private New_CharacterController playerController;
    private Transform mainCamera;

    void Start()
    {
        playerController = player.GetComponent<New_CharacterController>();
        mainCamera = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        HandleInput();
        UpdateCameraPositionAndRotation();
    }

    private void HandleInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;

        pitch -= mouseY * rotationSpeed;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void UpdateCameraPositionAndRotation()
    {
        if (cameraTarget != null)
            cameraTarget.rotation = Quaternion.Euler(pitch, yaw, 0f);
        else if (player != null)
            player.rotation = Quaternion.Euler(0f, yaw, 0f);
        Quaternion camRotation = cameraTarget.rotation;
        Vector3 desiredPosition = cameraTarget.position + camRotation * shoulderOffset;
        mainCamera.position = Vector3.Lerp(mainCamera.position, desiredPosition, followSpeed * Time.deltaTime);
        mainCamera.rotation = camRotation;
    }
}
