using UnityEngine;

/****************************************************
 *               CAMERA CONTROLLER CLASS           *
 ****************************************************
 * Description: This class controls the camera's   *
 * position in the game. It smoothly follows the   *
 * player's position to ensure fluid movement.     *
 *                                                 *
 * Features:                                       *
 * - Smoothly follows the player's position        *
 * - Uses Lerp for gradual movement                *
 * - Updates position in LateUpdate()              *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class CameraController : MonoBehaviour
    {
        // Transforms and vectors
        [SerializeField] private Transform playerTransform;
        private Vector3 _targetPosition;
        private Vector3 _velocity = Vector3.zero;
        
        // Position and speed
        private const float ZPos = -10f;
        [SerializeField] private float smoothSpeed = 1f; // Adjust for smoother camera movement

        private void LateUpdate()
        {
            if (!playerTransform) return;
    
            // Target position follows player instantly but applies small smoothing
            Vector3 targetPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, ZPos);
    
            // Use SmoothDamp for natural but quick motion
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothSpeed);
        }
    }
}