using System.Collections;
using _Project._Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project._Scripts.Units.Doors
{
    public class LockedDoorSceneTransition : MonoBehaviour
    {
        // Scene transition properties
        [SerializeField] private string sceneToLoad;
        [SerializeField] private float loadingTime = 1f;
        [SerializeField] private Vector2 playerPosition;
        [SerializeField] private VectorValue playerStorage;
        
        // Transition visuals
        [SerializeField] private GameObject fadeInPanel;
        [SerializeField] private GameObject fadeOutPanel;
        
        // Player and key management
        [SerializeField] private Backpack backpack;
        [SerializeField] private DoorType doorType;
        
        private bool _playerInRange;

        private void Awake()
        {
            // Instantiate fade-in effect at game start, destroy after 1 second
            if (fadeInPanel)
            {
                Destroy(Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity), 1);
            }
        }

        private void Update()
        {
            if (_playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                TryTransition();
            }
        }

        private void TryTransition()
        {
            if (!HasRequiredKey()) return;

            playerStorage.initialValue = playerPosition;
            StartCoroutine(FadeCoroutine());
            ConsumeKey();
        }

        private bool HasRequiredKey()
        {
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
            switch (doorType)
            {
                case DoorType.Standard:
                    backpack.hasStandardKey = false;
                    break;
                case DoorType.Silver:
                    backpack.hasSilverKey = false;
                    break;
                case DoorType.Bronze:
                    backpack.hasBronzeKey = false;
                    break;
                case DoorType.Bloody:
                    backpack.hasBloodyKey = false;
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                _playerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = false;
            }
        }

        private IEnumerator FadeCoroutine()
        {
            if (fadeOutPanel)
            {
                Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
            }

            yield return new WaitForSeconds(loadingTime);

            var asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
        }
    }
}

/*
using System.Collections;
using _Project._Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project._Scripts.Units.Doors
{
    public class LockedDoorSceneTransition : MonoBehaviour
    {
        // Scenes and loading times
        public string sceneToLoad;
        public float loadingTime;
        
        // Player attributes
        public Vector2 playerPosition;
        public VectorValue playerStorage;
        
        // Transition animations
        public GameObject fadeInPanel;
        public GameObject fadeOutPanel;
        
        // External class references
        public Backpack backpack;
        public DoorType doorType;

        private bool _playerInRange;
        public void Awake()
        {
            // Instantiate fadeInPanel only if it's not null and clean up after 1 second
            if (fadeInPanel != null)
            {
                Destroy(Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity), 1);
            }
        }

        private void Update()
        {
            // Check if player is in range and presses 'E' to trigger transition
            if (_playerInRange && Input.GetKeyDown(KeyCode.E) && backpack.hasStandardKey && doorType == DoorType.Standard)
            {
                playerStorage.initialValue = playerPosition;
                StartCoroutine(FadeCoroutine());
                backpack.hasStandardKey = false;
            }
            if (_playerInRange && Input.GetKeyDown(KeyCode.E) && backpack.hasSilverKey  && doorType == DoorType.Silver)
            {
                playerStorage.initialValue = playerPosition;
                StartCoroutine(FadeCoroutine());
                backpack.hasSilverKey = false;
            }
            if (_playerInRange && Input.GetKeyDown(KeyCode.E) && backpack.hasBronzeKey && doorType == DoorType.Bronze)
            {
                playerStorage.initialValue = playerPosition;
                StartCoroutine(FadeCoroutine());
                backpack.hasBronzeKey = false;
            }
            if (_playerInRange && Input.GetKeyDown(KeyCode.E) && backpack.hasBloodyKey && doorType == DoorType.Bloody)
            {
                playerStorage.initialValue = playerPosition;
                StartCoroutine(FadeCoroutine());
                backpack.hasBloodyKey = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Only mark player as in range, don't transition immediately
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                _playerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = false;
            }
        }

        private IEnumerator FadeCoroutine()
        {
            // Instantiate fadeOutPanel and wait before loading the scene
            if (fadeOutPanel)
            {
                Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
            }

            yield return new WaitForSeconds(loadingTime);

            // Load the scene asynchronously and wait for completion
            var async = SceneManager.LoadSceneAsync(sceneToLoad);
            while (async is { isDone: false })
            {
                yield return null;
            }
        }
    }
}
*/
