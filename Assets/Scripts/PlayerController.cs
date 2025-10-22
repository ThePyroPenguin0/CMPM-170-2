using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public GameObject player;
    public int direction = 0;
    public int oldDirection = 0;
    public float doubleJumpTimer = 0.8f;
    public float jumpForce = 7f;
    private bool doubleJumped = false;
    public float moveSpeed = 5f;

    [Header("Physics Settings")]
    public float acceleration = 10f;
    public float maxSpeed = 8f;
    public float friction = 0.9f;

    // [Header("Player Options")]
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

    void Awake()
    {
        // Get or add Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

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
    }

    void FixedUpdate()
    {
        if (oldDirection != direction)
        {
            directionUpdate();
            oldDirection = direction;
        }
        movement = GetMappedInput();
        checkForDirectionChange();
        ApplyPhysicsMovement();
    }

    private void ApplyPhysicsMovement()
    {
        if (movement.magnitude > 0)
        {
            Vector2 force = movement.normalized * acceleration;
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

        if (Input.GetKey(upKey)) input.y += 1f;
        if (Input.GetKey(downKey)) input.y -= 1f;
        if (Input.GetKey(leftKey)) input.x -= 1f;
        if (Input.GetKey(rightKey)) input.x += 1f;
        Debug.Log($"Raw Input: {input}");

        switch (direction)
        {
            case 0: // Original orientation
                input = new Vector2(input.x, input.y);
                break;
            case 1: // +90 degrees
                input = new Vector2(input.y, -input.x);
                break;
            case 2: // +180 degrees
                input = new Vector2(-input.x, -input.y);
                break;
            case 3: // +270 degrees
                input = new Vector2(-input.y, input.x);
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
    }

    public void RemapKeys(KeyCode newUp, KeyCode newDown, KeyCode newLeft, KeyCode newRight)
    {
        upKey = newUp;
        downKey = newDown;
        leftKey = newLeft;
        rightKey = newRight;
        Debug.Log($"Keys remapped: Up: {newUp}, Down: {newDown}, Left: {newLeft}, Right: {newRight}");
    }

    public void checkForDirectionChange()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            direction = (direction + 1) % 4;
        }
    }
}
