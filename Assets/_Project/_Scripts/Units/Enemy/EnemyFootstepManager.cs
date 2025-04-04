using UnityEngine;

/******************************************
 *         ENEMY FOOTSTEP MANAGER         *
 ******************************************
 * Description: Handles spatial 3D footstep
 * sounds for enemies. Plays randomized   *
 * sounds from the enemy's world position *
 * using a spatial AudioSource.           *
 ******************************************/

namespace _Project._Scripts.Units.Enemy
{
    public class EnemyFootstepManager : MonoBehaviour
    {
        [Header("Footstep Sound Settings")]
        [SerializeField] private AudioClip[] footstepSounds;
        [SerializeField] private float footstepCooldown = 0.5f;
        [SerializeField] private float footstepVolume = 1f;
        [SerializeField] private float footstepPitch = 1f;

        [Header("3D Sound Settings")]
        [SerializeField] private float minDistance = 1f;
        [SerializeField] private float maxDistance = 15f;

        private float _footstepTimer = 0f;
        private int _lastFootstepIndex = -1;

        private void Update()
        {
            if (_footstepTimer < footstepCooldown)
                _footstepTimer += Time.deltaTime;
        }

        public void TryPlayFootstepSound(Vector2 movementDirection)
        {
            if (_footstepTimer < footstepCooldown || movementDirection == Vector2.zero)
                return;

            PlayRandomFootstepSound();
            _footstepTimer = 0f;
        }

        private void PlayRandomFootstepSound()
        {
            if (footstepSounds == null || footstepSounds.Length == 0) return;

            Debug.Log($"[EnemyFootstepManager] Playing footstep at {transform.position}");

            int newIndex;
            do
            {
                newIndex = Random.Range(0, footstepSounds.Length);
            } while (newIndex == _lastFootstepIndex && footstepSounds.Length > 1);

            _lastFootstepIndex = newIndex;
            AudioClip clip = footstepSounds[newIndex];

            // Create temporary GameObject at enemy position
            GameObject temp = new GameObject("EnemyFootstepSound");
            temp.transform.position = transform.position;

            AudioSource source = temp.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = footstepVolume;
            source.pitch = footstepPitch;
            source.spatialBlend = 1f;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = minDistance;
            source.maxDistance = maxDistance;

            source.Play();
            Destroy(temp, clip.length + 0.2f); // Cleanup
        }

        /*************** Optional Runtime Adjustments ***************/
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
