using UnityEngine;

namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewInteractableSoundProfile", menuName = "Audio/Interactable Sound Profile")]
    public class InteractableSoundProfile : ScriptableObject
    {
        public AudioClip[] interactionSounds;  // Array to hold different sounds for the interaction
        public float volume = 1f;
        public float pitch = 1f;
        public float minDistance = 1f;
        public float maxDistance = 50f;
    }
}