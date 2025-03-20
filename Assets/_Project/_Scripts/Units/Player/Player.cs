using _Project._Scripts.Managers.Systems;
using _Project._Scripts.ScriptableObjects;
using UnityEngine;

/****************************************************
 *                  PLAYER CLASS                    *
 ****************************************************
 * Description: This class controls the player's   *
 * movement, animation, and inventory system. It   *
 * handles user input for movement, updates the    *
 * animator, and interacts with collectable items. *
 *                                                 *
 * Features:                                       *
 * - Handles player movement using Rigidbody2D     *
 * - Animates the player based on movement input   *
 * - Manages inventory and UI integration          *
 * - Detects and collects items from the game world*
 ****************************************************/

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
        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int MoveY = Animator.StringToHash("moveY");
        private static readonly int Moving = Animator.StringToHash("moving");

        private bool _isMovementLocked;
        
        [SerializeField] private float speed = 5f;
        [SerializeField] private VectorValue startPosition;
        [SerializeField] private Transform flashlightTransform;
        [SerializeField] private UIInventory uiInventory;

        private Rigidbody2D _rb;
        private Animator _animator;
        private Inventory _inventory;

        private Vector2 _movementInput;
        private Vector2 _movementDirection;
        private Quaternion _targetRotation;

        public PlayerState currentState;
        public Backpack backpack;
        public SpriteRenderer receivedThingSprite;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            currentState = PlayerState.Alive;
            InitializeInventory();
            transform.position = startPosition.initialValue;
        }

        private void Update()
        {
            if (currentState == PlayerState.Interact)
            {
                return;
            }
            ProcessInput();
            UpdateAnimation();
            RotateFlashlight();
        }

        private void FixedUpdate()
        {
            MoveCharacter();
        }

        private void LateUpdate()
        {
            flashlightTransform.rotation = Quaternion.Slerp(flashlightTransform.rotation, _targetRotation, Time.deltaTime * 10f);
        }

        private void ProcessInput()
        {
            if (_isMovementLocked)
            {
                _movementInput = Vector2.zero; // Prevent movement input
                return;
            }
            
            _movementInput.x = Input.GetAxisRaw("Horizontal");
            _movementInput.y = Input.GetAxisRaw("Vertical");

            if (_movementInput != Vector2.zero)
            {
                _movementDirection = _movementInput.normalized;
            }
        }

        private void MoveCharacter()
        {
            if (_movementInput != Vector2.zero)
            {
                _rb.MovePosition(_rb.position + _movementDirection * (speed * Time.fixedDeltaTime));
            }
        }
        public void LockMovement()
        {
            _isMovementLocked = true;
            _movementInput = Vector2.zero;
        }

        private void UpdateAnimation()
        {
            _animator.SetBool(Moving, _movementInput != Vector2.zero);

            if (_movementInput == Vector2.zero) return;
            _animator.SetFloat(MoveX, _movementInput.x);
            _animator.SetFloat(MoveY, _movementInput.y);
        }

        private void RotateFlashlight()
        {
            if (_movementInput == Vector2.zero) return;
            float angle = Mathf.Atan2(_movementInput.y, _movementInput.x) * Mathf.Rad2Deg;
            _targetRotation = Quaternion.Euler(0, 0, angle - 90);
        }

        public void CollectThing()
        {
            if (backpack.currentThing == null) return;
            if (currentState == PlayerState.Interact) return;
            currentState = PlayerState.Interact;
            receivedThingSprite.sprite = backpack.currentThing.itemSprite;
            currentState = PlayerState.Alive;
        }
        
        private void InitializeInventory()
        {
            if (uiInventory == null)
            {
                Debug.LogError("Player: UIInventory is not assigned in the Inspector!");
                return;
            }

            _inventory = new Inventory(UseItem);
            uiInventory.SetPlayer(this);
            uiInventory.SetInventory(_inventory);
        }

        private void UseItem(Item item)
        {
            if (item.itemType != Item.ItemType.Health) return;
            Debug.Log("Used health potion.");
            _inventory.RemoveItem(new Item { itemType = Item.ItemType.Health, amount = 1 });
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.TryGetComponent(out CollectableManager collectableManager)) return;
            _inventory.AddItem(collectableManager.GetItem());
            collectableManager.DestroySelf();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (currentState == PlayerState.Interact) return;
            currentState = PlayerState.Alive;
            receivedThingSprite.sprite = null;
        }
        
        public Vector2 GetPosition() => transform.position;
    }
}
