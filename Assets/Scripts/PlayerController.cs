using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Movement
    [SerializeField] private float speed = 12f;
    [SerializeField] private float gravity = -19.62f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Vector3 movementVector = Vector3.zero;
    Vector3 velocity;
    bool isGrounded;

    // Dash
    Dash dash;
    [SerializeField] private float dashSpeed = 30.0f;
    [SerializeField] private float dashLength = 0.2f;
    [SerializeField] private float dashResetTime = 2f;
    [SerializeField] private int maxDashAttempts = 1;
    CharacterController characterController;

    // Grapple
    private Camera playerCamera;
    private grappleState state;
    private Vector3 grapplePos;
    private enum grappleState {
        normal, launch, end
    }
    [SerializeField] private Transform hitPoint;

    // Called on component startup.
    private void Start()
    {
        this.characterController = GetComponent<CharacterController>();
        dash = new Dash(dashSpeed, dashLength, dashResetTime, maxDashAttempts);
        state = grappleState.normal;
    }

    private void Awake() {
        playerCamera = transform.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame.
    void Update()
    {
        switch (state) {
            default:
            case grappleState.normal:
                HandleMovement();
                dash.HandleDash(movementVector, transform, characterController, isGrounded);
                HandleGrappleStart();
                break;
            case grappleState.launch:
                HandleGrappleLaunch();
                break;
        }
    }

    void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance,groundMask);

        if (isGrounded && velocity.y < 0.0f)
        {
            velocity.y = -2f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        movementVector = Vector3.ClampMagnitude(transform.right * horizontalInput + transform.forward * verticalInput, 1.0f);
        controller.Move(movementVector * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    
    void HandleGrappleStart() {
        if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit rayHit)) {
                hitPoint.position = rayHit.point;
                grapplePos = rayHit.point;
                state = grappleState.launch;
            }
        }
    }

    void HandleGrappleLaunch() {
        Vector3 grappleDir = (grapplePos - transform.position).normalized;

        float launchSpeed = 10.0f;
        controller.Move(grappleDir * launchSpeed * Time.deltaTime);
    }
}
