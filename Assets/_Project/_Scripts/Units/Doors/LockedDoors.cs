using _Project._Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;

// Note: Locked doors use signals, whereas regular doors do not.
namespace _Project._Scripts.Units.Doors
{
    public enum DoorType
    {
        Standard,
        Silver,
        Bronze,
        Bloody
    }

    public class LockedDoors : MonoBehaviour
    {
        // Animation Hash
        private static readonly int OpenDoor = Animator.StringToHash("Open");

        // References
        private Animator _animator;
        private static Enemy.Enemy _enemy;
        private Player.Player _player;
        
        // State
        private bool _playerInRange;
        private bool _hasInteracted;
        
        // Serialized Fields
        [SerializeField] private DoorType doorType;
        [SerializeField] private Backpack backpack;
        [SerializeField] private Thing thing;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _enemy ??= FindFirstObjectByType<Enemy.Enemy>();
            _player = FindFirstObjectByType<Player.Player>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                _playerInRange = true;
            }
        }

        private void Update()
        {
            if (_playerInRange && !_hasInteracted && Input.GetKeyDown(KeyCode.E) && HasRequiredKey())
            {
                OpenDoorAction();
            }
        }

        private bool HasRequiredKey()
        {
            return doorType switch
            {
                DoorType.Standard => backpack.hasStandardKey,
                DoorType.Silver => backpack.hasSilverKey,
                DoorType.Bronze => backpack.hasBronzeKey,
                DoorType.Bloody => backpack.hasBloodyKey,
                _ => false
            };
        }

        private void OpenDoorAction()
        {
            _hasInteracted = true;
            _animator.SetBool(OpenDoor, true);

            if (FindFirstObjectByType<Player.Player>() is { } player)
            {
                player.LockMovement();
            }
            if (FindFirstObjectByType<Enemy.Enemy>() is { } enemy)
            {
                enemy.LockMovement();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = false;
            }
        }
    }
}
