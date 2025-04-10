using _Project._Scripts.Managers.Systems;
using _Project._Scripts.ScriptableObjects;
using UnityEngine;

namespace _Project._Scripts.Units.Player
{
    public enum PlayerState
    {
        Alive,
        Death,
        Interact
    }

    public class Player : MonoBehaviour
    {
        /*************** ANIMATION REFERENCES ***************/
        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int MoveY = Animator.StringToHash("moveY");
        private static readonly int Moving = Animator.StringToHash("moving");

        /*************** COMPONENT REFERENCES ***************/
        private Rigidbody2D _rb;
        private Animator _animator;

        [Header("Sprite Display")]
        public SpriteRenderer receivedThingSprite;

        /*************** PLAYER ATTRIBUTES ***************/
        [Header("Player Attributes")]
        [SerializeField] private float speed = 5f;
        [SerializeField] private VectorValue startPosition;
        [SerializeField] private VectorValue startDirection;
        [SerializeField] private QuaternionValue startFlashlightRotation;
        [SerializeField] private Transform flashlightTransform;

        private Vector2 _movementInput;
        private Vector2 _movementDirection;
        private Vector2 _lastNonZeroDirection = Vector2.down; 
        private Quaternion _targetRotation;
        private bool _isMovementLocked;

        /*************** EXTERNAL SYSTEMS ***************/
        [Header("State and Inventory")]
        public PlayerState currentState;
        public Backpack backpack;

        [Header("Audio")]
        [SerializeField] private PlayerFootstepManager footstepManager;

        /*************** PROPERTIES ***************/
        private Vector2 RawInput => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        /*************** UNITY LIFECYCLE ***************/
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            currentState = PlayerState.Alive;

            if (startPosition != null)
                transform.position = startPosition.initialValue;

            if (startDirection != null)
            {
                _movementDirection = startDirection.initialValue;
                _lastNonZeroDirection = startDirection.initialValue;
                _animator.SetFloat(MoveX, _movementDirection.x);
                _animator.SetFloat(MoveY, _movementDirection.y);
            }

            if (startFlashlightRotation != null)
            {
                flashlightTransform.rotation = startFlashlightRotation.initialValue;
                _targetRotation = startFlashlightRotation.initialValue;
            }

            if (receivedThingSprite != null)
                receivedThingSprite.sprite = null;
        }

        private void Update()
        {
            if (currentState == PlayerState.Interact) return;
            ProcessInput();
            UpdateAnimation();
            RotateFlashlight();

            if (_movementInput != Vector2.zero)
                footstepManager.TryPlayFootstepSound(_movementInput);
        }

        private void FixedUpdate()
        {
            MoveCharacter();
        }

        private void LateUpdate()
        {
            flashlightTransform.rotation = Quaternion.Slerp(flashlightTransform.rotation, _targetRotation, Time.deltaTime * 10f);

            if (startFlashlightRotation)
                startFlashlightRotation.initialValue = flashlightTransform.rotation;
        }

        /*************** MOVEMENT LOGIC ***************/
        private void ProcessInput()
        {
            _movementInput = _isMovementLocked ? Vector2.zero : RawInput;

            if (_movementInput != Vector2.zero)
            {
                _movementDirection = _movementInput.normalized;
                _lastNonZeroDirection = _movementDirection;

                if (startDirection)
                    startDirection.initialValue = _lastNonZeroDirection;
            }
        }

        private void MoveCharacter()
        {
            if (_movementInput != Vector2.zero)
                _rb.MovePosition(_rb.position + _movementDirection * (speed * Time.fixedDeltaTime));
        }

        public void LockMovement()
        {
            _isMovementLocked = true;
            _movementInput = Vector2.zero;
        }

        /*************** ANIMATION LOGIC ***************/
        private void UpdateAnimation()
        {
            bool isMoving = _movementInput != Vector2.zero;
            _animator.SetBool(Moving, isMoving);

            if (!isMoving) return;
            _animator.SetFloat(MoveX, _movementInput.x);
            _animator.SetFloat(MoveY, _movementInput.y);
        }

        /*************** FLASHLIGHT LOGIC ***************/
        private void RotateFlashlight()
        {
            if (_movementInput == Vector2.zero) return;

            float angle = Mathf.Atan2(_movementInput.y, _movementInput.x) * Mathf.Rad2Deg - 90f;
            _targetRotation = Quaternion.Euler(0f, 0f, angle);
        }

        /*************** ITEM COLLECTION ***************/
        public void CollectThing()
        {
            if (backpack.currentThing == null || currentState == PlayerState.Interact)
                return;

            currentState = PlayerState.Interact;
            receivedThingSprite.sprite = backpack.currentThing.itemSprite;
            currentState = PlayerState.Alive;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (currentState == PlayerState.Interact) return;
            currentState = PlayerState.Alive;
            receivedThingSprite.sprite = null;
        }

        /*************** GETTERS ***************/
        public Vector2 GetPosition() => transform.position;
    }
}
