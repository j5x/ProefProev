using UnityEngine;

namespace Player
{
    public class PlayerFlip : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer; // Player sprite
        [SerializeField] private Transform gun; // Gun transform

        private float horizontalInput;
        private bool facingRight = true;

        void Update()
        {
            horizontalInput = Input.GetAxis("Horizontal");
            SetupDirectionByComponent(); // Flip without affecting camera
        }

        private void SetupDirectionByComponent()
        {
            if (horizontalInput < 0 && facingRight || horizontalInput > 0 && !facingRight)
            {
                facingRight = !facingRight;

                // Flip Player Sprite
                _spriteRenderer.flipX = !facingRight;

                // Flip Gun by setting its local scale X
                Vector3 gunScale = gun.localScale;
                gunScale.x *= -1;
                gun.localScale = gunScale;
            }
        }
    }
}