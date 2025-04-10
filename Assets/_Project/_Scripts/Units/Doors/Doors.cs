using System.Collections;
using _Project._Scripts.Managers.Systems;
using _Project._Scripts.ScriptableObjects;
using UnityEngine;

//Note: locked doors use signals whereas regular doors DO NOT

namespace _Project._Scripts.Units.Doors
{
    /****************************************************
     *                  DOORS CLASS                    *
     ****************************************************
     * Description: This class handles the interaction *
     * with doors in the game. The player can interact *
     * with the door when in range, and the door opens *
     * when the player presses 'E'. Locked doors may    *
     * involve additional logic with signals, whereas  *
     * regular doors simply open. The door also locks  *
     * movement for both the player and enemy when      *
     * interacted with.                                 *
     *                                                  *
     * Features:                                        *
     * - Player can interact with the door by pressing *
     *   'E' when in range                              *
     * - The door opens and locks movement for player   *
     *   and enemy                                      *
     ****************************************************/

    public class Doors : MonoBehaviour
    {
        /****************************************************
         *                  ANIMATIONS                    *
         ****************************************************/
        [Header("Animations")]
        private static readonly int OpenDoor = Animator.StringToHash("Open");
        private Animator _animator;

        /****************************************************
         *                  ENEMY REFERENCE                *
         ****************************************************/
        [Header("Enemy Reference")]
        private static Enemy.Enemy _enemy;

        /****************************************************
         *                  PLAYER ATTRIBUTES              *
         ****************************************************/
        [Header("Player Interaction Attributes")]
        private bool _playerInRange;
        private bool _hasInteracted;
        
        /****************************************************
         *                      SOUND                       *
         ****************************************************/
        public InteractableSoundProfile doorOpenSoundProfile;
        
        /****************************************************
         *                  UNITY METHODS                  *
         ****************************************************/
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
        
        private IEnumerator StartSceneTransitionAfterDelay(float delay)
        {
            foreach (var sound in AmbientSound3D.ActiveInstances)
            {
                if (sound is not null)
                    sound.FadeOutAndDestroy();
            }

            yield return new WaitForSeconds(delay);

            if (FindFirstObjectByType<SceneTransition>() is { } transition)
            {
                transition.ForceTransition();
            }
        }
        
        /****************************************************
         *                  DOOR OPEN LOGIC                *
         ****************************************************/
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
                enemy.LockMovement(true);
            }

            if (doorOpenSoundProfile && doorOpenSoundProfile.interactionSounds.Length > 0)
            {
                AudioClip soundToPlay = doorOpenSoundProfile.interactionSounds[0];
                SoundManager.Instance.PlaySound(soundToPlay, transform.position,
                    doorOpenSoundProfile.volume,
                    doorOpenSoundProfile.pitch,
                    doorOpenSoundProfile.minDistance,
                    doorOpenSoundProfile.maxDistance);
            }

            StartCoroutine(StartSceneTransitionAfterDelay(1.0f)); // adjust this to match door animation length
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _playerInRange = false;
        }
    }
}
