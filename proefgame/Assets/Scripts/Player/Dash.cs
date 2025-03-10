using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Dash : MonoBehaviour
    {
        public float dashSpeed = 14f; // Dash speed
        public float dashDuration = 0.2f; // Duration of the dash
        public float dashCooldown = 1f; // Cooldown between dashes

        private Rigidbody2D rb; // Player's Rigidbody2D
        private bool isDashing; // Whether the player is dashing
        private float dashEndTime; // Time when the dash ends
        private float dashCooldownEndTime; // Time when the dash cooldown ends

        private Vector2 moveInput; // Stores movement input (for omni-directional dash)

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("Rigidbody2D component not found on the player!");
            }
        }

        // Called by the Input System when dash is pressed
        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log("Dash input detected");
                if (Time.time >= dashCooldownEndTime)
                {
                    StartDash(moveInput); // Pass the movement direction for dash
                }
                else
                {
                    Debug.Log("Dash is on cooldown");
                }
            }
        }

        // Called by the Movement script to update movement input
        public void UpdateMoveInput(Vector2 input)
        {
            moveInput = input;
        }

        private void StartDash(Vector2 direction)
        {
            isDashing = true;
            dashEndTime = Time.time + dashDuration;
            dashCooldownEndTime = Time.time + dashCooldown;

            // Normalize the direction to ensure consistent dash speed
            if (direction.magnitude > 0)
            {
                direction.Normalize();
            }
            else
            {
                // Default to right if no direction is pressed
                direction = Vector2.right;
            }

            // Apply dash velocity in the specified direction
            rb.linearVelocity = direction * dashSpeed;
            Debug.Log("Dashing in direction: " + direction + " with velocity: " + rb.linearVelocity);

            // End the dash after the dash duration
            Invoke(nameof(EndDash), dashDuration);
        }

        private void EndDash()
        {
            isDashing = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Reset vertical velocity after dash
            Debug.Log("Dash ended");
        }
    }
}