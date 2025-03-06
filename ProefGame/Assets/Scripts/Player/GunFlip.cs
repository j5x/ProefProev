using UnityEngine;

namespace Player
{
    public class GunFlip : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private float horizontalInput;
        private bool facingRight = true;

        void Update()
        {
            horizontalInput = Input.GetAxis("Horizontal");

            SetupDirectionByComponent();
        }

        private void SetupDirectionByScale()
        {
            if (horizontalInput < 0 && facingRight || horizontalInput > 0 && !facingRight)
            {
                facingRight = !facingRight;
                Vector3 playerScale = transform.localScale;
                playerScale.x *= -1;
            }
        }

        private void SetupDirectionByComponent()
        {
            if (horizontalInput < 0)
                _spriteRenderer.flipX = true;
            else if (horizontalInput > 0)
                _spriteRenderer.flipX = false;
        }

        private void SetupDirectionByRotation()
        {
            if (horizontalInput < 0 && facingRight || horizontalInput > 0 && !facingRight)
            {
                facingRight = !facingRight;
                transform.Rotate(new Vector3(0, 180, 0));
            }
                
        }
    }
}