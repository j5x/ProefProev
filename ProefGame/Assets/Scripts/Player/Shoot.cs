using UnityEngine;

namespace Player
{
    public class Shoot : MonoBehaviour
    {
        private Transform shootingPoint; // The actual shooting point instance
        public GameObject bulletPrefab; // Assign Bullet prefab
        public Transform markerPrefab; // Assign the marker object in the Inspector
        public float markerDistance = 3f; // Distance from player
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
            UpdateMarkerPosition(); // Update marker after aiming

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

            // Apply snapped rotation to the shooting point (handles the aiming)
            if (shootingPoint != null)
            {
                shootingPoint.rotation = Quaternion.Euler(0, 0, snappedAngle);
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
            if (markerPrefab == null || shootingPoint == null) return;

            // Place the marker at a fixed distance in the current aiming direction
            markerPrefab.position = transform.position + shootingPoint.right * markerDistance;
            markerPrefab.rotation = shootingPoint.rotation;
        }

        private void ShootBullet()
        {
            if (bulletPrefab == null || shootingPoint == null) return;

            // Instantiate the bullet and set its velocity using Bullet.cs logic
            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = shootingPoint.right * 10f; // Assuming Bullet.cs handles bullet speed and damage
            }

            // Trigger shooting animation
            animator.SetTrigger("isShooting");
        }
    }
}
