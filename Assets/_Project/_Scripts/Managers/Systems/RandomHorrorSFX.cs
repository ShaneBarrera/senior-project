using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Note: no SO are used, just the audio files
namespace _Project._Scripts.Managers.Systems
{
    public class RandomHorrorSFX : MonoBehaviour
    {
        [Header("Creepy Sound Settings")]
        [SerializeField] private AudioClip[] horrorClips;
        [SerializeField] private Vector2 delayRange = new Vector2(10f, 25f);
        [SerializeField] private float volume = 1f;
        [SerializeField] private float pitch = 1f;

        [Header("Fade Settings")]
        [SerializeField] private float fadeInTime = 0.5f;
        [SerializeField] private float fadeOutTime = 1.0f;

        [Header("Spatial Settings")]
        [SerializeField] private bool useRandomOffset = true;
        [SerializeField] private float radius = 5f;
        [SerializeField] private float minDistance = 1f;
        [SerializeField] private float maxDistance = 15f;

        [Header("Trigger Mode")] [SerializeField]
        private bool triggerBased;
        [SerializeField] private string triggerTag = "Player";

        [Header("Pooling Settings")]
        [SerializeField] private int poolSize = 5;
        private readonly Queue<AudioSource> _audioPool = new();

        private Coroutine _activeLoop;

        private void Awake()
        {
            // Pre-initialize the pool
            for (int i = 0; i < poolSize; i++)
            {
                GameObject go = new GameObject("HorrorAudioSource_" + i);
                go.transform.parent = this.transform;
                go.SetActive(false);

                AudioSource source = go.AddComponent<AudioSource>();
                source.spatialBlend = 1f;
                source.rolloffMode = AudioRolloffMode.Linear;
                source.minDistance = minDistance;
                source.maxDistance = maxDistance;

                _audioPool.Enqueue(source);
            }
        }

        private void Start()
        {
            if (!triggerBased)
            {
                _activeLoop = StartCoroutine(RandomHorrorLoop());
            }
        }

        private IEnumerator RandomHorrorLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(delayRange.x, delayRange.y));
                PlayRandomClip();
            }
        }

        private void PlayRandomClip()
        {
            if (horrorClips == null || horrorClips.Length == 0) return;

            AudioClip clip = horrorClips[Random.Range(0, horrorClips.Length)];
            if (!clip) return;

            AudioSource source = GetPooledSource();
            if (!source) return;

            Vector3 position = transform.position;
            if (useRandomOffset)
            {
                Vector2 offset2D = Random.insideUnitCircle.normalized * Random.Range(1f, radius);
                position += new Vector3(offset2D.x, offset2D.y, 0f);
            }

            source.transform.position = position;
            source.clip = clip;
            source.volume = 0f;
            source.pitch = pitch;
            source.loop = false;
            source.gameObject.SetActive(true);
            source.Play();

            StartCoroutine(FadeInAndOut(source, volume, fadeInTime, fadeOutTime));
        }

        private AudioSource GetPooledSource()
        {
            foreach (var src in _audioPool.Where(src => !src.isPlaying))
            {
                return src;
            }

            // Optional: Expand pool dynamically if needed
            if (poolSize < 10) // safety cap
            {
                GameObject go = new GameObject("HorrorAudioSource_" + poolSize++);
                go.transform.parent = this.transform;
                AudioSource newSource = go.AddComponent<AudioSource>();
                newSource.spatialBlend = 1f;
                newSource.rolloffMode = AudioRolloffMode.Linear;
                newSource.minDistance = minDistance;
                newSource.maxDistance = maxDistance;
                newSource.gameObject.SetActive(false);
                _audioPool.Enqueue(newSource);
                return newSource;
            }

            return null;
        }

        private IEnumerator FadeInAndOut(AudioSource source, float targetVolume, float fadeInDuration, float fadeOutDuration)
        {
            float timer = 0f;

            // Fade in
            while (timer < fadeInDuration)
            {
                timer += Time.deltaTime;
                source.volume = Mathf.Lerp(0f, targetVolume, timer / fadeInDuration);
                yield return null;
            }

            source.volume = targetVolume;
            yield return new WaitForSeconds(source.clip.length - fadeInDuration - fadeOutDuration);

            // Fade out
            timer = 0f;
            float startVolume = source.volume;
            while (timer < fadeOutDuration)
            {
                timer += Time.deltaTime;
                source.volume = Mathf.Lerp(startVolume, 0f, timer / fadeOutDuration);
                yield return null;
            }

            source.Stop();
            source.gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!triggerBased || !other.CompareTag(triggerTag)) return;

            _activeLoop ??= StartCoroutine(RandomHorrorLoop());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!triggerBased || !other.CompareTag(triggerTag)) return;

            if (_activeLoop == null) return;
            StopCoroutine(_activeLoop);
            _activeLoop = null;
        }
    }
}
