using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Dash : MonoBehaviour
    {
        public float dashDistance = 5f;
        public float dashCooldown = 0.3f;

        private Vector2 lastDirection = Vector2.right;
        private float dashCooldownEndTime;

        private void Update()
        {
            if (Time.time < dashCooldownEndTime)
            {
                Debug.Log("Dash on cooldown");
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed && Time.time >= dashCooldownEndTime)
            {
                DashMove();
            }
        }

        public void UpdateMoveInput(Vector2 input)
        {
            if (input.sqrMagnitude > 0.1f) 
            {
                lastDirection = input.normalized;
            }
        }

        private void DashMove()
        {
            transform.position += (Vector3)(lastDirection * dashDistance);
            dashCooldownEndTime = Time.time + dashCooldown;
            Debug.Log("Dashed in direction: " + lastDirection);
        }
    }
}