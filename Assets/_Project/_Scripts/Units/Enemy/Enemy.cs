using UnityEngine;

namespace _Project._Scripts.Units.Enemy
{
    public enum EnemyState
    {
        Idle,
        Walk
    }

    public class Enemy : MonoBehaviour
    {
        private static readonly int Walking = Animator.StringToHash("walking");
        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int MoveY = Animator.StringToHash("moveY");

        public EnemyState currentState;
        public float moveSpeed;
        public float chaseRadius;
        public Transform target;
        public Transform homePosition;

        private Rigidbody2D _rb;
        private Animator _animator;
<<<<<<< Updated upstream
        private float _chaseRadiusSquared; 
=======
        private float _chaseRadiusSquared;
        private bool _isMovementLocked; // Added flag to lock movement
>>>>>>> Stashed changes

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            currentState = EnemyState.Idle;
            target = GameObject.FindGameObjectWithTag("Player")?.transform;
            _chaseRadiusSquared = chaseRadius * chaseRadius; 
        }

        private void FixedUpdate()
        {
<<<<<<< Updated upstream
            if (target)
=======
            if (!_isMovementLocked && target)
>>>>>>> Stashed changes
            {
                CheckDistance();
            }
            Debug.Log($"Enemy State: {currentState} | Position: {transform.position}");
        }

        private void CheckDistance()
        {
<<<<<<< Updated upstream
            // Only calculate distance squared to avoid the expensive square root operation
=======
            if (_isMovementLocked) return; // Prevent enemy from moving when locked

>>>>>>> Stashed changes
            float distanceSquared = (target.position - transform.position).sqrMagnitude;

            if (distanceSquared <= _chaseRadiusSquared)
            {
                Vector2 direction = (target.position - transform.position).normalized;
                MoveTowardsTarget(direction);
                UpdateAnimation(direction);
                _animator.SetBool(Walking, true);
                ChangeState(EnemyState.Walk);
            }
            else
            {
                _animator.SetBool(Walking, false);
                ChangeState(EnemyState.Idle);
            }
        }

        private void MoveTowardsTarget(Vector2 direction)
        {
<<<<<<< Updated upstream
=======
            if (_isMovementLocked) return; // Prevent movement if locked
>>>>>>> Stashed changes
            Vector2 newPosition = _rb.position + direction * (moveSpeed * Time.fixedDeltaTime);
            _rb.MovePosition(newPosition);
        }

        private void SetAnimationFloat(Vector2 setVector)
        {
            _animator.SetFloat(MoveX, setVector.x);
            _animator.SetFloat(MoveY, setVector.y);
        }

        private void UpdateAnimation(Vector2 direction)
        {
<<<<<<< Updated upstream
            // Optimized direction determination (no need to check absolute values multiple times)
=======
>>>>>>> Stashed changes
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                SetAnimationFloat(direction.x > 0 ? Vector2.right : Vector2.left);
            }
            else
            {
                SetAnimationFloat(direction.y > 0 ? Vector2.up : Vector2.down);
            }
        }

        private void ChangeState(EnemyState newState)
        {
            if (currentState != newState)
            {
                currentState = newState;
            }
        }
<<<<<<< Updated upstream
=======
        public void LockMovement()
        {
            _isMovementLocked = true;
            _rb.linearVelocity = Vector2.zero; // Stop any current movement
            _animator.SetBool(Walking, false); // Stop walk animation
            ChangeState(EnemyState.Idle);
        }
>>>>>>> Stashed changes
    }
}
