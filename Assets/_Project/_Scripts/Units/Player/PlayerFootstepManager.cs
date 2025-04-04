using UnityEngine;

/******************************************
 *           FOOTSTEP MANAGER              *
 ******************************************
 * Description: Manages footstep sound    *
 * effects based on player movement, with *
 * customizable volume, pitch, and cooldown*
 ******************************************/

namespace _Project._Scripts.Units.Player
{
    public class PlayerFootstepManager : MonoBehaviour
    {
        /******************************************
         *          FOOTSTEP SOUND SETTINGS       *
         ******************************************/
        [Header("Footstep Sound Settings")]
        [SerializeField] private AudioClip[] footstepSounds;
        [SerializeField] private float footstepCooldown = 0.3f;
        [SerializeField] private float footstepVolume = 1f;
        [SerializeField] private float footstepPitch = 1f;

        private float _footstepTimer = 0f;

        /******************************************
         *          AUDIO SOURCE SETUP           *
         ******************************************/
        [Header("Audio Source Setup")]
        [SerializeField] private AudioSource audioSource;

        private void Awake()
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 0f;
        }

        private void Update()
        {
            if (_footstepTimer < footstepCooldown)
            {
                _footstepTimer += Time.deltaTime;
            }
        }

        /******************************************
         *           FOOTSTEP SOUND HANDLING     *
         ******************************************/
        public void TryPlayFootstepSound(Vector2 movementDirection)
        {
            if (movementDirection == Vector2.zero || !(_footstepTimer >= footstepCooldown)) return;
            PlayRandomFootstepSound();
            _footstepTimer = 0f;
        }

        private int _lastFootstepIndex = -1; // Store the last played index

        private void PlayRandomFootstepSound()
        {
            if (footstepSounds.Length > 1) // Ensure there's more than one sound to choose from
            {
                int newIndex;
                do
                {
                    newIndex = Random.Range(0, footstepSounds.Length);
                } while (newIndex == _lastFootstepIndex); // Keep rerolling if it's the same as last time

                _lastFootstepIndex = newIndex; // Store the new index
                AudioClip clip = footstepSounds[newIndex];
                audioSource.pitch = footstepPitch;
                audioSource.PlayOneShot(clip, footstepVolume);
            }
            else if (footstepSounds.Length == 1) // Just play the only sound available
            {
                audioSource.pitch = footstepPitch;
                audioSource.PlayOneShot(footstepSounds[0], footstepVolume);
            }
        }


        /******************************************
         *         DYNAMIC VOLUME/PITCH CONTROL  *
         ******************************************/
        public void SetFootstepVolume(float volume)
        {
            footstepVolume = Mathf.Clamp01(volume);
        }

        public void SetFootstepPitch(float pitch)
        {
            footstepPitch = Mathf.Clamp(pitch, 0.5f, 2f);
        }
    }
}
