using System;
using System.Collections;
using System.Collections.Generic;
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
    const float NORMAL_FOV = 60.0f;
    const float GRAPPLE_FOV = 100.0f;
    Grapple grapple;
    Camera playerCamera;
    GrappleFOV fov;
    Vector3 velocityMomentum;
    LineRenderer lineRenderer;
    [SerializeField] private float maxGrappleDist = 50.0f;
    [SerializeField] private Transform grappleLine;
    [SerializeField] private LayerMask grappleable;
    // Grapple Crosshair
    Crosshair crosshair;
    [SerializeField] private Transform isGrappleable;
    [SerializeField] private Transform isntGrappleable;

    // Double Jump
    [SerializeField] private int numJumps = 2;
    private int numJumped;

    // Called on component startup.
    private void Start()
    {
        this.characterController = GetComponent<CharacterController>();
        dash = new Dash(dashSpeed, dashLength, dashResetTime, maxDashAttempts);
    }

    private void Awake()
    {
        playerCamera = transform.Find("Main Camera").GetComponent<Camera>();
        fov = playerCamera.GetComponent<GrappleFOV>();
        grappleLine.gameObject.SetActive(false);
        grapple = new Grapple(playerCamera, fov, grappleLine, grappleable, maxGrappleDist);
        crosshair = new Crosshair(playerCamera, maxGrappleDist, grappleable, isGrappleable, isntGrappleable);
    }

    // Update is called once per frame.
    void Update()
    {
        crosshair.checkGrappleableCrosshair();

        // checks on the grapple state before determining what the player can handle per frame
        switch (grapple.State())
        {
        default:
        case Grapple.grappleState.normal:
            HandleMovement();
            dash.HandleDash(movementVector, transform, characterController, isGrounded, ref velocity.y);
            grapple.HandleGrappleStart();
            break;
        case Grapple.grappleState.shoot:
            HandleMovement();
            grapple.HandleGrappleShoot(transform, GRAPPLE_FOV);
            break;
        case Grapple.grappleState.launch:
            grapple.HandleGrappleLaunch(controller, transform, ref velocity, ref velocityMomentum, jumpHeight, NORMAL_FOV);
            break;
        }
    }

    void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0.0f) velocity.y = -2f;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        movementVector = Vector3.ClampMagnitude(transform.right * horizontalInput + transform.forward * verticalInput, 1.0f);

        HandleJump();

        velocity.y += gravity * Time.deltaTime;

        // adds momentum from grapple launches (if any)
        movementVector += velocityMomentum;

        controller.Move(movementVector * speed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);

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
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            numJumped = 0;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (Input.GetButtonDown("Jump") && !isGrounded)
        {
            if (++numJumped < numJumps) velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
