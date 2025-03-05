using UnityEngine;

namespace Player
{
    public class Shoot : MonoBehaviour
    {
        public Transform gun; // Assign Gun object in Inspector
        public GameObject shootingPointPrefab; // Assign ShootingPoint prefab
        private Transform shootingPoint; // The actual shooting point instance
        public GameObject bulletPrefab; // Assign Bullet prefab in the Inspector
        public float bulletSpeed = 10f;

        void Start()
        {
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
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 aimDirection = (mousePos - (Vector2)gun.position).normalized;

            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            gun.rotation = Quaternion.Euler(0, 0, angle);

            bool shouldFlip = angle > 90 || angle < -90;
            gun.GetComponent<SpriteRenderer>().flipY = shouldFlip;

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
        }
    }
}