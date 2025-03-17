using System.Collections;
using _Project._Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

/****************************************************
 *               SCENE TRANSITION SYSTEM             *
 ****************************************************
 * Description: This system handles the transition *
 * between scenes in the game. It triggers a scene *
 * change when the player enters a designated area, *
 * applies fade-in and fade-out effects, and waits *
 * for the scene to load asynchronously.            *
 *                                                  *
 * Features:                                        *
 * - Instantiates fade-in and fade-out panels      *
 * - Triggers scene transition when the player     *
 *   enters a specific area                        *
 * - Asynchronously loads the next scene after a   *
 *   delay                                           *
 * - Stores the player's position for smooth       *
 *   transitions between scenes                    *
 ****************************************************/

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

        public void Awake()
        {
            // Instantiate fadeInPanel only if it's not null and clean up after 1 second
            if (fadeInPanel != null)
            {
                Destroy(Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity), 1);
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            // Trigger scene transition only if the object is the player, and it's not a trigger
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                playerStorage.initialValue = playerPosition;
                StartCoroutine(FadeCoroutine());
            }
        }

        public IEnumerator FadeCoroutine()
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