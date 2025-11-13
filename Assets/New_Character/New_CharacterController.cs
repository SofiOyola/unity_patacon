using UnityEngine;

[RequireComponent (typeof(CharacterController))]

public class New_CharacterController : MonoBehaviour
{   
    //Configuración del movimiento 
    [Header("Movimiento")]
    public float WalkSpeed = 4f;  
    public float SprintSpeed = 6f;
    public float jumpHeight = 2f;
    public float rotationSpeed = 10f;
    public float mouseSensitivity = 1f;
    public float gravity = -20f;

    //Referenciación a otros componentes que vamos a usar
    [Header("Referenciación")]
    public Transform cameraTransform;
    public Animator animator; 

    private CharacterController characterController;
    private Vector3 Velocity;
    private float currentSpeed;
    private float yaw;
    private Vector3 externalVelocity = Vector3.zero;

    //Para determinar sí se está moviendo min48
    public bool IsMoving {get; private set;}
    //Poder acceder al eje horizontal o vertical 
    public Vector2 CurrentInput{get; private set;}
    //Para el salto (verificar el piso)
    public bool IsGrounded{get; private set;}
    //Lectura de la rotación
    public float CurrentYaw => yaw;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController> ();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
        updateAnimator();
    }

    //Función control de movimiento
    void HandleMovement(){
        IsGrounded = characterController.isGrounded; //Verifica sí tocamos o no suelo

        if(IsGrounded && Velocity.y < 0){
            if(externalVelocity.y > -0.05f && externalVelocity.y < 0.05f)
                Velocity.y = 0;
            else
                Velocity.y = -2f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;
        IsMoving = inputDirection.magnitude > 0.1f;

        Vector3 moveDirection = Vector3.zero;

        if(IsMoving){
            moveDirection = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * inputDirection;
            bool isSprinting = Input.GetKey(KeyCode.LeftShift);
            currentSpeed = isSprinting ? SprintSpeed : WalkSpeed;
        }

        if(Input.GetButtonDown("Jump")&& IsGrounded){
            Velocity.y = Mathf.Sqrt(jumpHeight*-2f*gravity);
            animator?.SetBool("isJumping", true);

        }

        Velocity.y += gravity*Time.deltaTime;


        //Para plataformas
        Vector3 finalMovement = (moveDirection * currentSpeed + externalVelocity)*Time.deltaTime;
        finalMovement.y += Velocity.y * Time.deltaTime;

        //Mover el personaje a través del character controller
        characterController.Move(finalMovement);

        //Grounded
        if(IsGrounded && Velocity.y < 0f){
            animator?.SetBool("isJumping", false);
        }
    }

    void HandleRotation(){
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        yaw += mouseX;

        //Slerp es un smooth para una rotación suave (no brusca)
        if(IsMoving){
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, yaw, 0f), rotationSpeed*Time.deltaTime);

        }
    }

    void updateAnimator(){
        float SpeedPercent = IsMoving?(currentSpeed == SprintSpeed? 1f: 0.5f):0f;
        animator?.SetFloat("Speed", SpeedPercent, 0.1f, Time.deltaTime);
        animator?.SetBool("IsGrounded", IsGrounded);
        animator?.SetFloat("VerticalSpeed", Velocity.y);
    }


}
