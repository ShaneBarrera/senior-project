using System.Collections;
using _Project._Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/****************************************************
 *            LOCKED DOOR SCENE TRANSITION          *
 ****************************************************
 * Description: This class handles the interaction *
 * with a locked door that, when the player is in   *
 * range and presses the 'E' key, attempts to       *
 * transition to a new scene. The player must have  *
 * the correct key for the door to allow the scene *
 * transition. If the player doesn't have the key, *
 * a locked message will be displayed.             *
 *                                                  *
 * Features:                                        *
 * - Scene transition when the correct key is used *
 * - Displays a locked message if the player lacks  *
 *   the required key                              *
 * - Fades in and out when transitioning between    *
 *   scenes                                          *
 ****************************************************/

namespace _Project._Scripts.Units.Doors
{
    public class LockedDoorSceneTransition : MonoBehaviour
    {
        /****************************************************
         *                  SERIALIZED FIELDS               *
         ****************************************************/
        [Header("Scene Transition Settings")]
        [SerializeField] private string sceneToLoad;
        [SerializeField] private float loadingTime = 1f; 
        [SerializeField] private Vector2 playerPosition; 
        [SerializeField] private VectorValue playerStorage; 
        
        [Header("UI Elements")]
        [SerializeField] private GameObject fadeInPanel; 
        [SerializeField] private GameObject fadeOutPanel; 
        [SerializeField] private GameObject textBox; 
        [SerializeField] private TextMeshProUGUI dialogueText; 
        [SerializeField] private string lockedMessage; 
        
        [Header("Key and Door Settings")]
        [SerializeField] private Backpack backpack; 
        [SerializeField] private DoorType doorType; 
        
        private bool _playerInRange; 
        
        /****************************************************
         *                  UNITY METHODS                  *
         ****************************************************/
        private void Awake()
        {
            if (fadeInPanel)
            {
                var fadeInstance = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity);
                Destroy(fadeInstance, 1); 
            }
            if (textBox != null) textBox.SetActive(false);
        }

        private void Update()
        {
            if (_playerInRange && Input.GetKeyDown(KeyCode.E)) // If the player is in range and presses 'E'
            {
                TryTransition();
            }
        }

        /****************************************************
         *                  DOOR TRANSITION LOGIC          *
         ****************************************************/
        private void TryTransition()
        {
            if (!HasRequiredKey()) // If the player doesn't have the required key
            {
                DisplayLockedMessage(); 
                return;
            }

            // Store player position and start the scene transition
            playerStorage.initialValue = playerPosition;
            StartCoroutine(FadeCoroutine());
            ConsumeKey(); 
        }

        private bool HasRequiredKey()
        {
            // Check if the player has the key associated with the door type
            return doorType switch
            {
                DoorType.Standard => backpack.hasStandardKey,
                DoorType.Silver => backpack.hasSilverKey,
                DoorType.Bronze => backpack.hasBronzeKey,
                DoorType.Bloody => backpack.hasBloodyKey,
                _ => false
            };
        }

        private void ConsumeKey()
        {
            // Consume the appropriate key from the backpack; remain true for re-entering
            switch (doorType)
            {
                case DoorType.Standard:
                    backpack.hasStandardKey = true;
                    break;
                case DoorType.Silver:
                    backpack.hasSilverKey = true;
                    break;
                case DoorType.Bronze:
                    backpack.hasBronzeKey = true;
                    break;
                case DoorType.Bloody:
                    backpack.hasBloodyKey = true;
                    break;
            }
        }

        private void DisplayLockedMessage()
        {
            // Show a message indicating the door is locked
            textBox.SetActive(true);
            dialogueText.text = lockedMessage;
        }

        /****************************************************
         *                  COLLISION HANDLERS             *
         ****************************************************/
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                _playerInRange = true; // Player enters range
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _playerInRange = false; // Player exits range
            if (textBox != null) 
            {
                textBox.SetActive(false); // Hide locked message
            }
        }

        /****************************************************
         *                  FADE COROUTINE                 *
         ****************************************************/
        private IEnumerator FadeCoroutine()
        {
            if (fadeOutPanel)
            {
                Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity); // Instantiate fade-out panel
            }

            yield return new WaitForSeconds(loadingTime); // Wait before transitioning

            // Start the scene loading asynchronously
            var asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
            while (asyncOperation is { isDone: false })
            {
                yield return null; 
            }
        }
    }
}