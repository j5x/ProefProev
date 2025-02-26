using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Shoot : MonoBehaviour
    {
        public Transform shootingPoint;
        public GameObject bulletPrefab;
        public float bulletSpeed = 10f;
        public float shootingOffset = 1.25f; // Distance from player

        private Vector2 aimDirection;
        private bool isMovingRight = true; // Used for flipping
        private bool allowDiagonalShooting = false; // Toggled via modifier key

        private void Update()
        {
            HandleAiming();

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                ShootBullet();
            }
        }

        private void HandleAiming()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 playerPos = transform.position;

            // Calculate direction from player to mouse
            Vector2 rawDirection = (worldMousePos - playerPos).normalized;

            // Check if modifier key is pressed for diagonal shooting
            allowDiagonalShooting = Keyboard.current.leftShiftKey.isPressed;

            // Snap aiming to allowed angles
            float angle = Mathf.Atan2(rawDirection.y, rawDirection.x) * Mathf.Rad2Deg;
            angle = SnapToAllowedAngle(angle);

            // Convert snapped angle back to a unit direction vector
            aimDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            // Flip the sprite instead of scaling the transform
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = aimDirection.x < 0;
            }

            // Move the shooting point to the offset position
            shootingPoint.position = (Vector2)transform.position + (aimDirection * shootingOffset);
            shootingPoint.rotation = Quaternion.Euler(0, 0, angle);
        }

        private float SnapToAllowedAngle(float angle)
        {
            // Normalize angle to [0, 360)
            angle = (angle + 360) % 360;

            // Define allowed angles
            float[] angles = allowDiagonalShooting
                ? new float[] { 0, 45, 90, 135, 180, 225, 270, 315 } // 8-way if Shift is held
                : new float[] { 0, 90, 180, 270 }; // 4-way otherwise

            // Find the closest angle
            float closestAngle = angles[0];
            float minDifference = Mathf.Abs(angle - closestAngle);

            foreach (float targetAngle in angles)
            {
                float diff = Mathf.Abs(angle - targetAngle);
                if (diff < minDifference)
                {
                    minDifference = diff;
                    closestAngle = targetAngle;
                }
            }
            return closestAngle;
        }

        private void ShootBullet()
        {
            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = aimDirection * bulletSpeed;
            }
        }

        // Draw debug arrow in the Scene view
        private void OnDrawGizmos()
        {
            if (shootingPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, shootingPoint.position);
                Gizmos.DrawSphere(shootingPoint.position, 0.1f);
            }
        }
    }
}
