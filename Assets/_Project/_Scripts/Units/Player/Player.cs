using System.Runtime.CompilerServices;
using _Project._Scripts.Managers.Systems;
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
        // Movement IDs
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Speed = Animator.StringToHash("Speed");

        // Variables for movement
        public float moveSpeed = 5.0f;
        public Rigidbody2D rb;
        private Vector2 _lastMovement = Vector2.zero;
        private Vector2 _movement;
        public Animator animator;
        public Transform flashlightTransform;

        // Managers
        private Inventory _inventory;

        [FormerlySerializedAs("ui_inventory")] [SerializeField]
        private UIInventory uiInventory;

        public void Start()
        {
            // Ensure hierarchy correctly defines inventory 
            if (uiInventory == null)
            {
                Debug.LogError("PlayerMovement: UIInventory is not assigned in the Inspector!");
                return;
            }

            // Create new inventory and set
            _inventory = new Inventory(UseItem);
            uiInventory.SetPlayer(this);
            uiInventory.SetInventory(_inventory);
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");

            // Animate the sprite
            animator.SetFloat(Horizontal, _movement.x);
            animator.SetFloat(Vertical, _movement.y);
            animator.SetFloat(Speed, _movement.sqrMagnitude);

            // Only rotate the flashlight if movement direction changes
            if (_movement != Vector2.zero && _movement != _lastMovement)
            {
                RotateFlashlight();
                _lastMovement = _movement; // Update previous movement
            }
        }

        // Physics (fixed timer) 
        private void FixedUpdate()
        {
            // Constant movement speed
            rb.MovePosition(rb.position + _movement * (moveSpeed * Time.fixedDeltaTime));
        }

        // Get player current position 
        public Vector2 GetPosition()
        {
            return transform.position;
        }

        // Use inventory items
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
            if (_movement != Vector2.zero)
            {
                float angle = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg;
                flashlightTransform.rotation = Quaternion.Euler(0, 0, angle - 90);
            }
        }
        
        // Collision detection
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