using _Project._Scripts.ScriptableObjects;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/****************************************************
 *                  AMBIENT SOUND 3D               *
 ****************************************************
 * Description: Handles multiple 3D ambient sounds *
 * that fade in when a scene starts and fade out  *
 * gracefully before transitioning to a new scene.*
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class AmbientSound3D : MonoBehaviour
    {
        [SerializeField] private AmbientSoundProfile[] soundProfiles;
        [SerializeField] private float fadeInDuration = 1.5f;
        [SerializeField] private float fadeOutDuration = 1.5f;

        private readonly List<AudioSource> _audioSources = new List<AudioSource>();
        private readonly List<float> _targetVolumes = new List<float>();
        public static readonly List<AmbientSound3D> ActiveInstances = new();
        private bool _isFadingOut;

        private void Awake()
        {
            ActiveInstances.Add(this);
            
            if (soundProfiles == null || soundProfiles.Length == 0)
            {
                Debug.LogWarning("No Ambient Sound Profiles assigned to " + gameObject.name);
                return;
            }

            foreach (var profile in soundProfiles)
            {
                if (profile == null || profile.clips == null || profile.clips.Length == 0) continue;

                foreach (var clip in profile.clips)
                {
                    AudioSource newSource = gameObject.AddComponent<AudioSource>();
                    newSource.clip = clip;
                    newSource.volume = 0f; 
                    newSource.pitch = profile.pitch;
                    newSource.spatialBlend = 1f;
                    newSource.loop = true;
                    newSource.playOnAwake = false;
                    newSource.rolloffMode = AudioRolloffMode.Linear;
                    newSource.minDistance = profile.minDistance;
                    newSource.maxDistance = profile.maxDistance;

                    _audioSources.Add(newSource);
                    _targetVolumes.Add(profile.volume);
                }
            }

            PlayAndFadeInAll();
        }

        private void OnEnable()
        {
            if (!ActiveInstances.Contains(this))
                ActiveInstances.Add(this);
        }

        private void OnDisable()
        {
            ActiveInstances.Remove(this);
        }
        
        private void PlayAndFadeInAll()
        {
            for (int i = 0; i < _audioSources.Count; i++)
            {
                _audioSources[i].Play();
                StartCoroutine(FadeInRoutine(_audioSources[i], _targetVolumes[i]));
            }
        }

        private IEnumerator FadeInRoutine(AudioSource source, float targetVolume)
        {
            float timer = 0f;
            while (timer < fadeInDuration)
            {
                timer += Time.deltaTime;
                float t = timer / fadeInDuration;
                source.volume = Mathf.Lerp(0f, targetVolume, t);
                yield return null;
            }
            source.volume = targetVolume;
        }

        public void FadeOutAndDestroy()
        {
            if (!_isFadingOut)
            {
                _isFadingOut = true;
                transform.SetParent(null); // Detach from parent so it's not destroyed with the enemy
                DontDestroyOnLoad(gameObject);
                StartCoroutine(FadeOutRoutine());
            }
        }

        private IEnumerator FadeOutRoutine()
        {
            float timer = 0f;
            List<float> startVolumes = new List<float>();
            foreach (var src in _audioSources)
                startVolumes.Add(src.volume);

            while (timer < fadeOutDuration)
            {
                timer += Time.deltaTime;
                float t = timer / fadeOutDuration;

                for (int i = 0; i < _audioSources.Count; i++)
                {
                    if (_audioSources[i] is not null)
                        _audioSources[i].volume = Mathf.Lerp(startVolumes[i], 0f, t);
                }
                yield return null;
            }

            foreach (var src in _audioSources)
                src.Stop();

            Destroy(gameObject);
        }
        
        public void FadeToVolume(float targetVolume, float duration = 1f)
        {
            foreach (var src in _audioSources)
            {
                StartCoroutine(FadeVolumeRoutine(src, targetVolume, duration));
            }
        }

        private IEnumerator FadeVolumeRoutine(AudioSource source, float target, float duration)
        {
            if (source is null) yield break;
            float start = source.volume;
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.unscaledDeltaTime;
                source.volume = Mathf.Lerp(start, target, timer / duration);
                yield return null;
            }

            source.volume = target;
        }

        private void OnDestroy()
        {
            ActiveInstances.Remove(this);
        }
    }
}