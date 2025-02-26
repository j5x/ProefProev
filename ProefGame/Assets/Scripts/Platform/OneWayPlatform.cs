using UnityEngine;
using UnityEngine.InputSystem;

namespace Platform
{
    public class OneWayPlatform : MonoBehaviour
    {
        public float disableDuration = 0.5f; // Duration to disable collision
        private Collider2D platformCollider;

        private InputAction moveDownAction;
        private InputAction jumpAction;

        private void Start()
        {
            platformCollider = GetComponent<Collider2D>();

            // Get the input actions from the Input System
            var inputActionAsset = InputSystem.actions;
            if (inputActionAsset == null)
            {
                Debug.LogError("Input Action Asset is not assigned or found.");
                return;
            }

            // Find the "Move" and "Jump" actions
            moveDownAction = inputActionAsset.FindAction("Move");
            jumpAction = inputActionAsset.FindAction("Jump");

            if (moveDownAction == null || jumpAction == null)
            {
                Debug.LogError("Could not find 'Move' or 'Jump' action in Input System. Please ensure the actions are defined in your Input Action Asset.");
                return;
            }

            moveDownAction.Enable();
            jumpAction.Enable();
        }

        private void Update()
        {
            // Check if the "Down" key (S key) was pressed this frame
            if (moveDownAction != null && moveDownAction.ReadValue<Vector2>().y < 0)
            {
                DisableCollision();
            }

            // Check if the "Jump" key was pressed this frame
            if (jumpAction != null && jumpAction.WasPressedThisFrame())
            {
                DisableCollision();
            }
        }

        private void DisableCollision()
        {
            // Disable the platform's collider
            platformCollider.enabled = false;

            // Re-enable the collider after the specified duration
            Invoke(nameof(ReEnableCollider), disableDuration);
        }

        private void ReEnableCollider()
        {
            platformCollider.enabled = true;
        }
    }
}