using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platform
{
    public class OneWayPlatform : MonoBehaviour
    {
        private PlatformEffector2D effector;
        public float waitTime = 0.5f; // Time to wait before resetting

        private InputAction moveDownAction;
        private Collider2D platformCollider;

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
                StartCoroutine(DisableCollision());
            }
        }

        IEnumerator DisableCollision()
        {
            // Temporarily disable collision with the player
            effector.rotationalOffset = 180f; // Flip the effector to allow passing through
            platformCollider.enabled = false; // Disable the collider entirely

            yield return new WaitForSeconds(waitTime);

            // Re-enable collision with the player
            platformCollider.enabled = true; // Re-enable the collider
            effector.rotationalOffset = 0f; // Reset the effector to its original state
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