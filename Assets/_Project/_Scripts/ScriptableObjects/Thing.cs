using UnityEngine;

/****************************************************
 *                  THING CLASS                    *
 ****************************************************
 * Description: This class represents an item in   *
 * the game, which can be collected and stored in  *
 * the player's backpack. Items may include keys   *
 * and other objects.                              *
 *                                                 *
 * Features:                                       *
 * - Stores item name and sprite                   *
 * - Identifies if the item is a key               *
 * - Used as a ScriptableObject for easy data      *
 *   management and creation                       *
 ****************************************************/

namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Thing", menuName = "ScriptableObjects/Thing")]
    public class Thing : ScriptableObject
    {
        // Item information
        public Sprite itemSprite;
        public string itemName;
        
        // Validate presence items in player backpack
        public bool isStandardKey;
        public bool isSilverKey;
        public bool isBronzeKey;
        public bool isBloodyKey;
    }
}
