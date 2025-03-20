using UnityEngine;

namespace _Project._Scripts.Units.Doors
{
    public class Doors : MonoBehaviour
    {
        private static readonly int OpenDoor = Animator.StringToHash("Open");
        private Animator _animator;
        private static Enemy.Enemy _enemy; // Cache the enemy reference
        private bool _playerInRange;
        private bool _hasInteracted;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            // Cache the enemy at the start instead of searching every time
            if (_enemy == null)
                _enemy = FindFirstObjectByType<Enemy.Enemy>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;
            _playerInRange = true;
        }

        private void Update()
        {
            if (_playerInRange && !_hasInteracted && Input.GetKeyDown(KeyCode.E))
            {
                Open();
            }
        }

        private void Open()
        {
            _hasInteracted = true;
            _animator.SetBool(OpenDoor, true);

            if (FindFirstObjectByType<Player.Player>() is { } player)
            {
                player.LockMovement();
            }
            
            _enemy?.LockMovement(); // Stop the cached enemy
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _playerInRange = false;
        }
    }
}