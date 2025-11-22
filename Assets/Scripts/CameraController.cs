using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Setup")]
    public Transform player;             
    public Transform cameraTarget;        
    public Vector3 shoulderOffset = new Vector3(0.3f, 10f, -8f); 
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
        UpdateCameraPositionAndRotation();
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
            cameraTarget.rotation = Quaternion.Euler(0f, yaw, 0f);
        else if (player != null)
            player.rotation = Quaternion.Euler(0f, yaw, 0f);
        Quaternion camRotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPosition = cameraTarget.position + camRotation * shoulderOffset;
        mainCamera.position = Vector3.Lerp(mainCamera.position, desiredPosition, followSpeed * Time.deltaTime);
        mainCamera.rotation = camRotation;
    }
}
