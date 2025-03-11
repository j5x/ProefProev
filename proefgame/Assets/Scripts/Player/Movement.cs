using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float maxSpeed = 7f; // Maximum horizontal movement speed
        public float acceleration = 50f; // How quickly the player reaches max speed
        public float deceleration = 50f; // How quickly the player stops
        public float friction = 10f; // Friction when on the ground

        [Header("Jump Settings")]
        public float jumpForce = 12f; // Initial jump force
        public float gravity = 40f; // Gravity when falling
        public float coyoteTime = 0.1f; // Time after leaving a platform where the player can still jump
        public float jumpBufferTime = 0.1f; // Time before landing where a jump input is buffered
        public float variableJumpMultiplier = 0.5f; // How much holding jump increases jump height

        [Header("Dash Settings")]
        public float dashDistance = 5f; // Distance of the dash
        public float dashCooldown = 1f; // Cooldown between dashes

        private Vector2 moveInput; // Stores movement input
        private float horizontalVelocity; // Current horizontal velocity
        private bool isGrounded; // Whether the player is on the ground
        private float coyoteTimeCounter; // Counts down coyote time
        private float jumpBufferCounter; // Counts down jump buffer time
        private bool isJumping; // Whether the player is currently jumping
        private bool isDashing; // Whether the player is currently dashing
        private float dashCooldownEndTime; // Time when dash is available again
        private Vector2 lastDirection = Vector2.right; // Default dash direction

        private Animator animator; // Animator reference
        private Rigidbody2D rb; // Reference to Rigidbody2D for physics

        private void Awake()
        {
            animator = GetComponent<Animator>(); // Get the Animator component
            rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        }

        private void Update()
        {
            HandleMovement();
            HandleJump();
            HandleDash();
            UpdateAnimator();
        }

        private void FixedUpdate()
        {
            ApplyPhysics();
        }

        private void HandleMovement()
        {
            // Calculate target velocity based on input
            float targetVelocity = moveInput.x * maxSpeed;

            // Apply acceleration or deceleration
            if (Mathf.Abs(moveInput.x) > 0.1f)
            {
                horizontalVelocity = Mathf.MoveTowards(horizontalVelocity, targetVelocity, acceleration * Time.deltaTime);
            }
            else
            {
                horizontalVelocity = Mathf.MoveTowards(horizontalVelocity, 0, deceleration * Time.deltaTime);
            }

            // Apply friction when on the ground
            if (isGrounded && Mathf.Abs(moveInput.x) < 0.1f)
            {
                horizontalVelocity = Mathf.MoveTowards(horizontalVelocity, 0, friction * Time.deltaTime);
            }

            // Update the Rigidbody2D's velocity
            rb.linearVelocity = new Vector2(horizontalVelocity, rb.linearVelocity.y);
        }

        private void HandleJump()
        {
            // Update coyote time
            if (isGrounded)
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            // Update jump buffer
            if (jumpBufferCounter > 0)
            {
                jumpBufferCounter -= Time.deltaTime;
            }

            // Jump if coyote time or jump buffer is active
            if (coyoteTimeCounter > 0 && jumpBufferCounter > 0)
            {
                Jump();
            }

            // Variable jump height
            if (isJumping && rb.linearVelocity.y > 0 && !Input.GetButton("Jump")) // Replace with your jump input
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * variableJumpMultiplier);
            }
        }

        private void Jump()
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            isJumping = true;
            coyoteTimeCounter = 0; // Reset coyote time
            jumpBufferCounter = 0; // Reset jump buffer
        }

        private void HandleDash()
        {
            if (isDashing) return;

            // Update last movement direction
            if (Mathf.Abs(moveInput.x) > 0.1f)
            {
                lastDirection = new Vector2(moveInput.x, 0).normalized;
            }

            // Dash if cooldown is over
            if (Time.time >= dashCooldownEndTime && Input.GetButtonDown("Dash")) // Replace with your dash input
            {
                Dash();
            }
        }

        private void Dash()
        {
            isDashing = true;
            rb.linearVelocity = lastDirection * dashDistance;
            dashCooldownEndTime = Time.time + dashCooldown;
            Invoke(nameof(EndDash), 0.2f); // End dash after a short duration
        }

        private void EndDash()
        {
            isDashing = false;
        }

        private void ApplyPhysics()
        {
            // Apply gravity
            if (!isGrounded)
            {
                rb.linearVelocity += Vector2.down * gravity * Time.fixedDeltaTime;
            }
        }

        private void UpdateAnimator()
        {
            animator.SetFloat("xVelocity", Mathf.Abs(horizontalVelocity)); // Update xVelocity
            animator.SetFloat("yVelocity", rb.linearVelocity.y); // Update yVelocity
            animator.SetBool("isJumping", !isGrounded); // Set isJumping based on ground status
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                jumpBufferCounter = jumpBufferTime; // Start jump buffer
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed && Time.time >= dashCooldownEndTime)
            {
                Dash();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                isGrounded = true;
                isJumping = false;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                isGrounded = false;
            }
        }
    }
}