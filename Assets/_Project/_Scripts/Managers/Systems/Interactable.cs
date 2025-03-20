using _Project._Scripts.ScriptableObjects;
using UnityEngine;

namespace _Project._Scripts.Managers.Systems
{
    public class Interactable : MonoBehaviour {

        public SignalSender context;
        public bool playerInRange;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;
            context.Raise();
            playerInRange = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;
            context.Raise();
            playerInRange = false;
        }
    }
}