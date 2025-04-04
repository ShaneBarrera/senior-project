using UnityEngine;

//Note: locked doors use signals whereas regular doors DO NOT

namespace _Project._Scripts.Units.Doors
{
    
    public class Doors : MonoBehaviour
    {
        // Animations
        private static readonly int OpenDoor = Animator.StringToHash("Open");
        private Animator _animator;
        
        // Enemy reference
        private static Enemy.Enemy _enemy; 
        
        // Player Attributes
        private bool _playerInRange;
        private bool _hasInteracted;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            
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
            if (FindFirstObjectByType<Enemy.Enemy>() is { } enemy)
            {
                enemy.LockMovement();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _playerInRange = false;
        }
    }
}