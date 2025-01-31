using UnityEngine;

namespace _Project._Scripts.Units.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        // Movement IDs
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Speed = Animator.StringToHash("Speed");
        
        // Variables for movement
        public float moveSpeed = 5.0f;
        public Rigidbody2D rb;
        private Vector2 _movement;
        public Animator animator;
        
        // Update per frame; handle input and animation
        private void Update()
        {
            _movement.x = Input.GetAxis("Horizontal"); // -1 to 1 
            _movement.y = Input.GetAxis("Vertical"); // -1 to 1
            
            // Animate the sprite
            animator.SetFloat(Horizontal, _movement.x);
            animator.SetFloat(Vertical, _movement.y);
            animator.SetFloat(Speed, _movement.sqrMagnitude);
        }
    
        // Physics (fixed timer) 
        private void FixedUpdate()
        {
            // Constant movement speed
            rb.MovePosition(rb.position + _movement * (moveSpeed * Time.fixedDeltaTime));
        }
    }
}
