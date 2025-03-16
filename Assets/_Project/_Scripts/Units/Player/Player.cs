using _Project._Scripts.Managers.Systems;
using _Project._Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

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
    public class Player : MonoBehaviour
    {
        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int MoveY = Animator.StringToHash("moveY");
        private static readonly int Moving = Animator.StringToHash("moving");

        // Variables for movement
        public float speed = 5f;
        private Rigidbody2D _rb;
        public VectorValue startPosition;
        private Vector2 _change;
        private Vector2 _lastMovement;
        private Vector2 _movementDirection;
        private Animator _animator;
        
        // Lighting
        public Transform flashlightTransform;
        private Quaternion _targetRotation;

        // Managers
        private Inventory _inventory;
        [FormerlySerializedAs("ui_inventory")] [SerializeField]
        private UIInventory uiInventory;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            SetInventory();
        }

        private void Update()
        {
            HandleInput();
            UpdateAnimation();
            RotateFlashlight();
        }

        private void FixedUpdate()
        {
            MoveCharacter();
        }

        void HandleInput()
        {
            _change.x = Input.GetAxisRaw("Horizontal");
            _change.y = Input.GetAxisRaw("Vertical");

            if (_change != Vector2.zero)
            {
                _movementDirection = _change.normalized; // Normalize to maintain consistent speed
            }
        }

        void MoveCharacter()
        {
            if (_change != Vector2.zero)
            {
                _rb.MovePosition(_rb.position + _movementDirection * (speed * Time.fixedDeltaTime));
            }
        }

        void UpdateAnimation()
        {
            if (_change != Vector2.zero)
            {
                _animator.SetFloat(MoveX, _change.x);
                _animator.SetFloat(MoveY, _change.y);
                _animator.SetBool(Moving, true);
            }
            else
            {
                _animator.SetBool(Moving, false);
            }
        }

        void SetInventory()
        {
            if (uiInventory == null)
            {
                Debug.LogError("PlayerMovement: UIInventory is not assigned in the Inspector!");
                return;
            }

            _inventory = new Inventory(UseItem);
            uiInventory.SetPlayer(this);
            uiInventory.SetInventory(_inventory);
            
            transform.position = startPosition.initialValue;
        }

        public Vector2 GetPosition()
        {
            return transform.position;
        }

        private void UseItem(Item item)
        {
            switch (item.itemType)
            {
                case Item.ItemType.Health:
                    Debug.Log("Used health potion.");
                    _inventory.RemoveItem(new Item { itemType = Item.ItemType.Health, amount = 1 });
                    break;
            }
        }

        private void RotateFlashlight()
        {
            if (_change != Vector2.zero)
            {
                float angle = Mathf.Atan2(_change.y, _change.x) * Mathf.Rad2Deg;
                _targetRotation = Quaternion.Euler(0, 0, angle - 90);
            }
        }

        private void LateUpdate()
        {
            flashlightTransform.rotation = Quaternion.Slerp(flashlightTransform.rotation, _targetRotation, Time.deltaTime * 10f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            CollectableManager collectableManager = collision.GetComponent<CollectableManager>();
            if (collectableManager != null)
            {
                _inventory.AddItem(collectableManager.GetItem());
                collectableManager.DestroySelf();
            }
        }
    }
}
