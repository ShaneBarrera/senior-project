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

        public void Awake()
        {
            if (fadeInPanel != null)
            {
                GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
                Destroy(panel, 1);
            }
        }
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player") && !other.isTrigger)
            {
                playerStorage.initialValue = playerPosition;
                StartCoroutine(FadeCoroutine()); 
            }
        }

        public IEnumerator FadeCoroutine()
        {
            if (fadeOutPanel is not null)
            {
                Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
            }
            yield return new WaitForSeconds(loadingTime);
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);

            while (async is { isDone: false })
            {
                yield return null;
            }
        }
    }
}
