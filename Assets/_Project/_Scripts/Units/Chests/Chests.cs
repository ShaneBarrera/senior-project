using _Project._Scripts.Managers.Systems;
using _Project._Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace _Project._Scripts.Units.Chests
{
    public class Chests : Interactable
    {
        [SerializeField] private GameObject textBox;
        [SerializeField] private TextMeshProUGUI dialogueText;
        
        private bool _hasInteracted; 
        
        private static readonly int OpenChest = Animator.StringToHash("Open");
        private Animator _animator;

        public Thing contents;
        public Backpack backpack;
        public SignalSender collectThing;
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