using UnityEngine;

namespace Player
{
    public class Shoot : MonoBehaviour
    {
        public Transform gun; // Assign Gun Transform (child of player hand)
        public GameObject shootingPointPrefab; // Assign ShootingPoint prefab
        private Transform shootingPoint; // The actual shooting point instance
        public GameObject bulletPrefab; // Assign Bullet prefab
        public float bulletSpeed = 10f;
        public Camera mainCamera;

        private SpriteRenderer playerSpriteRenderer;
        private Animator animator;

        void Start()
        {
            playerSpriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            // Instantiate Shooting Point at the gun's position
            if (shootingPointPrefab != null)
            {
                shootingPoint = Instantiate(shootingPointPrefab, gun.position, gun.rotation).transform;
                shootingPoint.SetParent(gun);
            }
        }

        void Update()
        {
            HandleAiming();

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

            // Apply snapped rotation to the gun
            gun.rotation = Quaternion.Euler(0, 0, snappedAngle);

            // Flip player sprite if aiming left
            bool shouldFlip = snappedAngle > 90 || snappedAngle < -90;
            if (playerSpriteRenderer != null)
            {
                playerSpriteRenderer.flipX = shouldFlip;
            }

            // Keep shooting point rotation aligned with gun
            if (shootingPoint != null)
            {
                shootingPoint.rotation = gun.rotation;
            }
        }

        private void ShootBullet()
        {
            if (bulletPrefab == null || shootingPoint == null) return;

            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = shootingPoint.right * bulletSpeed;
            }

            // Trigger shooting animation
            animator.SetTrigger("isShooting");
        }
    }
}
    