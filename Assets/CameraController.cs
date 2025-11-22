using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Setup")]
    public Transform player;             
    public Transform cameraTarget;        
    public Vector3 shoulderOffset = new Vector3(0.3f, 1.7f, -2f); 
    public float followSpeed = 10f;    
    public float rotationSpeed = 5f;
    public float mouseSensitivity = 0.5f;

    [Header("Orbita")]
    public float minPitch = 28f;
    public float maxPitch = 60f;

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
/*
        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        // Ajusta la dirección inicial
        yaw = player.eulerAngles.y;*/
    }

    void LateUpdate()
    {
        HandleInput();
        UpdateCameraPosition();
        /*
        if (player == null) return;

        // Rotación con el mouse
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -30f, 60f);
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Posición ideal detrás del personaje
        Vector3 desiredPosition = player.position + rotation * offset;

        // Movimiento suave
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followSpeed);

        // Mira al punto CameraTarget
        if (cameraTarget != null)
            transform.LookAt(cameraTarget);
        else
            transform.LookAt(player);*/
    }

    void HandleInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (playerController.IsMoving)
        {
            yaw = playerController.CurrentYaw;
        }
        else
        {
            yaw += mouseX * rotationSpeed;
        }

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

    }

    void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 targetPosition = cameraTarget.position + rotation * shoulderOffset;

        mainCamera.position = Vector3.Lerp(mainCamera.position, targetPosition, followSpeed * Time.deltaTime);
        mainCamera.LookAt(cameraTarget);
    }
}
