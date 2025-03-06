using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Shoot : MonoBehaviour
    {
        public Transform gun; // Assign Gun object in Inspector
        // Removed the Left and Right Hand logic
        // public Transform leftHandPoint; // Assign Left Hand Transform
        // public Transform rightHandPoint; // Assign Right Hand Transform
        public GameObject shootingPointPrefab; // Assign ShootingPoint prefab
        private Transform shootingPoint;
        public GameObject bulletPrefab;
        public float bulletSpeed = 10f;
        public float gunRadius = 1.5f; // The radius at which the gun will orbit the player
        public Vector2 gunOffset = new Vector2(0.5f, 0f); // The offset of the gun from the player (can be adjusted)

        private SpriteRenderer playerSpriteRenderer; // Reference to the player's SpriteRenderer
        private bool lastFlipState; // Tracks last known X-flip state

        void Start()
        {
            playerSpriteRenderer = GetComponent<SpriteRenderer>(); // Get player's SpriteRenderer

            if (shootingPointPrefab != null)
            {
                shootingPoint = Instantiate(shootingPointPrefab, gun.position, gun.rotation).transform;
                shootingPoint.SetParent(gun);
            }

            lastFlipState = playerSpriteRenderer.flipX; // Store initial flip state
            // UpdateGunHand(); // Removed as we no longer need to update based on hand
        }

        void Update()
        {
            HandleAiming();

            // Check if the player clicked to shoot
            if (Input.GetMouseButtonDown(0))
            {
                ShootBullet();
            }

            // Update the gun position around the player based on the angle
            UpdateGunPosition();
        }

        private void HandleAiming()
        {
            // Get the mouse position in world space
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 aimDirection = (worldMousePos - (Vector2)gun.position).normalized;

            // Calculate the rotation angle
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            // Rotate only the Gun (not the player)
            gun.rotation = Quaternion.Euler(0, 0, angle);

            // Flip the Gun sprite when aiming past 90° or -90°
            bool shouldFlip = angle > 90 || angle < -90;
            gun.GetComponent<SpriteRenderer>().flipY = shouldFlip;

            // Ensure the ShootingPoint rotates with the gun
            if (shootingPoint != null)
            {
                shootingPoint.rotation = gun.rotation;
            }
        }

        private void UpdateGunPosition()
        {
            // Calculate the position of the gun based on the player's position and the desired radius
            float angle = gun.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 gunPosition = new Vector2(Mathf.Cos(angle) * gunRadius, Mathf.Sin(angle) * gunRadius) + (Vector2)transform.position;

            // Apply the gun offset (adjust position slightly based on offset)
            gun.position = gunPosition + gunOffset;
        }

        // Removed the UpdateGunHand method as it's not needed now
        // private void UpdateGunHand()
        // {
        //     // Reverse logic: If player is facing left (flipX = true), place gun in the right hand
        //     gun.SetParent(!playerSpriteRenderer.flipX ? leftHandPoint : rightHandPoint, false); 
        //     // If player is facing right, place the gun in the left hand
        // }

        private void ShootBullet()
        {
            if (bulletPrefab == null || shootingPoint == null) return;

            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = shootingPoint.right * bulletSpeed; // Set bullet velocity in the direction of the gun
            }
        }
    }
}
