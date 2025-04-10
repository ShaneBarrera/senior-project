using _Project._Scripts.Managers.Systems;
using _Project._Scripts.ScriptableObjects;
using UnityEngine;

/****************************************************
 *                  ENEMY BEHAVIOR                 *
 ****************************************************
 * Description: This class handles the behavior of *
 * an enemy unit, including its movement and state *
 * transitions. The enemy follows the player when   *
 * within a specified chase radius and performs    *
 * animation based on its movement. It also handles*
 * locking movement when necessary.                *
 *                                                  *
 * Features:                                        *
 * - Enemy movement towards the player within the  *
 *   chase radius                                  *
 * - State management for idle and walking states  *
 * - Animation handling based on movement direction*
 * - Movement lock to prevent actions under certain*
 *   conditions                                     *
 ****************************************************/

namespace _Project._Scripts.Units.Enemy
{
    public enum EnemyState
    {
        Idle,
        Stalk,
        Walk,
        Aggressive
    }

    public class Enemy : MonoBehaviour
    {
        private static readonly int Walking = Animator.StringToHash("walking"); 
        private static readonly int MoveX = Animator.StringToHash("moveX"); 
        private static readonly int MoveY = Animator.StringToHash("moveY"); 

        private Rigidbody2D _rb; 
        private Animator _animator; 

        [Header("Enemy Behavior Profile")]
        [SerializeField] private EnemyBehaviorProfile behaviorProfile;

        [Header("Enemy Attributes")]
        public EnemyState currentState; 
        public Transform target; 

        private float _chaseRadiusSquared;
        private float _stalkRadiusSquared;
        private float _aggressiveRadiusSquared;
        private float _roamCooldown;
        private Vector2 _roamTarget;
        private bool _isMovementLocked; 

        private float _moveSpeed;
        private float _chaseRadius;
        private float _idleBufferRadius;
        private float _roamRadius;
        private float _stalkRadius;
        private bool _isAggressive;
        private bool _hasKilledPlayer;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>(); 
            _animator = GetComponent<Animator>(); 
        }

        private void Start()
        {
            if (!target)
                target = GameObject.FindGameObjectWithTag("Player")?.transform;

            if (behaviorProfile != null)
            {
                _moveSpeed = behaviorProfile.moveSpeed;
                _chaseRadius = behaviorProfile.chaseRadius;
                _roamRadius = behaviorProfile.roamRadius;
                _idleBufferRadius = behaviorProfile.idleBufferRadius;
                _stalkRadius = behaviorProfile.stalkRadius;
                _isAggressive = behaviorProfile.isAggressive;

                if (behaviorProfile.allowAttributeRandomization)
                {
                    _moveSpeed *= Random.Range(1f - behaviorProfile.moveSpeedVariance, 1f + behaviorProfile.moveSpeedVariance);
                    _chaseRadius *= Random.Range(1f - behaviorProfile.chaseRadiusVariance, 1f + behaviorProfile.chaseRadiusVariance);
                    _roamRadius *= Random.Range(0.9f, 1.1f);
                    _idleBufferRadius *= Random.Range(0.9f, 1.1f);
                    _stalkRadius *= Random.Range(0.9f, 1.1f);
                }
            }

            _chaseRadiusSquared = _chaseRadius * _chaseRadius;
            _stalkRadiusSquared = _stalkRadius * _stalkRadius;
            _aggressiveRadiusSquared = (_chaseRadius * 0.3f) * (_chaseRadius * 0.3f);

            currentState = EnemyState.Idle;
        }

        private void FixedUpdate()
        {
            if (_isMovementLocked || !target) return;

            float distanceSquared = (target.position - transform.position).sqrMagnitude;

            if (_isAggressive && distanceSquared <= _aggressiveRadiusSquared)
            {
                EnterAggressiveState();
            }
            else if (distanceSquared <= _chaseRadiusSquared * 0.5f)
            {
                EngageTarget();
            }
            else if (distanceSquared <= _chaseRadiusSquared)
            {
                EnterStalkState();
            }
            else
            {
                DisengageAndRoam();
            }
        }

        private void EngageTarget()
        {
            Vector2 direction = (target.position - transform.position).normalized;
            MoveTowardsTarget(direction, _moveSpeed);
            UpdateAnimation(direction);
            SetWalking(true);
            ChangeState(EnemyState.Walk);
        }

        private void EnterStalkState()
        {
            if (currentState != EnemyState.Stalk)
            {
                SetWalking(false);
                ChangeState(EnemyState.Stalk);
            }

            Vector2 direction = (target.position - transform.position).normalized;
            MoveTowardsTarget(direction, _moveSpeed * 0.25f);
            UpdateAnimation(direction);
        }

        private void EnterAggressiveState()
        {
            Vector2 direction = (target.position - transform.position).normalized;
            MoveTowardsTarget(direction, _moveSpeed * 1.5f);
            UpdateAnimation(direction);
            SetWalking(true);
            ChangeState(EnemyState.Aggressive);
        }

        private void DisengageAndRoam()
        {
            if (currentState != EnemyState.Idle)
            {
                SetWalking(false);
                ChangeState(EnemyState.Idle);
            }

            Roam();
        }

        private void MoveTowardsTarget(Vector2 direction, float speed)
        {
            if (_isMovementLocked) return;
            Vector2 newPosition = _rb.position + direction * (speed * Time.fixedDeltaTime);
            _rb.MovePosition(newPosition);
        }

        private void Roam()
        {
            if (Time.time >= _roamCooldown)
            {
                _roamTarget = (Vector2)transform.position + Random.insideUnitCircle * _roamRadius;
                _roamCooldown = Time.time + Random.Range(2f, 5f);
            }

            Vector2 direction = (_roamTarget - (Vector2)transform.position).normalized;
            MoveTowardsTarget(direction, _moveSpeed * 0.5f);
        }

        private void SetAnimationFloat(Vector2 setVector)
        {
            _animator.SetFloat(MoveX, setVector.x); 
            _animator.SetFloat(MoveY, setVector.y); 
        }

        private void UpdateAnimation(Vector2 direction)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y) + 0.1f)
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

        private void SetWalking(bool isWalking)
        {
            _animator.SetBool(Walking, isWalking);
        }

        public void LockMovement(bool shouldLock)
        {
            _isMovementLocked = shouldLock;

            // Safeguard in case _rb was destroyed during scene transition
            if (_rb is null) return;

            if (shouldLock)
            {
                _rb.linearVelocity = Vector2.zero;
                SetWalking(false);
                ChangeState(EnemyState.Idle);
            }
        }

        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_hasKilledPlayer) return;

            if (other.CompareTag("Player") && !other.isTrigger)
            {
                DeathManager deathManager = FindFirstObjectByType<DeathManager>();
                if (deathManager == null) return;
                deathManager.TriggerDeath();
                _hasKilledPlayer = true;
            }
        }
    }
}
