using _Project._Scripts.Managers.Systems;
using _Project._Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;

/****************************************************
 *                  CHEST CLASS                    *
 ****************************************************
 * Description: This class handles the interaction *
 * with chests in the game. When the player is in  *
 * range and presses 'E', the chest opens, grants  *
 * an item, and sends a signal to notify other     *
 * systems. The chest cannot be reopened once it   *
 * has been interacted with.                       *
 *                                                  *
 * Features:                                        *
 * - Opens when the player presses 'E'             *
 * - Grants an item to the player's backpack       *
 * - Prevents multiple interactions after opening  *
 * - Sends signals upon interaction                *
 * - Displays item information via UI text         *
 ****************************************************/

namespace _Project._Scripts.Units.Chests
{
    public class Chests : Interactable
    {
        /****************************************************
         *                  EXTERNAL CLASS REFERENCES     *
         ****************************************************/
        [Header("External Class References")]
        public Thing contents;
        public Backpack backpack;
        public SignalSender collectThing;
        public BoolValue storedOpen;

        /****************************************************
         *                  PRO GUI                    *
         ****************************************************/
        [Header("ProText GUI")]
        [SerializeField] private GameObject textBox;
        [SerializeField] private TextMeshProUGUI dialogueText;

        /****************************************************
         *                  ANIMATION                     *
         ****************************************************/
        [Header("Animation")]
        private static readonly int OpenChest = Animator.StringToHash("Open");
        private Animator _animator;

        /****************************************************
         *                  INTERACTION STATE              *
         ****************************************************/
        [Header("Interaction State")]
        private bool _hasInteracted; 
        
        /****************************************************
         *                   AUDIO                          *  
         ****************************************************/
        public InteractableSoundProfile chestSoundProfile;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();

            if (storedOpen == null)
                Debug.LogWarning($"Chest '{name}' is missing its BoolValue reference!");

            if (_animator == null)
                Debug.LogError($"Animator component is missing on Chest '{name}'.");

            _hasInteracted = storedOpen != null && storedOpen.runtimeValue;

            if (_hasInteracted)
            {
                _animator.SetBool(OpenChest, true);
            }
            
            Debug.Log($"Chest '{name}' loaded | Opened: {_hasInteracted}");
        }

        
        private void Update()
        {
            if (playerInRange && !_hasInteracted && Input.GetKeyDown(KeyCode.E))
            {
                Open();
            }
        }

        private void Open()
        {
            textBox?.SetActive(true);

            if (dialogueText is not null && contents is not null)
                dialogueText.text = contents.itemName;

            if (backpack is not null && contents is not null)
            {
                backpack.AddThing(contents);
                backpack.currentThing = contents;
            }

            collectThing?.Raise();

            _hasInteracted = true;
            _animator.SetBool(OpenChest, true);

            if (storedOpen)
                storedOpen.runtimeValue = _hasInteracted;

            if (!chestSoundProfile || chestSoundProfile.interactionSounds.Length <= 0) return;
            AudioClip soundToPlay = chestSoundProfile.interactionSounds[0];
            SoundManager.Instance?.PlaySound(
                soundToPlay,
                transform.position,
                chestSoundProfile.volume,
                chestSoundProfile.pitch,
                chestSoundProfile.minDistance,
                chestSoundProfile.maxDistance
            );
        }
        
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !other.isTrigger && !_hasInteracted)
            {
                //context.Raise();
                playerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;
            playerInRange = false;

            if (textBox != null)
                textBox.SetActive(false);

            if (backpack != null)
                backpack.currentThing = null;
        }

    }
}
