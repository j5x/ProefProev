using UnityEngine;
using UnityEngine.InputSystem;

namespace Platform
{
    public class OneWayPlatform : MonoBehaviour
    {
        private PlatformEffector2D effector;
        private Collider2D platformCollider;

        private InputAction moveDownAction;

        void Start()
        {
            effector = GetComponent<PlatformEffector2D>();
            platformCollider = GetComponent<Collider2D>();

            // Get the "Move" action from the Input System
            var inputActionAsset = InputSystem.actions;
            if (inputActionAsset == null)
            {
                Debug.LogError("Input Action Asset is not assigned or found.");
                return;
            }

            // Find the "Move" action
            moveDownAction = inputActionAsset.FindAction("Move");
            if (moveDownAction == null)
            {
                Debug.LogError("Could not find 'Move' action in Input System. Please ensure the action is defined in your Input Action Asset.");
                return;
            }

            moveDownAction.Enable();
        }

        void Update()
        {
            // Check if the "Down" key (S key) was pressed this frame
            if (moveDownAction != null && moveDownAction.ReadValue<Vector2>().y < 0)
            {
                // Temporarily disable collision to allow the player to drop down
                platformCollider.enabled = false;
                Invoke(nameof(ReenableCollision), 0.5f); // Re-enable collision after a short delay
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            // Check if the colliding object is the player
            if (collision.CompareTag("Player"))
            {
                // Check if the player is below the platform
                if (collision.transform.position.y < transform.position.y)
                {
                    // Disable the platform collider to allow the player to jump through
                    platformCollider.enabled = false;
                }
                else
                {
                    // Re-enable the platform collider if the player is above
                    platformCollider.enabled = true;
                }
            }
        }

        private void ReenableCollision()
        {
            // Re-enable the platform collider after dropping down
            platformCollider.enabled = true;
        }

        private void OnDestroy()
        {
            // Disable the input action when the object is destroyed
            if (moveDownAction != null)
            {
                moveDownAction.Disable();
            }
        }
    }
}