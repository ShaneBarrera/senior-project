using _Project._Scripts.Managers.Systems;
using _Project._Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace _Project._Scripts.Units.Chests
{
    /****************************************************
     *                  CHEST CLASS                    *
     ****************************************************
     * Description: This class handles the interaction *
     * with chests in the game. When the player is in  *
     * range and presses 'E', the chest opens, grants  *
     * an item, and sends a signal to notify other     *
     * systems. The chest cannot be reopened once it   *
     * has been interacted with.                       *
     *                                                 *
     * Features:                                       *
     * - Opens when the player presses 'E'             *
     * - Grants an item to the player's backpack       *
     * - Prevents multiple interactions after opening  *
     * - Sends signals upon interaction                *
     * - Displays item information via UI text         *
     ****************************************************/

    public class Chests : Interactable
    {
        // External class references
        public Thing contents;
        public Backpack backpack;
        public SignalSender collectThing;
        
        // ProText GUI
        [SerializeField] private GameObject textBox;
        [SerializeField] private TextMeshProUGUI dialogueText;
        
        // Animation
        private static readonly int OpenChest = Animator.StringToHash("Open");
        private Animator _animator;
        
        private bool _hasInteracted; 
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        private void Update()
        {
            if (playerInRange && !_hasInteracted && Input.GetKeyDown(KeyCode.E))
            {
                Open();
            }
            else
            {
                ChestOpened();
            }
        }

        private void Open()
        {
            textBox.SetActive(true);
            dialogueText.text = contents.itemName;
            backpack.AddThing(contents);
            backpack.currentThing = contents;
            collectThing.Raise();
            //context.Raise();
            _hasInteracted = true;
            _animator.SetBool(OpenChest, true);
        }

        private void ChestOpened()
        {
            collectThing.Raise();
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
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                //context.Raise();
                playerInRange = false;
                textBox.SetActive(false);
                backpack.currentThing = null;
            }
        }

    }
}