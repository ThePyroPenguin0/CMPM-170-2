using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;

    [Header("Movement Settings")]
    public GameObject player;
    public int direction = 0;
    public int oldDirection = 0;
    public float doubleJumpTimer = 0.5f;
    // public float upAccel = 0.8f;
    public float downAccel = 3f;
    public float leftAccel = 5f;
    public float rightAccel = 5f;
    public bool onGround = true;
    public bool doubleJumped = false;

    [Header("Physics Settings")]
    public float acceleration = 10f;
    public float maxSpeed = 8f;
    public float friction = 0.9f;

    [Header("Player Options")]
    public Camera mainCamera;

    [Header("Teleport Settings")]
    public Vector2 teleportPosition = new Vector2(-10f, 4f);

    [Header("Jump Bar Settings")]
    public ResourceBar jumpBar;
    public int maxJumpResource = 100;
    public float jumpRechargeRate = 83f;
    private int currentJumpResource;
    private bool canDoubleJump = false;

    [Header("Managerial")]
    public UIManager uiManager;
    public GravityController gravityController;
    public GravityRotator gravityRotator;

    public enum ControlScheme
    {
        WASD = 0,
        IJKL = 1,
        ArrowKeys = 2
    }

    [SerializeField] private ControlScheme selectedControlScheme = ControlScheme.WASD;

    [Header("Key Maps")]
    [SerializeField] private KeyCode upKey;
    [SerializeField] private KeyCode downKey;
    [SerializeField] private KeyCode leftKey;
    [SerializeField] private KeyCode rightKey;

    private KeyCode originalUpKey;
    private KeyCode originalDownKey;
    private KeyCode originalLeftKey;
    private KeyCode originalRightKey;



    private Vector2 movement;
    private Rigidbody2D rb;

    private Vector3 cameraVelocity = Vector3.zero;
    public float cameraSmoothTime = 0.2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        switch (selectedControlScheme)
        {
            case ControlScheme.WASD:
                originalUpKey = KeyCode.W;
                originalDownKey = KeyCode.S;
                originalLeftKey = KeyCode.A;
                originalRightKey = KeyCode.D;
                upKey = originalUpKey;
                downKey = originalDownKey;
                leftKey = originalLeftKey;
                rightKey = originalRightKey;
                break;
            case ControlScheme.IJKL:
                originalUpKey = KeyCode.I;
                originalDownKey = KeyCode.K;
                originalLeftKey = KeyCode.J;
                originalRightKey = KeyCode.L;
                upKey = originalUpKey;
                downKey = originalDownKey;
                leftKey = originalLeftKey;
                rightKey = originalRightKey;
                break;
            case ControlScheme.ArrowKeys:
                originalUpKey = KeyCode.UpArrow;
                originalDownKey = KeyCode.DownArrow;
                originalLeftKey = KeyCode.LeftArrow;
                originalRightKey = KeyCode.RightArrow;
                upKey = originalUpKey;
                downKey = originalDownKey;
                leftKey = originalLeftKey;
                rightKey = originalRightKey;
                break;
            default:
                Debug.LogError("How could you possibly screw up a dropdown menu?");
                break;
        }
    }
    void Start()
    {
        originalUpKey = upKey;
        originalDownKey = downKey;
        originalLeftKey = leftKey;
        originalRightKey = rightKey;
        currentJumpResource = maxJumpResource;
        jumpBar.setMaxResource(maxJumpResource);
        jumpBar.setResource(currentJumpResource);
        animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        movement = GetMappedInput();
        checkForDirectionChange();
        ApplyPhysicsMovement(movement);
        FillJumpBar();

        // Animation parameters 
        float speed = rb.linearVelocity.magnitude;

        if (rb.linearVelocity.x < -0.1f)
        {
            animator.transform.localScale = new Vector3(-1, 1, 1); // Face left
        }
        else if (rb.linearVelocity.x > 0.1f)
        {
            animator.transform.localScale = new Vector3(1, 1, 1); // Face right
        }
        
        // Calculate velocity relative to gravity direction
        Vector2 gravityDir = gravityController.gravityDirection.normalized;
        float velocityAlongGravity = Vector2.Dot(rb.linearVelocity, gravityDir);
        
        bool isWalking = speed > 0.1f && onGround;
        bool isJumping = !onGround && velocityAlongGravity < -0.1f; // Moving against gravity
        bool isFalling = !onGround && velocityAlongGravity > 0.1f;  // Moving with gravity

        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", isFalling);
    }

    void LateUpdate()
    {
        Vector3 targetPosition = transform.position + new Vector3(0, 0, -3);
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref cameraVelocity, cameraSmoothTime);
    }

    private void ApplyPhysicsMovement(Vector2 rawInput)
    {
        if (rawInput.magnitude > 0)
        {
            Vector2 force = rawInput.normalized * acceleration;
            rb.AddForce(force, ForceMode2D.Force);

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }
        else
        {
            rb.linearVelocity *= friction;

            if (rb.linearVelocity.magnitude < 0.1f)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    private Vector2 GetMappedInput()
    {
        Vector2 input = Vector2.zero;

        if (Input.GetKey(upKey))
        {
            if (onGround)
            {
                rb.AddForce(-gravityController.gravityDirection.normalized * 20, ForceMode2D.Impulse);
                canDoubleJump = false;
                onGround = false;
                Invoke("EnableDoubleJump", doubleJumpTimer);
            }
            else if (!onGround && !doubleJumped && canDoubleJump && currentJumpResource >= 100)
            {
                jumpBar.setResource(0);
                currentJumpResource = 0;
                rb.AddForce(-gravityController.gravityDirection.normalized * 15, ForceMode2D.Impulse);
                doubleJumped = true;
                canDoubleJump = false;
                Invoke("ResetDoubleJump", doubleJumpTimer);
            }
        }
        if (Input.GetKey(downKey)) input.y -= downAccel;
        if (Input.GetKey(leftKey)) input.x -= leftAccel;
        if (Input.GetKey(rightKey)) input.x += rightAccel;
        // Debug.Log($"Raw Input: {input}");

        switch (direction)
        {
            case 0: // Original orientation
                input = new Vector2(input.x, input.y);
                break;
            case 1: // +90 degrees
                input = new Vector2(-input.y, input.x);
                break;
            case 2: // +180 degrees
                input = new Vector2(-input.x, -input.y);
                break;
            case 3: // +270 degrees
                input = new Vector2(input.y, -input.x);
                break;
            default:
                Debug.LogError($"How in the Kentucky Fried Fuck did you pass {direction} as the direction?");
                break;
        }

        return input;
    }

    private void directionUpdate()
    {
        switch (direction)
        {
            case 0: // Original orientation
                RemapKeys(originalUpKey, originalDownKey, originalLeftKey, originalRightKey);
                break;
            case 1: // +90 degrees
                RemapKeys(originalLeftKey, originalRightKey, originalDownKey, originalUpKey);
                break;
            case 2: // +180 degrees
                RemapKeys(originalDownKey, originalUpKey, originalRightKey, originalLeftKey);
                break;
            case 3: // +270 degrees
                RemapKeys(originalRightKey, originalLeftKey, originalUpKey, originalDownKey);
                break;
            default:
                Debug.LogError($"How in the Kentucky Fried Fuck did you pass {direction} as the direction?");
                break;
        }
        Debug.Log($"New direction: {direction}, new control scheme: up is {upKey}, down is {downKey}, left is {leftKey}, right is {rightKey}");
    }

    public void RemapKeys(KeyCode newUp, KeyCode newDown, KeyCode newLeft, KeyCode newRight)
    {
        upKey = newUp;
        downKey = newDown;
        leftKey = newLeft;
        rightKey = newRight;
        return;
    }

    public void checkForDirectionChange()
    {
        if(direction != oldDirection)
        {
            oldDirection = direction;
            directionUpdate();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Spike"))
        {
            TeleportPlayer();
            uiManager.ShowRandomText(false);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            onGround = true;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("WinFlag"))
        {
            uiManager.ShowRandomText(true);
            Invoke("TeleportPlayer", 5f);
        }
    }

    private void TeleportPlayer()
    {
        if (teleportPosition != null)
        {
            rb.linearVelocity = Vector2.zero;
            gravityRotator.RotateGravity(0);
            direction = 0;
            directionUpdate();
            transform.position = teleportPosition;
        }
    }

    private void FillJumpBar()
    {
        if (currentJumpResource < maxJumpResource)
        {
            currentJumpResource++;
            jumpBar.setResource(currentJumpResource);
        }
    }

    private void EnableDoubleJump()
    {
        if (!onGround)
        {
            canDoubleJump = true;
        }
    }
    private void ResetDoubleJump()
    {
        doubleJumped = false;
        canDoubleJump = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                onGround = false;
            }
    }
}
