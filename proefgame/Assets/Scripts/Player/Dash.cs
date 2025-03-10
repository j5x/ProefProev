using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Dash : MonoBehaviour
    {
        public float dashSpeed = 14f; // Dash speed
        public float dashDuration = 0.2f; // Duration of the dash
        public float dashCooldown = 0.5f; // Cooldown before dashing again

        private bool isDashing = false;
        private float dashEndTime;
        private float dashCooldownEndTime;
        private Vector2 lastMoveInput = Vector2.right; // Stores last movement input
        private Vector2 dashDirection;

        private void Update()
        {
            if (isDashing)
            {
                transform.position += (Vector3)(dashDirection * dashSpeed * Time.deltaTime);

                if (Time.time >= dashEndTime)
                {
                    isDashing = false;
                }
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed && Time.time >= dashCooldownEndTime)
            {
                StartDash();
            }
        }

        public void UpdateMoveInput(Vector2 input)
        {
            if (input.sqrMagnitude > 0) // If input is not zero, update lastMoveInput
            {
                lastMoveInput = input.normalized;
            }
        }

        private void StartDash()
        {
            isDashing = true;
            dashEndTime = Time.time + dashDuration;
            dashCooldownEndTime = Time.time + dashCooldown;

            // Use the last movement input as dash direction
            dashDirection = lastMoveInput;

            Debug.Log("Dashing in direction: " + dashDirection);
        }
    }
}