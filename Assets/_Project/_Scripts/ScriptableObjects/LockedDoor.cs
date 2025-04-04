using UnityEngine;


namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Message", menuName = "ScriptableObjects/Messages")]
    public class LockedDoor : ScriptableObject
    {
        // Item information
        public string lockedDoorMessage;
    }
}