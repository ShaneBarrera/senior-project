using UnityEngine;
using System.Collections.Generic;

namespace _Project._Scripts.Managers.Systems
{
    public class SoundManager : MonoBehaviour
    {
        // Singleton pattern for accessing SoundManager
        public static SoundManager Instance { get; private set; }

        [Header("AudioSource Pool Settings")]
        public int poolSize = 10;  // Number of AudioSources in the pool
        private readonly List<AudioSource> _audioSourcePool = new List<AudioSource>();  // Pool of reusable AudioSources

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize the pool of AudioSources
            for (int i = 0; i < poolSize; i++)
            {
                AudioSource newSource = new GameObject("AudioSource_" + i).AddComponent<AudioSource>();
                newSource.transform.parent = transform;  // Optionally make them children of SoundManager for better organization
                newSource.playOnAwake = false;  // Ensure we don't play them automatically
                _audioSourcePool.Add(newSource);
            }
        }

        // Play a sound with the provided parameters (position, volume, pitch, etc.)
        public void PlaySound(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f, float minDistance = 1f, float maxDistance = 50f)
        {
            // Find an available AudioSource from the pool
            AudioSource source = GetAvailableAudioSource();
            if (!source) return; // If no available source, return (could also dynamically grow the pool if desired)

            // Configure the AudioSource properties
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.spatialBlend = 1f; // 3D sound
            source.minDistance = minDistance;
            source.maxDistance = maxDistance;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.transform.position = position; // Set position in 3D space

            // Play the sound
            source.Play();

            // Optionally, we can queue the source to be reset after the sound finishes playing
            StartCoroutine(ResetAfterDelay(source, clip.length));
        }

        // Get an available AudioSource from the pool (or null if none available)
        private AudioSource GetAvailableAudioSource()
        {
            foreach (var source in _audioSourcePool)
            {
                if (source.isPlaying) continue; // Check if the source is not currently playing a sound
                source.gameObject.SetActive(true);  // Ensure the AudioSource's game object is active
                return source;
            }

            // If no available AudioSource, you could either create a new one or just return null
            // For now, we just return null (though a dynamic pool resizing is an option here)
            return null;
        }

        // Reset AudioSource after sound finishes playing
        private System.Collections.IEnumerator ResetAfterDelay(AudioSource source, float delay)
        {
            yield return new WaitForSeconds(delay);
            source.Stop();  // Stop the sound
            source.gameObject.SetActive(false);  // Optionally disable the game object for reuse
        }
    }
}
