using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Setup")]
    public Transform player;           // Jugador al que sigue la cámara
    public Vector3 offset = new Vector3(0, 17, 24); // Posición detrás del jugador
    public float followSpeed = 5f;     // Velocidad de seguimiento
    public float rotationSpeed = 2f;   // Velocidad de rotación con el mouse

    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        // Oculta y bloquea el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Si no se asignó manualmente, busca el jugador automáticamente
        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Rotación con el mouse
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -30f, 60f);

        // Calcula rotación
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Calcula posición deseada
        Vector3 desiredPosition = player.position + rotation * offset;

        // Movimiento suave de la cámara
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followSpeed);
        transform.LookAt(player.position + Vector3.up * 1.5f); // Mira ligeramente por encima del centro del jugador
    }
}
