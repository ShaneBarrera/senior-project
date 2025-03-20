using System.Collections;
using _Project._Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project._Scripts.Managers.Systems
{
    public class SceneTransition : MonoBehaviour
    {
        public string sceneToLoad;
        public Vector2 playerPosition;
        public VectorValue playerStorage;
        public GameObject fadeInPanel;
        public GameObject fadeOutPanel;
        public float loadingTime;

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
            if (_playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                playerStorage.initialValue = playerPosition;
                StartCoroutine(FadeCoroutine());
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
