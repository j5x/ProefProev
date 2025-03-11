using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        public float moveSpeed = 7f; // Speed of movement
        public float jumpForce = 12f; // Jump force
        public float gravity = 40f; // Custom gravity

        private Vector2 moveInput; // Stores movement input
        private Dash dash; // Reference to dash script
        private bool isGrounded;
        
        private Animator animator; // Animator reference
        private Rigidbody2D rb; // Reference to Rigidbody2D for movement

        private void Awake()
        {
            dash = GetComponent<Dash>(); // Get the Dash component
            animator = GetComponent<Animator>(); // Get the Animator component
            rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        }

        private void Update()
        {
            if (!dash) return;

            // Update Animator parameters based on movement and jump state
            animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x)); // Update xVelocity
            animator.SetFloat("yVelocity", rb.linearVelocity.y); // Update yVelocity
            animator.SetBool("isJumping", !isGrounded); // Set isJumping based on ground status
        }

        private void FixedUpdate()
        {
            // Apply movement: horizontal velocity only, as vertical is managed by physics (gravity & jumping)
            Vector2 movement = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y); 
            rb.linearVelocity = movement;

            // Update Animator for xVelocity and yVelocity
            animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x)); // Update xVelocity
            animator.SetFloat("yVelocity", rb.linearVelocity.y); // Update yVelocity
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed && isGrounded)
            {
                Jump();
            }
        }

        private void Jump()
        {
            // Apply the jump force directly to the Rigidbody2D's y velocity
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Set the vertical velocity for the jump
            isGrounded = false; // Player is no longer grounded after jumping
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                isGrounded = true; // Player is grounded when they touch a platform
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                isGrounded = false; // Player is no longer grounded after leaving the platform
            }
        }
    }
}
