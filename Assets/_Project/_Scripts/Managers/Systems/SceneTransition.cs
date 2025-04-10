using System.Collections;
using _Project._Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

/*****************************************************
 *              SCENE TRANSITION MANAGER            *
 *****************************************************
 * Handles scene transitions with fade effects and  *
 * ensures player position is stored correctly.      *
 *****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class SceneTransition : MonoBehaviour
    {
        /******************************************
         *              SCENE AND LOADING         *
         ******************************************/
        public string sceneToLoad;
        public float loadingTime;
        private bool _isTransitioning;

        /******************************************
         *              PLAYER ATTRIBUTES         *
         ******************************************/
        public Vector2 playerPosition;
        public VectorValue playerStorage;
        private bool _playerInRange;

        /******************************************
         *             TRANSITION ANIMATIONS      *
         ******************************************/
        public GameObject fadeInPanel;
        public GameObject fadeOutPanel;
        
        public void Awake()
        {
            if (!fadeInPanel) return;
            var fadeInstance = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity);
            Destroy(fadeInstance, 1); // Destroys only the new instance, not the prefab reference
        }
        
        private void Update()
        {
            // Check if player is in range and presses 'E' to trigger transition
            if (!_playerInRange || !Input.GetKeyDown(KeyCode.E)) return;
            playerStorage.initialValue = playerPosition;
            StartCoroutine(FadeCoroutine());
        }
        
        public void ForceTransition()
        {
            if (_isTransitioning) return;
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
            _isTransitioning = true;

            if (fadeOutPanel)
            {
                Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
            }

            yield return new WaitForSeconds(loadingTime);

            var async = SceneManager.LoadSceneAsync(sceneToLoad);
            while (async is { isDone: false })
            {
                yield return null;
            }
        }
    }
}
