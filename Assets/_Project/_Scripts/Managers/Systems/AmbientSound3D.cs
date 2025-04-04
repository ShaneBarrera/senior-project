using _Project._Scripts.ScriptableObjects;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/****************************************************
 *                  AMBIENT SOUND 3D               *
 ****************************************************
 * Description: This class handles multiple 3D     *
 * ambient sounds using sound profiles. It allows  *
 * an enemy (or other game object) to play         *
 * multiple ambient sounds simultaneously, each    *
 * with its own settings like volume, pitch, and   *
 * spatial blend.                                  *
 *                                                 *
 * New Feature: All sounds fade in when the scene  *
 * starts.                                         *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class AmbientSound3D : MonoBehaviour
    {
        [SerializeField] private AmbientSoundProfile[] soundProfiles;
        [SerializeField] private float fadeInDuration = 1.5f;

        private readonly List<AudioSource> _audioSources = new List<AudioSource>();
        private readonly List<float> _targetVolumes = new List<float>();

        private void Awake()
        {
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
                    newSource.volume = 0f; // start at 0 for fade in
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
    }
}
