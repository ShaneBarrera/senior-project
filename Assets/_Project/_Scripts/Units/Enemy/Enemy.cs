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
        // Animation references
        private static readonly int Walking = Animator.StringToHash("walking");
        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int MoveY = Animator.StringToHash("moveY");

        // Components
        private Rigidbody2D _rb;
        private Animator _animator;
        
        // Enemy attributes
        public EnemyState currentState;
        public Transform target;
        //public Transform homePosition;
        public float moveSpeed;
        public float chaseRadius;
        private float _chaseRadiusSquared;
        private bool _isMovementLocked; 

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
            if (!_isMovementLocked && target)
            {
                CheckDistance();
            }
            //Debug.Log($"Enemy State: {currentState} | Position: {transform.position}");
        }

        private void CheckDistance()
        {
            if (_isMovementLocked) return; // Prevent enemy from moving when locked

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
            if (_isMovementLocked) return; 
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
        public void LockMovement()
        {
            _isMovementLocked = true;
            _rb.linearVelocity = Vector2.zero; 
            _animator.SetBool(Walking, false); 
            ChangeState(EnemyState.Idle);
        }
    }
}
