using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 7f; // Speed of movement
    public float jumpForce = 12f; // Jump force
    public float gravity = 40f; // Custom gravity

    private Vector2 moveInput; // Stores movement input
    private Dash dash; // Reference to dash script
    private bool isGrounded;
    private float verticalVelocity = 0f; // Manages gravity and jumping

    private void Awake()
    {
        dash = GetComponent<Dash>(); // Get the Dash component
    }

    private void Update()
    {
        if (!dash) return;

        // Apply movement
        Vector2 movement = new Vector2(moveInput.x * moveSpeed, verticalVelocity);
        transform.position += (Vector3)(movement * Time.deltaTime);

        // Apply gravity if not grounded
        if (!isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // Update dash with movement input
        dash.UpdateMoveInput(moveInput);
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
        verticalVelocity = jumpForce;
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            verticalVelocity = 0; // Reset gravity when landing
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