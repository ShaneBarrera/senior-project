using UnityEngine;

/****************************************************
 *              AMBIENT SOUND PROFILE             *
 ****************************************************
 * Description: This ScriptableObject holds the    *
 * settings for an ambient sound profile, allowing *
 * for the customization of various audio attributes*
 * such as volume, pitch, and the distance range   *
 * at which the sound is audible. It can be used   *
 * to manage ambient sound effects within the game.*
 *                                                  *
 * Features:                                        *
 * - Stores an array of ambient sound clips        *
 * - Allows customization of volume and pitch      *
 * - Defines the minimum and maximum distances at  *
 *   which the sound is audible                    *
 ****************************************************/

// This uses SO for sounds; invoke in the relevant class itself 
// (i.e. chestOpen would occur in Chest.cs itself)

namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewAmbientSoundProfile", menuName = "Audio/Ambient Sound Profile")]
    public class AmbientSoundProfile : ScriptableObject
    {
        /******************************************
         *           AUDIO CLIPS & SETTINGS       *
         ******************************************/
        public AudioClip[] clips; 
        public float volume = 1f;
        public float pitch = 1f;
        public float minDistance = 1f;
        public float maxDistance = 50f;
    }
}