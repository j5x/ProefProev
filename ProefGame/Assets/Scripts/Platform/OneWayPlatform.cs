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

        void Start()
        {
            effector = GetComponent<PlatformEffector2D>();

            // Find the "Down" action in the Input Action Asset
            moveDownAction = InputSystem.actions.FindAction("Move/WASD/Down");
            if (moveDownAction == null)
            {
                Debug.LogError("Could not find 'Player/Down' action in Input System. Please ensure the action is defined in your Input Action Asset.");
                return;
            }

            moveDownAction.Enable();
        }

        void Update()
        {
            // Check if the "Down" action was pressed this frame
            if (moveDownAction != null && moveDownAction.WasPressedThisFrame())
            {
                StartCoroutine(DisableCollision());
            }
        }

        IEnumerator DisableCollision()
        {
            // Disable collision with the player layer
            effector.colliderMask &= ~(1 << LayerMask.NameToLayer("Player"));
            yield return new WaitForSeconds(waitTime);

            // Re-enable collision with the player layer
            effector.colliderMask |= (1 << LayerMask.NameToLayer("Player"));
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