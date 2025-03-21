using _Project._Scripts.ScriptableObjects;
using UnityEngine;

/****************************************************
 *               INTERACTABLE CLASS                *
 ****************************************************
 * Description: This class handles interactions    *
 * between the player and objects in the game. It  *
 * detects when the player enters or exits its     *
 * trigger zone and sends a signal accordingly.    *
 *                                                 *
 * Features:                                       *
 * - Detects player proximity using trigger events *
 * - Sends a signal when the player enters or exits*
 * - Tracks whether the player is in range         *
 ****************************************************/

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