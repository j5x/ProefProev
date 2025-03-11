using UnityEngine;

namespace Player
{
    public class PlayerFlip : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private bool isWalking;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            HandleWalking();
        }

        private void HandleWalking()
        {
            // Check if the player is moving left or right
            isWalking = Mathf.Abs(GetComponent<Rigidbody2D>().linearVelocity.x) > 0.1f;

            if (isWalking)
            {
                // Flip sprite based on movement direction
                bool shouldFlip = GetComponent<Rigidbody2D>().linearVelocity.x < 0;
                spriteRenderer.flipX = shouldFlip;
            }
        }
    }
}