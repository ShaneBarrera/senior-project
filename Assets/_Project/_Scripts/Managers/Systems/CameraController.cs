using UnityEngine;

/****************************************************
 *               CAMERA CONTROLLER CLASS            *
 ****************************************************
 * Description: This class controls the camera's    *
 * position in the game. It continuously updates the *
 * camera's position to follow the player's current *
 * position on the X and Y axes, while maintaining a *
 * fixed Z-axis value to ensure proper perspective.  *
 *                                                 *
 * Features:                                       *
 * - Follows the player's position in the game     *
 * - Keeps the camera's Z-position fixed           *
 * - Updates the camera's position every frame     *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        private const float ZPos = -10;

        // Simple top-down camera that follows player's current position 
        private void Update()
        {
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, ZPos);
        }
    }
}