using UnityEngine;

namespace UI
{
    public class AimMarker : MonoBehaviour
    {
        public Transform player; // Assign the Player Transform
        public Camera mainCamera;
        public float markerOffsetX = 12.5f; // Fixed offset from the player

        private Vector2 aimDirection;
        private SpriteRenderer markerSprite;

        void Start()
        {
            markerSprite = GetComponent<SpriteRenderer>(); // Get sprite renderer for the marker
        }

        void Update()
        {
            HandleMarkerPosition();
        }

        private void HandleMarkerPosition()
        {
            // Get mouse position in world space
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            
            // Calculate direction from player to mouse
            aimDirection = (mousePos - (Vector2)player.position).normalized;

            // Set the marker position at the offset distance along the X-axis
            Vector2 markerPosition = new Vector2(player.position.x + markerOffsetX * Mathf.Sign(aimDirection.x), player.position.y);
            transform.position = markerPosition;

            // Rotate the marker to face the direction of the mouse
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // If aiming to the left, flip the marker (optional)
            markerSprite.flipY = angle > 90 || angle < -90;
        }
    }
}