using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float maxSpeed = 7; // Maximum movement speed
    public float jumpTakeOffSpeed = 7; // Jump force
    public float dashSpeed = 14; // Dash speed
    public float dashDuration = 0.2f; // Duration of the dash
    public float dashCooldown = 1f; // Cooldown between dashes

    private Rigidbody2D rb; // Player's Rigidbody2D
    private bool isGrounded; // Whether the player is on the ground
    private bool isDashing; // Whether the player is dashing
    private float dashEndTime; // Time when the dash ends
    private float dashCooldownEndTime; // Time when the dash cooldown ends

    private MovingPlatform currentPlatform; // Reference to the platform the player is standing on

    private InputAction m_MoveAction;
    private InputAction m_JumpAction;
    private InputAction m_DashAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set up input actions (using your previous setup)
        m_MoveAction = InputSystem.actions.FindAction("Player/Move");
        m_JumpAction = InputSystem.actions.FindAction("Player/Jump");
        m_DashAction = InputSystem.actions.FindAction("Player/Dash");

        m_MoveAction.Enable();
        m_JumpAction.Enable();
        m_DashAction.Enable();
    }

    private void Update()
    {
        // Handle movement input
        float moveInput = m_MoveAction.ReadValue<Vector2>().x;

        // Handle jump input
        if (isGrounded && m_JumpAction.WasPressedThisFrame())
        {
            Jump();
        }

        // Handle dash input
        if (m_DashAction.WasPressedThisFrame() && Time.time >= dashCooldownEndTime)
        {
            StartDash();
        }

        // Apply movement
        Vector2 targetVelocity = new Vector2(moveInput * maxSpeed, rb.linearVelocity.y);

        // Apply platform velocity if on a platform
        if (currentPlatform != null)
        {
            targetVelocity += currentPlatform.PlatformVelocity / Time.deltaTime; // Ensure velocity is in units per second
            Debug.Log("Platform Velocity: " + currentPlatform.PlatformVelocity);
        }

        rb.linearVelocity = targetVelocity;
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpTakeOffSpeed);
        isGrounded = false; // Player is no longer grounded after jumping
    }

    private void StartDash()
    {
        isDashing = true;
        dashEndTime = Time.time + dashDuration;
        dashCooldownEndTime = Time.time + dashCooldown;

        // Apply dash velocity
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0) + (Vector2)transform.right * dashSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is standing on a moving platform or ground
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("MovingPlatform"))
        {
            isGrounded = true;
            if (collision.gameObject.CompareTag("Platform"))
            {
                currentPlatform = collision.gameObject.GetComponent<MovingPlatform>();
                Debug.Log("Landed on platform: " + currentPlatform.name);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player leaves the moving platform or ground
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("MovingPlatform"))
        {
            isGrounded = false;
            if (collision.gameObject.CompareTag("Platform"))
            {
                currentPlatform = null;
                Debug.Log("Left platform");
            }
        }
    }
}