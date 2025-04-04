using System.Collections;
using UnityEngine;

/****************************************************
 *                  AUDIO TRACK MANAGER            *
 ****************************************************
 * Description: This class handles the management  *
 * of background music and ambient sounds in the   *
 * game. It provides functions to smoothly transition *
 * between tracks and play ambient sounds. The music *
 * can be faded in and out over time, and ambient   *
 * sounds can also be transitioned similarly.       *
 *                                                 *
 * Features:                                       *
 * - Plays and transitions between background music*
 * - Smooth fading of music and ambient sounds     *
 * - Allows changing ambient sounds with fade      *
 * - Provides a singleton instance for global      *
 *   access to the audio manager                   *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class AudioTrackManager : MonoBehaviour
    {
        /******************************************
         *        EXTERNAL REFERENCES             *
         ******************************************/
        public static AudioTrackManager Instance { get; private set; }

        [Header("Audio Sources (DO NOT SET)")]
        public AudioSource musicSource;
        public AudioSource nextMusicSource;
        public AudioSource ambientSource;

        /******************************************
         *              SETTINGS                  *
         ******************************************/
        [Header("Settings")]
        public float transitionTime = 2f;
        public float defaultMusicVolume = 1.0f;
        public float defaultAmbientVolume = 1.0f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            musicSource = gameObject.AddComponent<AudioSource>();
            nextMusicSource = gameObject.AddComponent<AudioSource>();
            ambientSource = gameObject.AddComponent<AudioSource>();

            musicSource.loop = true;
            nextMusicSource.loop = true;
            ambientSource.loop = true;
        }

        /******************************************
         *          PLAY MUSIC FUNCTION           *
         ******************************************/
        public void PlayMusic(AudioClip newClip, float volume = -1f)
        {
            if (musicSource.clip == newClip) return; 

            float targetVolume = (volume >= 0) ? volume : defaultMusicVolume;
            StartCoroutine(FadeToNewTrack(newClip, targetVolume));
        }

        /******************************************
         *        PLAY AMBIENT SOUND FUNCTION     *
         ******************************************/
        public void PlayAmbientSound(AudioClip newClip, float volume = -1f)
        {
            if (ambientSource.clip == newClip) return; 

            float targetVolume = (volume >= 0) ? volume : defaultAmbientVolume;
            StartCoroutine(FadeAmbientSound(newClip, targetVolume));
        }

        /******************************************
         *        FADE TO NEW TRACK FUNCTION      *
         ******************************************/
        private IEnumerator FadeToNewTrack(AudioClip newClip, float targetVolume)
        {
            nextMusicSource.clip = newClip;
            nextMusicSource.volume = 0;
            nextMusicSource.Play();

            float timer = 0f;
            float startVolume = musicSource.volume; // Store the starting volume

            while (timer < transitionTime)
            {
                timer += Time.unscaledDeltaTime;
                float t = timer / transitionTime;
                t = t * t * (3f - 2f * t); // Smooth-step easing for smoother fade

                musicSource.volume = Mathf.Lerp(startVolume, 0f, t);
                nextMusicSource.volume = Mathf.Lerp(0f, targetVolume, t);

                yield return null;
            }

            musicSource.Stop();
            musicSource.volume = 0f; // Ensure it’s fully faded out
            nextMusicSource.volume = targetVolume; // Ensure full volume
            (musicSource, nextMusicSource) = (nextMusicSource, musicSource);
        }

        /******************************************
         *        FADE AMBIENT SOUND FUNCTION     *
         ******************************************/
        private IEnumerator FadeAmbientSound(AudioClip newClip, float targetVolume)
        {
            float timer = 0f;
            float startVolume = ambientSource.volume;

            while (timer < transitionTime)
            {
                timer += Time.unscaledDeltaTime;
                ambientSource.volume = Mathf.Lerp(startVolume, 0f, timer / transitionTime);
                yield return null;
            }

            ambientSource.clip = newClip;
            ambientSource.Play();

            timer = 0f;
            while (timer < transitionTime)
            {
                timer += Time.unscaledDeltaTime;
                ambientSource.volume = Mathf.Lerp(0f, targetVolume, timer / transitionTime);
                yield return null;
            }
        }
        
        public void FadeOutMusic(float targetVolume = 0f)
        {
            StartCoroutine(FadeOutMusicCoroutine(targetVolume));
        }

        private IEnumerator FadeOutMusicCoroutine(float targetVolume)
        {
            float timer = 0f;
            float startVolume = musicSource.volume;

            while (timer < transitionTime)
            {
                timer += Time.unscaledDeltaTime;
                float t = timer / transitionTime;
                t = t * t * (3f - 2f * t);

                musicSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
                yield return null;
            }

            if (targetVolume == 0f)
            {
                musicSource.Stop();
            }
        }

    }
}
