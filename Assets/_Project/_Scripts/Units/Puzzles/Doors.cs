using UnityEngine;

namespace _Project._Scripts.Units.Puzzles
{
    public class Doors : MonoBehaviour
    {
        private static readonly int OpenDoor = Animator.StringToHash("Open");
        private Animator _animator;

        private void Awake() // Use Awake instead of Start for component fetching
        {
            _animator = GetComponent<Animator>();
        }

        private void Open()
        {
            _animator.SetBool(OpenDoor, true);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger || !other.TryGetComponent(out Player.Player player)) return;

            Open();
            player.LockMovement();
        }
    }
}