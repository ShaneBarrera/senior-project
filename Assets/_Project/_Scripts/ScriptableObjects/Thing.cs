using UnityEngine;

// Equivalent to an Item.cs class

namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Thing", menuName = "ScriptableObjects/Thing")]
    public class Thing : ScriptableObject
    {
        public Sprite itemSprite;
        public string itemName;
        public bool isKey;
    }
}
