using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private float mouseSensitivity = 250.0f;
    MouseLook mouseLook;

    [Header("Movement")]
    [SerializeField] private float speed = 12.0f;
    [SerializeField] private float gravity = -19.62f;
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Vector3 movementVector = Vector3.zero;
    Vector3 velocity;
    bool isGrounded;

    [Header("Double Jump")]
    [SerializeField] private int numJumps = 2;
    private int numJumped;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 30.0f;
    [SerializeField] private float dashLength = 0.2f;
    [SerializeField] private float dashResetTime = 2f;
    [SerializeField] private int maxDashAttempts = 1;
    CharacterController characterController;
    Dash dash;

    [Header("Crouch Dash")]
    [SerializeField] private float crouchDashSpeed = 50.0f;
    [SerializeField] private float crouchDashLength = 0.2f;
    [SerializeField] private float crouchDashResetTime = 0.0f;
    [SerializeField] private int maxCrouchDashAttempts = 1;
    [SerializeField] private float reducedHeight = 0.7f;
    private float origHeight;
    Dash crouchDash;

    [Header("Grappling Gun")]
    [SerializeField] private float maxGrappleDist = 50.0f;
    [SerializeField] private Transform grappleLine;
    [SerializeField] private LayerMask grappleable;
    [SerializeField] private Collider grappleParent;
    [SerializeField] [Range(0,360)] private float normalFOV = 60.0f;
    [SerializeField] [Range(0,360)] private float grappleFOV = 100.0f;
    Grapple grapple;
    Camera playerCamera;
    GrappleFOV fov;
    Vector3 velocityMomentum;

    // Grapple Crosshair.
    [Header("Crosshair Grapple Interaction")]
    [SerializeField] private Transform isGrappleable;
    [SerializeField] private Transform isntGrappleable;
    Crosshair crosshair;

    [Header("Wall Run")]
    [SerializeField] private float minHeight = 4.0f;
    [SerializeField] private float maxWallDistance = 1.4f;
    [SerializeField] private float wallRunForce = 3.0f;
    [SerializeField] private float maxWallRunSpeed = 25.0f;
    [SerializeField] private float maxWallRunAngle = 20.0f;
    [SerializeField] private float sideWallJumpMultiplier = 3.0f;
    [SerializeField] private float rotateSpeedMultiplier = 5.0f;
    [SerializeField] [Range(0,2)] private int jumpRefresh = 0;
    [SerializeField] private LayerMask wallRunable;
    KeyCode right = KeyCode.D;
    KeyCode left = KeyCode.A;
    KeyCode straight = KeyCode.W;
    WallRun wallRun;

    [Header("Boost Pad")]
    [SerializeField] private LayerMask boostPadLayer;
    [SerializeField] private float boostYAmt = 50.0f;
    [SerializeField] private float boostForwardAmt = 10.0f;
    BoostPad boostPad;

    // Called on component startup.
    private void Start()
    {
        this.characterController = GetComponent<CharacterController>();

        // Crouch Height
        origHeight = this.characterController.height;

        // Dash
        dash = new Dash(dashSpeed, dashLength, dashResetTime, maxDashAttempts);
        crouchDash = new Dash(crouchDashSpeed, crouchDashLength, crouchDashResetTime, maxCrouchDashAttempts);

        // Get grapple objects and disable their collision with the player.
        if (grappleParent)
        {
            Collider[] grappleChildren = grappleParent.GetComponentsInChildren<Collider>();
            foreach(Collider obj in grappleChildren)
            {
                Physics.IgnoreCollision(this.GetComponent<Collider>(), obj.GetComponent<Collider>());
            }
            Physics.IgnoreCollision(this.GetComponent<Collider>(), grappleParent);
        }

        wallRun = new WallRun(mainCamera, minHeight, maxWallDistance, wallRunForce, maxWallRunSpeed, maxWallRunAngle, jumpRefresh, wallRunable);

        boostPad = new BoostPad(boostYAmt, boostForwardAmt, boostPadLayer);
    }

    private void Awake()
    {
        mouseLook = new MouseLook(mainCamera, mouseSensitivity, maxWallRunAngle, rotateSpeedMultiplier);
        mouseLook.MouseStart();

        playerCamera = transform.Find("Camera").GetComponent<Camera>();
        fov = playerCamera.GetComponent<GrappleFOV>();
        grappleLine.gameObject.SetActive(false);
        grapple = new Grapple(playerCamera, fov, grappleLine, grappleable, maxGrappleDist);
        crosshair = new Crosshair(playerCamera, maxGrappleDist, grappleable, isGrappleable, isntGrappleable);
    }

    // Update is called once per frame.
    void Update()
    {
        crosshair.checkGrappleableCrosshair();
        wallRun.CheckWalls(transform, ref numJumped);
        mouseLook.HandleMouse(transform, wallRun.IsWallRunning(), wallRun.IsWallRight(), wallRun.IsWallLeft());

        // checks on the grapple state before determining what the player can handle per frame
        switch (grapple.State())
        {
        default:
        case Grapple.grappleState.normal:
        {
            wallRun.HandleWallRun(transform, ref movementVector, groundCheck, groundDistance, groundMask, right, left, straight);
            HandleMovement();
            dash.HandleDash(movementVector, transform, characterController, isGrounded, ref velocity.y);

            boostPad.HandleBoost(transform, characterController.height, ref velocity.y);

            // Uncomment if you wanna have a go at making the crouch dash work.
            //if (Input.GetButtonDown("Crouch"))
            //{
            //    HandleCrouchDown();
            //    crouchDash.HandleDash(movementVector, transform, characterController, isGrounded, ref velocity.y);
            //}
            //if (Input.GetButtonUp("Crouch"))
            //{
            //    HandleCrouchUp();
            //}

            grapple.HandleGrappleStart();
            break;
        }
        case Grapple.grappleState.shoot:
            HandleMovement();
            grapple.HandleGrappleShoot(transform, grappleFOV);
            break;
        case Grapple.grappleState.launch:
            grapple.HandleGrappleLaunch(controller, transform, ref velocity, ref velocityMomentum, jumpHeight, normalFOV);
            break;
        }
    }

    void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) || Physics.CheckSphere(groundCheck.position, groundDistance, wallRunable);

        if (isGrounded && velocity.y < 0.0f)
        {
            velocity.y = 0.0f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        movementVector = Vector3.ClampMagnitude(transform.right * horizontalInput + transform.forward * verticalInput, 1.0f);

        HandleJump();

        // adds momentum from grapple launches (if any)
        movementVector += velocityMomentum;

        controller.Move(movementVector * speed * Time.deltaTime);
        // only add gravity when not wall running
        if (!wallRun.IsWallRunning())
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        // slowing down the momentum
        if (velocityMomentum.magnitude >= 0.0f)
        {
            float dragEffect = 3.0f;
            velocityMomentum -= velocityMomentum * dragEffect * Time.deltaTime;

            if (velocityMomentum.magnitude < 0.0f)
            {
                velocityMomentum = Vector3.zero;
            }
        }
    }

    void HandleJump()
    {
        // first grounded jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            numJumped = 0;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // double jump
        if (Input.GetButtonDown("Jump") && !isGrounded)
        {
            if (++numJumped < numJumps) velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // jump after a wall run
        if (Input.GetButtonDown("Jump") && wallRun.IsWallRunning())
        {
            // regular jump if player not going to the opposite direction of the wall
            if ((!Input.GetKey(right) && wallRun.IsWallRight()) || (!Input.GetKey(left) && wallRun.IsWallLeft()))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            // add some downward force regardless of the direction of the wall jump
            if ((Input.GetKey(right) || Input.GetKey(left)) && (wallRun.IsWallRight() || wallRun.IsWallLeft()))
            {
                velocity += -transform.up * jumpHeight;
            }
            // sideways jump
            if (Input.GetKey(left) && wallRun.IsWallRight())
            {
                movementVector += -transform.right * jumpHeight * sideWallJumpMultiplier;
            }
            if (Input.GetKey(right) && wallRun.IsWallLeft())
            {
                movementVector += transform.right * jumpHeight * sideWallJumpMultiplier;
            }

            // let player have numJumps - 1 remaining jumps left and add some forward force
            numJumped = 1;
            movementVector += transform.forward * jumpHeight;
        }
    }

    void HandleCrouchDown()
    {
        Debug.Log("Reducing Height!");
        var newYpos = this.mainCamera.localPosition;
        newYpos.y = reducedHeight - 0.2f;
        this.mainCamera.localPosition = newYpos;

        this.characterController.height = reducedHeight;
    }

    void HandleCrouchUp()
    {
        Debug.Log("Increasing Height!");
        this.characterController.height = origHeight;
    }
}
