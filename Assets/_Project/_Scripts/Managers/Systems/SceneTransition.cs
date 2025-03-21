using System.Collections;
using _Project._Scripts.ScriptableObjects;
using _Project._Scripts.Units.Doors;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project._Scripts.Managers.Systems
{
    public class SceneTransition : MonoBehaviour
    {
        // Scenes and loading times
        public string sceneToLoad;
        public float loadingTime;
        
        // Player attributes
        public Vector2 playerPosition;
        public VectorValue playerStorage;
        private bool _playerInRange;
        
        // Transition animations
        public GameObject fadeInPanel;
        public GameObject fadeOutPanel;
        
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
            if (!_playerInRange || !Input.GetKeyDown(KeyCode.E)) return;
            playerStorage.initialValue = playerPosition;
            StartCoroutine(FadeCoroutine());
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
