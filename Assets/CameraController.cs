using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Setup")]
    public Transform player;             
    public Transform cameraTarget;        
    public Vector3 offset = new Vector3(0, 15f, -4f); 
    public float followSpeed = 5f;    
    public float rotationSpeed = 2f;

    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        // Ajusta la dirección inicial
        yaw = player.eulerAngles.y;
    }

    void LateUpdate()
    {
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
            transform.LookAt(player);
    }
}
