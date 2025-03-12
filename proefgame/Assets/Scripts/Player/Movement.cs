using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float maxSpeed = 7f; 
        public float acceleration = 50f;
        public float deceleration = 50f;
        public float friction = 10f;

        [Header("Jump Settings")]
        public float jumpForce = 12f; 
        public float gravity = 40f;
        public float coyoteTime = 0.1f; 
        public float jumpBufferTime = 0.1f; 
        public float variableJumpMultiplier = 0.5f; 

        private Vector2 moveInput; 
        private float horizontalVelocity;
        private bool isGrounded;
        private float coyoteTimeCounter;
        private float jumpBufferCounter;
        private bool isJumping;

        private Animator animator; 
        private Rigidbody2D rb; 

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            HandleMovement();
            HandleJump();
            UpdateAnimator();
        }

        private void FixedUpdate()
        {
            ApplyPhysics();
        }

        private void HandleMovement()
        {
            float targetVelocity = moveInput.x * maxSpeed;

            if (Mathf.Abs(moveInput.x) > 0.1f)
            {
                horizontalVelocity = Mathf.MoveTowards(horizontalVelocity, targetVelocity, acceleration * Time.deltaTime);
            }
            else
            {
                horizontalVelocity = Mathf.MoveTowards(horizontalVelocity, 0, deceleration * Time.deltaTime);
            }

            if (isGrounded && Mathf.Abs(moveInput.x) < 0.1f)
            {
                horizontalVelocity = Mathf.MoveTowards(horizontalVelocity, 0, friction * Time.deltaTime);
            }

            rb.linearVelocity = new Vector2(horizontalVelocity, rb.linearVelocity.y);
        }

        private void HandleJump()
        {
            if (isGrounded)
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if (jumpBufferCounter > 0)
            {
                jumpBufferCounter -= Time.deltaTime;
            }

            if (coyoteTimeCounter > 0 && jumpBufferCounter > 0)
            {
                Jump();
            }

            if (isJumping && rb.linearVelocity.y > 0 && !Keyboard.current.spaceKey.isPressed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * variableJumpMultiplier);
            }
        }

        private void Jump()
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            isJumping = true;
            coyoteTimeCounter = 0;
            jumpBufferCounter = 0;
        }

        private void ApplyPhysics()
        {
            if (!isGrounded)
            {
                rb.linearVelocity += Vector2.down * gravity * Time.fixedDeltaTime;
            }
        }

        private void UpdateAnimator()
        {
            animator.SetFloat("xVelocity", Mathf.Abs(horizontalVelocity));
            animator.SetFloat("yVelocity", rb.linearVelocity.y);
            animator.SetBool("isJumping", !isGrounded);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                jumpBufferCounter = jumpBufferTime;
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