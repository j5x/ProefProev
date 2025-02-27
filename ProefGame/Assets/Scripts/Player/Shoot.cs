using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Shoot : MonoBehaviour
    {
        public Transform shootingPoint;
        public GameObject bulletPrefab;
        public GameObject aimMarkerPrefab;
        public float bulletSpeed = 10f;
        public float shootingOffset = 1.25f; // Distance from player

        private GameObject aimMarker;
        private Vector2 aimDirection;

        private void Start()
        {
            // Instantiate the aiming marker
            if (aimMarkerPrefab != null)
            {
                aimMarker = Instantiate(aimMarkerPrefab);
            }
        }

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

            // Calculate aim direction
            aimDirection = (worldMousePos - playerPos).normalized;

            // Move the shooting point
            shootingPoint.position = playerPos + (aimDirection * shootingOffset);
            shootingPoint.right = aimDirection; // Rotate shooting point to face aim direction

            // Move and rotate the aim marker
            if (aimMarker != null)
            {
                aimMarker.transform.position = shootingPoint.position;
                aimMarker.transform.rotation = shootingPoint.rotation;
            }

            // Flip the player's sprite based on movement direction
            SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
            if (playerSprite != null)
            {
                playerSprite.flipX = aimDirection.x < 0;
            }
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
