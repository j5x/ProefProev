using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        public float maxSpeed = 7f; // Maximum movement speed
        public float jumpTakeOffSpeed = 7f; // Jump force

        private Rigidbody2D rb; // Player's Rigidbody2D
        private Animator animator; // Animator reference
        private bool isGrounded; // Whether the player is on the ground

        private Transform currentPlatform; // Reference to the platform the player is standing on
        private Vector2 moveInput; // Stores movement input
        private Dash dash; // Reference to the Dash script

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>(); // Get Animator component
            dash = GetComponent<Dash>(); // Get the Dash component
        }

        private void Update()
        {
            // Apply movement
            Vector2 targetVelocity = new Vector2(moveInput.x * maxSpeed, rb.linearVelocity.y);
            rb.linearVelocity = targetVelocity;

            // Update the Dash script with the current movement input
            dash.UpdateMoveInput(moveInput);

            // Update animator parameters
            animator.SetBool("isJumping", !isGrounded);
            animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
            animator.SetFloat("yVelocity", rb.linearVelocity.y);
        }

        // Called by the Input System when movement keys are pressed
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>(); // Get 2D movement input (WASD or arrow keys)
        }

        // Called by the Input System when jump is pressed
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed && isGrounded)
            {
                Jump();
            }
        }

        private void Jump()
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpTakeOffSpeed);
            isGrounded = false; // Player is no longer grounded after jumping
            animator.SetBool("isJumping", true); // Update animation
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Check if the player is standing on a platform
            if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("MovingPlatform"))
            {
                isGrounded = true;
                animator.SetBool("isJumping", false); // Update animation

                // Parent the player to the platform
                currentPlatform = collision.transform;
                transform.SetParent(currentPlatform);
                Debug.Log("Landed on platform: " + currentPlatform.name);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            // Check if the player leaves the platform
            if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("MovingPlatform"))
            {
                isGrounded = false;
                animator.SetBool("isJumping", true); // Update animation

                // Unparent the player from the platform
                transform.SetParent(null);
                currentPlatform = null;
                Debug.Log("Left platform");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Handle trigger-based grounding (optional)
            if (collision.CompareTag("Ground"))
            {
                isGrounded = true;
                animator.SetBool("isJumping", false);
            }
        }
    }
}
