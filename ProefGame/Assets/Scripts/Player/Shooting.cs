using UnityEngine;

namespace Player
{
    public class Shooting : MonoBehaviour
    {
        public GameObject shootingPoint; // Reference to the shooting point (in front of the player)
        public GameObject shootingMarker; // Reference to the shooting marker (arrow)
        public Camera mainCamera;

        private Vector3 markerOffset = new Vector3(-12.5f, 0f, 0f); // Offset for the marker (in front of the player)
        
        // Predefined 8 directions at 45-degree intervals
        private Vector3[] shootingDirections = new Vector3[]
        {
            new Vector3(1, 0, 0),      // 0° (Right)
            new Vector3(1, 1, 0),      // 45° (Up-right)
            new Vector3(0, 1, 0),      // 90° (Up)
            new Vector3(-1, 1, 0),     // 135° (Up-left)
            new Vector3(-1, 0, 0),     // 180° (Left)
            new Vector3(-1, -1, 0),    // 225° (Down-left)
            new Vector3(0, -1, 0),     // 270° (Down)
            new Vector3(1, -1, 0)      // 315° (Down-right)
        };

        private void Update()
        {
            // Get the mouse position in world space
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f; // Keep the Z value at 0 to stay in the 2D plane

            // Calculate the direction from the shooting point to the mouse position
            Vector3 shootingDirection = mousePos - shootingPoint.transform.position;

            // Get the closest predefined direction (snapping to one of the 8 angles)
            Vector3 closestDirection = GetClosestDirection(shootingDirection);

            // Update shooting marker position to always be in front of the player
            shootingMarker.transform.position = shootingPoint.transform.position + markerOffset;

            // Rotate the shooting marker to face the closest direction
            float angle = Mathf.Atan2(closestDirection.y, closestDirection.x) * Mathf.Rad2Deg;
            shootingMarker.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }

        // Function to get the closest direction from the predefined 8 directions (45-degree intervals)
        private Vector3 GetClosestDirection(Vector3 shootingDirection)
        {
            // Normalize the shooting direction to avoid scaling issues
            shootingDirection.Normalize();

            // Find the closest direction to the shooting direction
            Vector3 closestDirection = shootingDirections[0];
            float closestAngle = Vector3.Angle(shootingDirection, closestDirection);

            for (int i = 1; i < shootingDirections.Length; i++)
            {
                float angle = Vector3.Angle(shootingDirection, shootingDirections[i]);
                if (angle < closestAngle)
                {
                    closestAngle = angle;
                    closestDirection = shootingDirections[i];
                }
            }

            return closestDirection;
        }
    }
}
