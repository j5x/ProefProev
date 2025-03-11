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
            // Get player sprite renderer to check for flipping
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

            // Super Attack (Commented out for now)
            /*
            if (Input.GetMouseButtonDown(1)) // Right Mouse Button (for Super Attack)
            {
                ShootSuperAttack();
            }
            */
        }

        private void HandleAiming()
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 aimDirection = (mousePos - (Vector2)gun.position).normalized;

            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            gun.rotation = Quaternion.Euler(0, 0, angle);

            // Determine if the player should flip
            bool shouldFlip = angle > 90 || angle < -90;
            
            // Flip gun sprite based on aiming direction
            gun.GetComponent<SpriteRenderer>().flipY = shouldFlip;

            // Flip player sprite if necessary
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

        // Super Attack function (Commented out)
        /*
        private void ShootSuperAttack()
        {
            if (superAttackPrefab == null || shootingPoint == null) return;

            GameObject superAttack = Instantiate(superAttackPrefab, shootingPoint.position, shootingPoint.rotation);
            superAttack.GetComponent<SuperAttack>().PlayAnimationAndDestroy();

            // Trigger shooting animation
            animator.SetTrigger("isShooting");
        }
        */
    }
}
