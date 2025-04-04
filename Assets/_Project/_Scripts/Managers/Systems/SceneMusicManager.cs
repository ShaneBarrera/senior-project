using UnityEngine;

/****************************************************
 *               SCENE MUSIC MANAGER               *
 ****************************************************
 * Description: This class manages the playback of *
 * scene-specific music and ambient sounds. It     *
 * plays the designated scene music and ambient    *
 * sounds when the scene starts, allowing for a    *
 * cohesive audio atmosphere within the scene.      *
 *                                                  *
 * Features:                                        *
 * - Plays scene-specific music at the start       *
 * - Plays ambient sound to enhance the atmosphere *
 * - Supports volume control for both music and    *
 *   ambient sound                                  *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class SceneMusicManager : MonoBehaviour
    {
        /******************************************
         *           SCENE MUSIC SETTINGS         *
         ******************************************/
        [Header("Scene Music")]
        public AudioClip sceneMusic;
        [Range(0f, 1f)] public float sceneMusicVolume = 1.0f;

        /******************************************
         *           AMBIENT SOUND SETTINGS       *
         ******************************************/
        [Header("Ambient Sound")]
        public AudioClip ambientSound;
        [Range(0f, 1f)] public float ambientSoundVolume = 1.0f;
        
        private void Start()
        {
            if (sceneMusic != null)
            {
                AudioTrackManager.Instance.PlayMusic(sceneMusic, sceneMusicVolume);
            }

            if (ambientSound != null)
            {
                AudioTrackManager.Instance.PlayAmbientSound(ambientSound, ambientSoundVolume);
            }
        }
    }
}
