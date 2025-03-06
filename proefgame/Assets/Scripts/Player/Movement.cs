using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        public float maxSpeed = 7; // Maximum movement speed
        public float jumpTakeOffSpeed = 7; // Jump force
        public float dashSpeed = 14; // Dash speed
        public float dashDuration = 0.2f; // Duration of the dash
        public float dashCooldown = 1f; // Cooldown between dashes

        private Rigidbody2D rb;
        private bool isGrounded;
        private bool isDashing;
        private float dashEndTime;
        private float dashCooldownEndTime;
        private Transform currentPlatform;
        private SpriteRenderer spriteRenderer; // Added sprite renderer reference

        private InputAction m_MoveAction;
        private InputAction m_JumpAction;
        private InputAction m_DashAction;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component

            m_MoveAction = InputSystem.actions.FindAction("Player/Move");
            m_JumpAction = InputSystem.actions.FindAction("Player/Jump");
            m_DashAction = InputSystem.actions.FindAction("Player/Dash");

            if (m_MoveAction == null || m_JumpAction == null || m_DashAction == null)
            {
                Debug.LogError("One or more input actions are not found. Please check your Input Action Asset.");
            }

            m_MoveAction.Enable();
            m_JumpAction.Enable();
            m_DashAction.Enable();
        }

        private void Update()
        {
            Vector2 moveInput = m_MoveAction.ReadValue<Vector2>();

            // Flip sprite based on movement direction
            if (moveInput.x > 0.01f)
                spriteRenderer.flipX = true;
            else if (moveInput.x < -0.01f)
                spriteRenderer.flipX = false;

            if (isGrounded && m_JumpAction.WasPressedThisFrame())
            {
                Jump();
            }

            if (m_DashAction.WasPressedThisFrame() && Time.time >= dashCooldownEndTime)
            {
                StartDash(moveInput);
            }

            if (!isDashing)
            {
                Vector2 targetVelocity = new Vector2(moveInput.x * maxSpeed, rb.linearVelocity.y);
                rb.linearVelocity = targetVelocity;
            }
        }

        private void Jump()
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpTakeOffSpeed);
            isGrounded = false;
        }

        private void StartDash(Vector2 direction)
        {
            isDashing = true;
            dashEndTime = Time.time + dashDuration;
            dashCooldownEndTime = Time.time + dashCooldown;

            if (direction.magnitude > 0)
                direction.Normalize();
            else
                direction = Vector2.right;

            rb.linearVelocity = direction * dashSpeed;
            Invoke(nameof(EndDash), dashDuration);
        }

        private void EndDash()
        {
            isDashing = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("MovingPlatform"))
            {
                isGrounded = true;
                currentPlatform = collision.transform;
                transform.SetParent(currentPlatform);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("MovingPlatform"))
            {
                isGrounded = false;
                transform.SetParent(null);
                currentPlatform = null;
            }
        }
    }
}
