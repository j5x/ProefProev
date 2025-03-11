using UnityEngine;

namespace Player
{
    public class Shoot : MonoBehaviour
    {
        public Transform markerPrefab; // The marker will now act as the gun
        public GameObject bulletPrefab; // Assign Bullet prefab
        public float markerDistance = 3f; // Distance from player for the marker position
        public Camera mainCamera;

        private SpriteRenderer playerSpriteRenderer;
        private Animator animator;

        void Start()
        {
            playerSpriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            HandleAiming();
            UpdateMarkerPosition(); // Update marker position after aiming

            if (Input.GetMouseButtonDown(0)) // Left Mouse Button
            {
                ShootBullet();
            }
        }

        private void HandleAiming()
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 aimDirection = (mousePos - (Vector2)transform.position).normalized;

            // Get raw angle in degrees
            float rawAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            // Snap angle to 8-way increments (each step is 45°)
            float snappedAngle = Mathf.Round(rawAngle / 45f) * 45f;

            // Apply snapped rotation to the marker (marker now acts as gun)
            if (markerPrefab != null)
            {
                markerPrefab.rotation = Quaternion.Euler(0, 0, snappedAngle);
            }

            // Flip player sprite if aiming left (based on the angle)
            bool shouldFlip = snappedAngle > 90 || snappedAngle < -90;
            if (playerSpriteRenderer != null)
            {
                playerSpriteRenderer.flipX = shouldFlip;
            }
        }

        private void UpdateMarkerPosition()
        {
            if (markerPrefab == null) return;

            // Place the marker at a fixed distance in the current aiming direction
            markerPrefab.position = transform.position + markerPrefab.right * markerDistance;
        }

        private void ShootBullet()
        {
            if (bulletPrefab == null || markerPrefab == null) return;

            // Instantiate the bullet and set its velocity using Bullet.cs logic
            GameObject bullet = Instantiate(bulletPrefab, markerPrefab.position, markerPrefab.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = markerPrefab.right * 10f; // Assuming Bullet.cs handles bullet speed and damage
            }

            // Trigger shooting animation
            animator.SetTrigger("isShooting");
        }
    }
}
