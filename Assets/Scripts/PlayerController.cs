using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Required Objects")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform grappleLine;
    [SerializeField] private LayerMask grappleable;
    [SerializeField] private Collider grappleParent;
    [SerializeField] private LevelSelector ls; // TODO: This probably shouldn't be here. Put into a menu class or something.

    [Header("Audio")]
    [SerializeField] private string runningname;
    [SerializeField] private string grapplename;
    private AudioManager am;

    [Header("Player Settings")]
    [SerializeField] private Transform mainCamera;
    // Getting the default sensitivity from PlayerPrefs.
    private float mouseSensitivity = 150.0f;
    MouseLook mouseLook;

    [Header("Movement")]
    [SerializeField] private float speed = 12.0f;
    [SerializeField] private float gravity = -19.62f;
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float groundDistance = 0.4f;
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
    [SerializeField] private Image arrow1;
    [SerializeField] private Image arrow2;
    [SerializeField] private Image arrow3;
    [SerializeField] private Image dash1;
    [SerializeField] private Image dash2;
    DashUI dashUI;
    CharacterController characterController;
    Dash dash;

    [Header("Crouch Dash")]
    [SerializeField] private float crouchDashSpeed = 50.0f;
    [SerializeField] private float crouchDashLength = 0.2f;
    [SerializeField] private float crouchDashResetTime = 0.0f;
    [SerializeField] private int maxCrouchDashAttempts = 1;
    //[SerializeField] private float reducedHeight = 0.7f;
    private float origHeight;
    Dash crouchDash;

    [Header("Grappling Gun")]
    [SerializeField] private float maxGrappleDist = 50.0f;
    [SerializeField] private float dragEffect = 3.0f;
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
    [SerializeField] private float rotateSpeedMultiplier = 4.0f;
    [SerializeField] [Range(0,1)] private int jumpRefresh = 0;
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

    // Pause Menu
    private bool isPaused = false;

    // Called on component startup.
    private void Start()
    {
        this.characterController = GetComponent<CharacterController>();


        // Crouch Height
        origHeight = this.characterController.height;

        // Dash
        dash = new Dash(dashSpeed, dashLength, dashResetTime, maxDashAttempts);
        crouchDash = new Dash(crouchDashSpeed, crouchDashLength, crouchDashResetTime, maxCrouchDashAttempts);
        dashUI = new DashUI(dash, arrow1, arrow2, arrow3, dash1, dash2);

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
        // Audio
        am = gameObject.GetComponent<AudioManager>();

        // Mouse Sensitivity.
        mouseSensitivity = PlayerPrefs.GetFloat(PlayerPrefsKeys.sensitivitySliderKey, PlayerPrefsKeys.sensitivitySliderKeyDefaultValue);
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
        wallRun.CheckWalls(transform, ref numJumped, wallRun.IsWallRunning());
        mouseLook.HandleMouse(transform, wallRun.IsWallRunning(), wallRun.IsWallRight(), wallRun.IsWallLeft());

        // checks on the grapple state before determining what the player can handle per frame
        if (!isPaused)
        {
            switch (grapple.State())
            {
            default:
            case Grapple.grappleState.normal:
            {
                wallRun.HandleWallRun(transform, ref movementVector, groundCheck, groundDistance, groundMask, right, left, straight);
                HandleMovement();
                dash.HandleDash(movementVector, transform, characterController, ref velocity.y);

                boostPad.HandleBoost(transform, characterController.height, ref velocity.y);

                if (grapple.HandleGrappleStart()) am.Play(grapplename); // TODO: this doesn't account for multiple shots?
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

        dashUI.HandleDashUI();
    }

    void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) || Physics.CheckSphere(groundCheck.position, groundDistance, wallRunable);

        PlayWalkSound(); // Handles the playing and pausing of walking movement.

        if (isGrounded)
        {
            numJumped = 0;
            if (velocity.y < 0.0f)
            {
                velocity.y = 0.0f;
            }
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
            numJumped = 1;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // double jump
        if (Input.GetButtonDown("Jump") && !isGrounded)
        {
            if (++numJumped <= numJumps) velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // jump after a wall run
        if (Input.GetButtonDown("Jump") && wallRun.IsWallRunning())
        {
            wallRun.StopWallRun();

            // add some downward force regardless of the direction of the wall jump
            if ((Input.GetKey(right) || Input.GetKey(left)) && (wallRun.IsWallRight() || wallRun.IsWallLeft()))
            {
                velocity += -transform.up * jumpHeight;
            }
            // sideways jump
            if (wallRun.IsWallRight())
            {
                movementVector += -transform.right * jumpHeight * sideWallJumpMultiplier;
            }
            if (wallRun.IsWallLeft())
            {
                movementVector += transform.right * jumpHeight * sideWallJumpMultiplier;
            }

            // let player have numJumps - 1 remaining jumps left and add some forward force
            numJumped = 1;
            movementVector += transform.forward * jumpHeight;
        }
    }

    public void PlayWalkSound()
    {
        // This plays (at a random point) and stops the running audio when moving on a ground.
        am.SetVolume(runningname, 1.0f);
        if ((isGrounded || wallRun.IsWallRunning()) && movementVector.magnitude > 0.1f && !isPaused) // I dunno why the epsilon has to be so high.
        {
            float delay = Random.Range(0.0f, 5.0f);
            if (!am.isPlaying(runningname)) am.PlayRandom(runningname);
        }
        else
        {
            am.Stop(runningname);
        }
    }

    public void SetPausedStatus(bool isPaused)
    {
        this.isPaused = isPaused;
    }

    public void SetSensitivity(float mouseSensitivity)
    {
        mouseLook.SetSensitivity(mouseSensitivity);
    }
}
